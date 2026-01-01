using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public struct Constants
{
    public const int MUSIC = 0;
    public const int POKE = 1;
    public const int MISC1 = 2;
    public const int MISC2 = 3;
    public const int MISC3 = 4;
    public const int MISC4 = 5;
    public const int UI1 = 6;
    public const int UI2 = 7;

    public const string MALE = "♂";
    public const string FEMALE = "♀";
}

public class GameManager : MonoBehaviour
{
    public bool debugging;
    

    public int forceEnemyIndex = -1;
    public int forcePlayerIndex = -1;
    public float playerActDelay = 1f;

    public static bool inMenu;
    public static GameManager instance;

    public SceneType currentScene;
    public Area currentArea;
    public SoundAsset WorldMusic;
    public SoundAsset BattleMusic;
    

    public List<PokemonInstance> playerPokemons;
    public List<PokemonInstance> enemyPokemons;

    public int currentActivePokemon;


    public PokemonBase[] testPlayer;
    public int TestPlayerLVL;

    

    Transform pokemonInstanceParent;
  //  Player_Map playerMap;
    public BattleManager battleManager { get; private set; }
    public MenuManager menuManager { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        SceneManager.sceneLoaded += OnSceneChange;
        SceneManager.sceneUnloaded += OnSceneChange;
    }
    private void OnEnable()
    {        
        //playerMap = FindObjectOfType<Player_Map>();

        pokemonInstanceParent = transform.Find("Pokemon");

        List<int> ranList = new List<int>() { 0,1,2};

        int rand = Random.Range(0, ranList.Count);
        int pPoke = ranList[rand];

        if (forcePlayerIndex > -1)
            pPoke = forcePlayerIndex;

        PokemonInstance p1 = CreateNewPokemonInstance(testPlayer[pPoke], TestPlayerLVL);
        ranList.Remove(rand);

        rand = Random.Range(0, ranList.Count);
        pPoke = ranList[rand];

        PokemonInstance p2 = CreateNewPokemonInstance(testPlayer[pPoke], TestPlayerLVL);
        ranList.Remove(rand);

        rand = Random.Range(0, ranList.Count);
        pPoke = ranList[rand];

        PokemonInstance p3 = CreateNewPokemonInstance(testPlayer[pPoke], TestPlayerLVL);
        ranList.Remove(rand);


        p1.SetAllyStatus(true);
        p2.SetAllyStatus(true);
        p3.SetAllyStatus(true);


        playerPokemons.Add(p1);
        playerPokemons.Add(p2);
        playerPokemons.Add(p3);
    }
    private void Update()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.T))
        {
            ExitEncouter();
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.X))
        {
            EncouterPokemon();
        }
        
    }
    //Called before OnStart and after OnEnable
    //On Scene Loaded
    void OnSceneChange(Scene newScene, LoadSceneMode sceneLoadMode)
    {    
        if (currentScene == SceneType.Battle)
        {
            if(debugging)
            {
                

                for (int i = 0; i < 3; i++)
                {
                    int ran = Random.Range(0, 3);
                    int ran2 = Random.Range(0, 11);
                    int ran3 = Random.Range(1, 3);

                    if (forcePlayerIndex > -1)
                        ran = forceEnemyIndex;

                    PokemonInstance inst = CreateNewPokemonInstance(testPlayer[ran], ran2 / ran3 + 1);
                    enemyPokemons.Add(inst);
                }
               

                AudioManager.instance.PlayExternal(BattleMusic.sound.name);               
            }

            battleManager = FindObjectOfType<BattleManager>();
            BattleManager.instance = battleManager;
            battleManager.SetupBattle(1);

        }
        else if (currentScene == SceneType.Menu)
        {
            //screenEffects.Play("open");

            menuManager = FindObjectOfType<MenuManager>();
            menuManager.SetupMenu(MenuState.POKEMON);
        }
        else
        {
            AudioManager.instance.PlayExternal(WorldMusic.sound.name);
            World.Fade(true);
            //playerMap.canMove = true;
        }
    }
    //On Scene UnLoaded

    void OnSceneChange(Scene newScene)
    {
        instance.enemyPokemons.RemoveAll(item => item == null);
        AudioManager.instance.PlayExternal(WorldMusic.sound.name);
        World.Fade(true);
        playerPokemons[0].Data().Heal();
        //playerMap.canMove = true;
    }
    static void RemovePokemonInstance(PokemonInstance instance)
    {
        Destroy(instance.gameObject);
    }
    PokemonInstance CreateNewPokemonInstance(PokemonBase pBase, int lvl)
    {
        PokemonInstance pInstance = new GameObject(pBase.name).AddComponent<PokemonInstance>();
        pInstance.Setup(new Pokemon(pBase, lvl));
        pInstance.transform.parent = pokemonInstanceParent;
        pInstance.transform.localPosition = Vector3.zero;

        return pInstance;
    }

    public static void OpenMenuScene()
    {
        World.Fade(false);
        instance.StartCoroutine(instance.LoadMenuScene());

    }
    public static void EncouterPokemon()
    {
        AudioManager.instance.Stop(Constants.MUSIC);
        AudioManager.instance.PlayExternal(instance.BattleMusic.sound.name);

        PokemonEncounter encounter = instance.currentArea.encounters.GetPokemon();

        PokemonBase pkmn = encounter.pokemon;
        int lvl = encounter.GetLevel();

        if (lvl < instance.playerPokemons[0].Data().Level) World.DoBattleTransition(World.BattleTransition.WildWeak);
        else World.DoBattleTransition(World.BattleTransition.WildStrong);

        PokemonInstance inst = instance.CreateNewPokemonInstance(pkmn, lvl);
        instance.enemyPokemons.Add(inst);



        instance.StartCoroutine(instance.LoadBattleScene());
    }
    public static void ExitEncouter()
    {
        if(instance.debugging)
        {
            SceneManager.LoadScene(1);
            return;
        }

        foreach (var inst in instance.enemyPokemons)
        {           
            RemovePokemonInstance(inst);
        }

        instance.StartCoroutine(instance.LoadWorldScene());
    }
    
    public IEnumerator LoadWorldScene()
    {

        yield return new WaitForSeconds(1f);
        SceneManager.UnloadSceneAsync(1);
        currentScene = SceneType.World;
        World.SetActive(true);
    }
    public IEnumerator LoadBattleScene()
    {
        yield return new WaitForSeconds(2f);
        World.SetActive(false);
        currentScene = SceneType.Battle;
        SceneManager.LoadScene(1,LoadSceneMode.Additive);
    }
    public IEnumerator LoadMenuScene()
    {
        yield return new WaitForSeconds(2f);
        World.SetActive(false);
        currentScene = SceneType.Menu;
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }
}

public enum SceneType
{
    World,
    Battle,
    Menu
}
