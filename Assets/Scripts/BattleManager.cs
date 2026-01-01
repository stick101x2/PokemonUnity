using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public enum BattleState
{
    START,
    PLAYER_ACTION,
    PLAYER_TURN,
    ENEMY_TURN,
    IDLE
}
public enum BattleMenu
{
    OTHER,
    ACTION,
    MOVE,
    POKEMON,
    ITEM,
    POKE_SELECT,

}
public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;


    public bool skipIntro;
    [SerializeField] int battleSize = 1;
   
    [SerializeField] Animator arena;
    [SerializeField] Animator screenEffects;
    [SerializeField] Animator battleUiAnimations;

    [SerializeField] MouseUiHandler mouseHandle;

    [SerializeField] BattleUnitInfo playerUI;
    [SerializeField] BattleUnitInfo enemyUI;

    [SerializeField] List<BattleUnitInfo> playerUIs;
    [SerializeField] List<BattleUnitInfo> enemyUIs;

    [SerializeField] DialogBox messegeBox;
    [SerializeField] BattleUIActions playerActions;
    [SerializeField] BattleMoveMenu playerMoves;

    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;

    [SerializeField] List<BattleUnit> playerUnits;
    [SerializeField] List<BattleUnit> enemyUnits;

    [SerializeField] BattlePositions allySinglePositions;
    [SerializeField] BattlePositions allyDoublePositions;

    [SerializeField] BattlePositions enemySinglePositions;
    [SerializeField] BattlePositions enemyDoublePositions;

    [SerializeField] BattleState state = BattleState.START;
    [SerializeField] BattleMenu menu = BattleMenu.OTHER;
    [SerializeField] Sprite status_attack;


    [SerializeField] Sprite maleIcon;
    [SerializeField] Sprite femaleIcon;
    [SerializeField] Sprite genderlessIcon;

    public Damage damagedInfo;

    [SerializeField] List<BattleUnit> moveOrder;

    int currentUnit;
    int targetIndex;

    bool moved;
    public DialogBox Messenger { get { return messegeBox; } }

    public void SetupBattle(int battleSize)
    {
        this.battleSize = battleSize;
        //playerUnit.Setup(playerUI, GameManager.instance.playerPokemons[0]);
        // enemyUnit.Setup(enemyUI, GameManager.instance.enemyPokemons[0]);

        int uiOffset = 0;
        BattlePositions allyPokePositions = allySinglePositions;
        BattlePositions enemyPokePositions = enemySinglePositions;

        playerUIs[0].gameObject.SetActive(true);
        playerUIs[1].gameObject.SetActive(false);
        playerUIs[2].gameObject.SetActive(false);

        enemyUIs[0].gameObject.SetActive(true);
        enemyUIs[1].gameObject.SetActive(false);
        enemyUIs[2].gameObject.SetActive(false);

        if (battleSize == 2)
        {
            uiOffset = 1;
            allyPokePositions = allyDoublePositions;
            enemyPokePositions = enemyDoublePositions;

           

            playerUIs[0].gameObject.SetActive(false);
            playerUIs[1].gameObject.SetActive(true);
            playerUIs[2].gameObject.SetActive(true);

            enemyUIs[0].gameObject.SetActive(false);
            enemyUIs[1].gameObject.SetActive(true);
            enemyUIs[2].gameObject.SetActive(true);
        }

        allyPokePositions.Setup();
        enemyPokePositions.Setup();

        for (int i = 0; i < battleSize; i++)
        {   
            playerUnits[i].Setup(playerUIs[i + uiOffset], GameManager.instance.playerPokemons[i], allyPokePositions.GetPosition(i));
            enemyUnits[i].Setup(enemyUIs[i + uiOffset], GameManager.instance.enemyPokemons[i], enemyPokePositions.GetPosition(i));

            PokemonInstance pp = playerUnits[i].pokemon;
            PokemonInstance ep = enemyUnits[i].pokemon;

            playerUIs[i + uiOffset].SetData(pp.Data(), this);
            enemyUIs[i + uiOffset].SetData(ep.Data(), this);
        }


        state = BattleState.START;

        playerActions.Close();
        playerMoves.Close();

        for (int i = 0; i < battleSize; i++)
        {
            playerUnits[i].Animate("wait");
            enemyUnits[i].Animate("wait");
            playerUnits[i].UI.AnimIntro(true);
            enemyUnits[i].UI.AnimIntro(false);
            playerUnits[i].Animate("inactive");

            if (battleSize == 2)
            {
                playerUnits[i].UI.ShowAllInfo(false);
            }
        }
      
        

       

        messegeBox.SetDialog("");
        screenEffects.Play("open");

        StartCoroutine(Battle());
        
    }

    IEnumerator Battle()
    {
        if (!skipIntro)
        {
            yield return new WaitForSeconds(2.16666666667f);
            yield return new WaitForSeconds(2f);
            
            for (int i = 0; i < battleSize; i++)
            {
                enemyUnits[i].UI.AnimEnter(false);
                enemyUnits[i].PlayPokemonAnimation();
            }

            AudioManager.Play(enemyUnits[0].pokemon.Data().Cry, Constants.POKE);

            if (battleSize == 2)
            {
                AudioManager.Play(enemyUnits[1].pokemon.Data().Cry, Constants.MISC4);
            }

            yield return messegeBox.TypeDialog($"A wild {enemyUnit.pokemon.Data().Name} appeared!", true);

            for (int i = 0; i < battleSize; i++)
            {
                playerUnits[i].UI.AnimEnter(true);
            }
           
            arena.Play("player_enter");
            yield return messegeBox.TypeDialog($"Go! {playerUnit.pokemon.Data().Name}!");
            yield return new WaitForSeconds(1.2f);

            AudioManager.Play("ball_open", Constants.MISC1);
            arena.Play("player_poke_enter");
            yield return new WaitForEndOfFrame();

            for (int i = 0; i < battleSize; i++)
            {
                playerUnits[i].Animate("spawn");
            }

            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < battleSize; i++)
            {
                playerUnits[i].PlayPokemonAnimation();
            }

            AudioManager.Play(playerUnits[0].pokemon.Data().Cry, Constants.POKE);
            if(battleSize == 2)
            {
                AudioManager.Play(playerUnits[1].pokemon.Data().Cry, Constants.MISC4);
            }

            yield return new WaitForSeconds(GameManager.instance.playerActDelay);
        }
        else
        {
            arena.Play("player_poke_enter");
            playerUnit.Animate("spawn");
            playerUnit.UI.AnimEnter(true);
            enemyUnit.UI.AnimEnter(false);
        }

        
        PlayerAct();
    }

    IEnumerator UnitAct(Move move, BattleUnit offense, BattleUnit defense)
    {

        string actorName = offense.pokemon.Data().Name;

        yield return messegeBox.TypeDialog($"{actorName} used {move.Base.Name}!");
        if (!AccuracyCheck(move))
        {
            yield return new WaitForSeconds(0.25f);
            yield return messegeBox.TypeDialog($"{offense.pokemon.Data().Name}'s attack missed!");
            yield return new WaitForSeconds(0.5f);
            yield break;
        }

        yield return new WaitForSeconds(0.1f);
        yield return move.Act(this, offense, defense);

        if (damagedInfo.fainted)
        {
            AudioManager.Play(defense.pokemon.Data().Cry, Constants.POKE, 0.9f);
            yield return new WaitForSeconds(0.5f);
            if (state == BattleState.ENEMY_TURN)
                defense.Animate("faint_p");
            else
                defense.Animate("faint");
            defense.UI.AnimFaint();


            yield return new WaitForSeconds(0.5f);
            yield return messegeBox.TypeDialog($"{defense.pokemon.Data().Name} fainted!");
            yield return new WaitForSeconds(1f);
            screenEffects.Play("fade_out");
            GameManager.ExitEncouter();
            yield break;
        }

        yield return new WaitForSeconds(0.25f);
    }

    void EndTurn()
    {
        StopAllCoroutines();

        playerUnit = playerUnits[0];
        enemyUnit = enemyUnits[0];

        currentUnit = 0;
        targetIndex = 0;

        PlayerAct();
    }
    // --- ENEMY FUNCTIONS ---

    void EnemyAct()
    {
        

        Move move = enemyUnit.pokemon.Data().GetRandomMove();
        enemyUnit.moveToUse = move;

        enemyUnit.unitToTarget = playerUnits[Random.Range(0, battleSize)];

        if (battleSize > 1 && currentUnit < battleSize-1)
        {
            currentUnit++;

            enemyUnit = enemyUnits[currentUnit];

            EnemyAct();

            return;
        }

        //StartCoroutine(messegeBox.TypeDialog($"Proccesing Turns Now!"));
        
        DecideTurnOrder();


        StartCoroutine(ProccessTurnOrder());
    }

    IEnumerator ProccessTurnOrder()
    {
        for (int i = 0; i < moveOrder.Count; i++)
        {
            yield return UnitAct(moveOrder[i].moveToUse, moveOrder[i], moveOrder[i].unitToTarget);
        }

        EndTurn();
    }
    // --- PLAYER FUNCTIONS ---
    public void PerformPlayerMove(Move move)
    {
        if (menu == BattleMenu.POKE_SELECT)
            return;

        playerUnit.moveToUse = move;

        if(battleSize > 1)
        {
            SelectPokemonDoubleBattle();
            return;
        }


        StartEnemyAct();


        //StartCoroutine(UnitAct(move));
    }

    public void StartEnemyAct()
    {
        state = BattleState.PLAYER_TURN;
        battleUiAnimations.Play("none");

        playerUnit.Animate("wait");
        playerUnit.UI.AnimStatic();

        playerActions.Close();
        playerMoves.Close();
        menu = BattleMenu.OTHER;
       
        currentUnit = 0;
        EnemyAct();     
    }
    public void SelectPokemonDoubleBattle()
    {
        state = BattleState.PLAYER_ACTION;
        menu = BattleMenu.ACTION;

        battleUiAnimations.Play("none");
        menu = BattleMenu.POKE_SELECT;

        playerUnit.Animate("wait");
        playerUnit.UI.AnimStatic();
        messegeBox.SetDialog("Select Target");

        EnemySelect();
    }
    public void DecideTurnOrder()
    {
        moveOrder.Clear();
        for (int i = 0; i < battleSize; i++)
        {
            moveOrder.Add(playerUnits[i]);
        }
        for (int i = 0; i < battleSize; i++)
        {
            moveOrder.Add(enemyUnits[i]);
        }

        moveOrder.Sort(SortBySpeed);
        moveOrder.Reverse();
    }


    private void Update()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.Y))
        {
            DecideTurnOrder();
        }

        if (menu == BattleMenu.POKE_SELECT) 
        {
            PlayerPokeSelect();
        }
    }

    static int SortBySpeed(BattleUnit u1, BattleUnit u2)
    {
        int compare1 = u1.GetSpeed().CompareTo(u2.GetSpeed());
        
        if(compare1 == 0)
        {
            int random = UnityEngine.Random.Range(0, 101);
            if(random > 50)
            {
                compare1 = 1;
            }else
            {
                compare1 = -1;
            }
        }

         return compare1;
    }

    void PlayerAct()
    {
        state = BattleState.PLAYER_ACTION;
        menu = BattleMenu.ACTION;
        battleUiAnimations.Play("action_open");
        AudioManager.Play("slide_in", Constants.MISC1);

        messegeBox.InstantDialog($"What will {playerUnit.pokemon.Data().Name} do?",0.05f);
        mouseHandle.ClearRects();
        mouseHandle.AddRects(playerActions.MenuItems);
        playerMoves.Close();

        playerActions.player = playerUnit;
        playerActions.Open();

        playerUnit.Animate("idle");
        playerUnit.UI.AnimIdle();
    }
    void PlayerMove()
    {
        state = BattleState.PLAYER_ACTION;
        menu = BattleMenu.MOVE;
        mouseHandle.ClearRects();
        mouseHandle.AddRects(playerMoves.MenuItems);
        playerActions.Close();

        playerMoves.player = playerUnit;
        playerMoves.Open();
    }

    void PlayerPokeSelect()
    {
        Vector2 dir = UserInput.GetDpad();

        if(Keyboard.current.pKey.wasPressedThisFrame)
        {
            playerUnit.unitToTarget = enemyUnits[targetIndex];

            playerUnit.Animate("wait");
            playerUnit.UI.AnimStatic();

            currentUnit++;

            if (currentUnit < battleSize)
            {

                playerUnit = playerUnits[currentUnit];

                EnemySelectEnd();
                PlayerAct();
                return;
            }

            EnemySelectEnd();
            StartEnemyAct();
            
            return;
        }

        if (dir.magnitude < 0.1f)
            moved = false;

        if (moved)
            return;

        if (dir.x > 0.1f)
            targetIndex++;
        else if(dir.x < -0.1f)
            targetIndex++;

        if(targetIndex >= battleSize)       
            targetIndex = 0;
        else if (targetIndex < 0)
            targetIndex = battleSize - 1;

       
       

        if(dir.magnitude > 0.1f)
        {
            moved = true;
            EnemySelect();


        }
    }
    public void EnemySelectEnd()
    {
        for (int i = 0; i < battleSize; i++)
        {          
            enemyUnits[i].UI.AnimStatic();
            enemyUnits[i].Animate("wait");
        }
    }

    public void EnemySelect()
    {
        for (int i = 0; i < battleSize; i++)
        {
            if (targetIndex == i)
            {
                enemyUnits[i].Animate("selected");
                enemyUnits[i].UI.AnimIdle();
                continue;
            }
            enemyUnits[i].UI.AnimStatic();


            enemyUnits[i].Animate("wait");

        }
    }


    bool AccuracyCheck(Move move)
    {
        int rand = Random.Range(0, 101);
        if (move.Base.Accuracy < rand)
            return false;
        return true;
    }
    
    public BattleMenu GetMenu()
    {
        return menu;
    }
    public BattleState GetState()
    {
        return state;
    }

    public Sprite GetStatusEffect()
    {
        return status_attack;
    }
    public Sprite GetGenderIcon(int gender)
    {
        Sprite genderSprite = genderlessIcon;
        if (gender == 0)
            genderSprite = maleIcon;
        else if (gender == 1)
            genderSprite = femaleIcon;

        return genderSprite;
    }
    void SwitchState()
    {

    }
    void SwitchMenu(int nextMenu)
    {
        SwitchMenu((BattleMenu)nextMenu);
    }
    void SwitchMenu(BattleMenu nextMenu)
    {
        switch (nextMenu)
        {
            case BattleMenu.OTHER:
                break;
            case BattleMenu.ACTION:
                PlayerAct();
                break;
            case BattleMenu.MOVE:
                PlayerMove();
                break;
            case BattleMenu.POKEMON:
                break;
            case BattleMenu.ITEM:
                break;
            case BattleMenu.POKE_SELECT:
                break;
            default:
                break;
        }
    }
    
}
