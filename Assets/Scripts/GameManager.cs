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
    Player_Map playerMap;
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
        playerMap = FindObjectOfType<Player_Map>();
        pokemonInstanceParent = transform.Find("Pokemon");
        PokemonInstance p1 = CreateNewPokemonInstance(testPlayer[0], TestPlayerLVL);
        PokemonInstance p2 = CreateNewPokemonInstance(testPlayer[1], TestPlayerLVL);
        PokemonInstance p3 = CreateNewPokemonInstance(testPlayer[2], TestPlayerLVL);
        p1.SetAllyStatus(true);
        p2.SetAllyStatus(true);
        p3.SetAllyStatus(true);
        playerPokemons.Add(p1);
        playerPokemons.Add(p2);
        playerPokemons.Add(p3);
    }

    //Called before OnStart and after OnEnable
    //On Scene Loaded
    void OnSceneChange(Scene newScene, LoadSceneMode sceneLoadMode)
    {    
        if (currentScene == SceneType.Battle)
        {
            if(debugging)
            {
                AudioManager.instance.PlayExternal(BattleMusic.sound.name);               
            }

            battleManager = FindObjectOfType<BattleManager>();
            battleManager.SetupBattle();

        }
        else if (currentScene == SceneType.Menu)
        {
            menuManager = FindObjectOfType<MenuManager>();
            menuManager.SetupMenu(MenuState.POKEMON);
        }
        else
        {
            AudioManager.instance.PlayExternal(WorldMusic.sound.name);
            World.Fade(true);
            playerMap.canMove = true;
        }
        Debug.Log("enter scene");

        debugging = false;
    }
    //On Scene UnLoaded

    void OnSceneChange(Scene newScene)
    {
        instance.enemyPokemons.RemoveAll(item => item == null);
        AudioManager.instance.PlayExternal(WorldMusic.sound.name);
        World.Fade(true);
        playerPokemons[0].Data().Heal();
        playerMap.canMove = true;
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


    public static void EncouterPokemon()
    {
        int rng = Random.Range(0, 256);
        if (rng < 127) World.DoBattleTransition(World.BattleTransition.WildWeak);
        else World.DoBattleTransition(World.BattleTransition.WildStrong);

        AudioManager.instance.Stop(Constants.MUSIC);
        AudioManager.instance.PlayExternal(instance.BattleMusic.sound.name);

        PokemonEncounter encounter = instance.currentArea.encounters.GetPokemon();
        PokemonInstance inst = instance.CreateNewPokemonInstance(encounter.pokemon, encounter.GetLevel());
        instance.enemyPokemons.Add(inst);

        instance.StartCoroutine(instance.LoadBattleScene());
    }
    public static void ExitEncouter()
    {
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
}

public enum SceneType
{
    World,
    Battle,
    Menu
}
