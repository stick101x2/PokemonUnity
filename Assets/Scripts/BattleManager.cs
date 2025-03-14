using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
}
public class BattleManager : MonoBehaviour
{
    public bool skipIntro;
    [SerializeField] Animator arena;
    [SerializeField] Animator screenEffects;
    [SerializeField] MouseUiHandler mouseHandle;
    [SerializeField] BattleUnitInfo playerUI;
    [SerializeField] BattleUnitInfo enemyUI;
    [SerializeField] DialogBox messegeBox;
    public DialogBox Messenger { get { return messegeBox; } }
    [SerializeField] BattleUIActions playerActions;
    [SerializeField] BattleMoveMenu playerMoves;

    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;

    [SerializeField] BattleState state = BattleState.START;
    [SerializeField] BattleMenu menu = BattleMenu.OTHER;
    public Damage damagedInfo;
    public void SetupBattle()
    {      
        playerUnit.Setup(playerUI, GameManager.instance.playerPokemons[0]);
        enemyUnit.Setup(enemyUI, GameManager.instance.enemyPokemons[0]);

        PokemonInstance pp = playerUnit.pokemon;
        PokemonInstance ep = enemyUnit.pokemon;

        playerUI.SetData(pp.Data());
        enemyUI.SetData(ep.Data());

        state = BattleState.START;

        playerActions.Close();
        playerMoves.Close();

        playerUnit.Animate("wait");
        enemyUnit.Animate("wait");
        playerUnit.UI.AnimIntro(true);
        enemyUnit.UI.AnimIntro(false);
        playerUnit.Animate("inactive");

        messegeBox.SetDialog("");
        screenEffects.Play("open");

        StartCoroutine(Battle());
        
    }

    IEnumerator Battle()
    {
        if(!skipIntro)
        {
            yield return new WaitForSeconds(2f);
            enemyUnit.UI.AnimEnter(false);
            AudioManager.Play(enemyUnit.pokemon.Data().Cry, Constants.POKE);
            yield return messegeBox.TypeDialog($"A wild {enemyUnit.pokemon.Data().Name.ToUpper()} appeared!", true);
            playerUnit.UI.AnimEnter(true);
            arena.Play("player_enter");
            yield return messegeBox.TypeDialog($"Go!    {playerUnit.pokemon.Data().Name.ToUpper()}!");
            yield return new WaitForSeconds(1f);
            AudioManager.Play("ball_open", Constants.MISC1);
            arena.Play("player_poke_enter");
            yield return new WaitForEndOfFrame();
            playerUnit.Animate("spawn");
            AudioManager.Play(playerUnit.pokemon.Data().Cry, Constants.POKE);
            yield return new WaitForSeconds(1f);
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

    IEnumerator UnitAct(Move move)
    {
        playerUnit.Animate("wait");
        enemyUnit.Animate("wait");
        playerUnit.UI.AnimStatic();

        string actorName = state == BattleState.PLAYER_TURN ? playerUnit.pokemon.Data().Name : enemyUnit.pokemon.Data().Name;
        BattleUnit offense = state == BattleState.PLAYER_TURN ? playerUnit : enemyUnit;
        BattleUnit defense = state == BattleState.PLAYER_TURN ? enemyUnit : playerUnit;
        yield return messegeBox.TypeDialog($"{actorName} used {move.Base.Name}!");
        if (!AccuracyCheck(move))
        {
            yield return new WaitForSeconds(0.25f);
            yield return messegeBox.TypeDialog($"{offense.pokemon.Data().Name}'s attack missed!");
            yield return new WaitForSeconds(0.5f);
            SwapTurn();
            yield break;
        }

        yield return new WaitForSeconds(0.1f);
        yield return move.Act(this, offense, defense);

        if (damagedInfo.fainted)
        {
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
        SwapTurn();
    }

    void SwapTurn()
    {
        StopAllCoroutines();
        if(state == BattleState.PLAYER_TURN)
        {
           
            EnemyAct();
           
        }else
        {
            PlayerAct();
        }    
    }
    // --- ENEMY FUNCTIONS ---

    void EnemyAct()
    {
        state = BattleState.ENEMY_TURN;
        Move move = enemyUnit.pokemon.Data().GetRandomMove();
        StartCoroutine(UnitAct(move));
    }
    // --- PLAYER FUNCTIONS ---
    public void PerformPlayerMove(Move move)
    {
        state = BattleState.PLAYER_TURN;
        playerActions.Close();
        playerMoves.Close();
        menu = BattleMenu.OTHER;

        StartCoroutine(UnitAct(move));
    }
    void PlayerAct()
    {
        state = BattleState.PLAYER_ACTION;
        menu = BattleMenu.ACTION;

        messegeBox.InstantDialog($"What will {playerUnit.pokemon.Data().Name} do?",0.05f);
        mouseHandle.ClearRects();
        mouseHandle.AddRects(playerActions.items);
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
        mouseHandle.AddRects(playerMoves.items);
        playerActions.Close();

        playerMoves.player = playerUnit;
        playerMoves.Open();
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
            default:
                break;
        }
    }
    
}
