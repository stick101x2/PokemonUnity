using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public bool Debug;
    public static GameManager instance;
    public SceneType currentScene;

    public SoundAsset WorldMusic;
    public SoundAsset BattleMusic;
    public const string BATTLE_SCENE = "Battle";
    public const string WORLD_SCENE = "World";

    public List<PokemonInstance> playerPokemons;
    public List<PokemonInstance> enemyPokemons;

    public int currentActivePokemon;

    public PokemonBase testEnemy;
    public int TestEnemyLVL;
    public PokemonBase testPlayer;
    public int TestPlayerLVL;

    Transform pokemonInstanceParent;
    bool ONAWAKE;
    bool ONENABLE;
    bool ONSTART;
    public BattleManager battleManager { get; set; }
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
    private void Start()
    {
        PokemonInstance p = CreateNewPokemonInstance(testPlayer, TestPlayerLVL);
        p.SetAllyStatus(true);
        playerPokemons.Add(p);
    }

    public PokemonInstance CreateNewPokemonInstance(PokemonBase pBase,int lvl)
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
        if(rng < 127) World.DoBattleTransition(World.BattleTransition.WildWeak);
        else World.DoBattleTransition(World.BattleTransition.WildStrong);
        AudioManager.instance.Stop(AudioManager.SOURCE_MUSIC);
        AudioManager.instance.PlayExternal(instance.BattleMusic.sound.name);
        instance.StartCoroutine(instance.LoadBattleScene());
    }
    public static void ExitEncouter()
    {
        instance.StartCoroutine(instance.LoadWorldScene());

    }
    //Called before OnStart and after OnEnable
    void OnSceneChange(Scene newScene, LoadSceneMode sceneLoadMode)
    {      
        if (currentScene == SceneType.Battle)
        {
            if(Debug)
            {
                AudioManager.instance.PlayExternal(BattleMusic.sound.name);
                PokemonInstance p = CreateNewPokemonInstance(testPlayer, TestPlayerLVL);
                p.SetAllyStatus(true);
                playerPokemons.Add(p);
            }

            pokemonInstanceParent = transform.Find("Pokemon");
            PokemonInstance e = CreateNewPokemonInstance(testEnemy, TestEnemyLVL);
           
            enemyPokemons.Add(e);

            battleManager = FindObjectOfType<BattleManager>();


            battleManager.SetupBattle();
        }
        else
        {

            AudioManager.instance.PlayExternal(WorldMusic.sound.name);
            World.Fade(true);
            
        }

        Debug = false;
    }
    void OnSceneChange(Scene newScene)
    {
        AudioManager.instance.PlayExternal(WorldMusic.sound.name);
        World.Fade(true);
    }
    public IEnumerator LoadWorldScene()
    {

        yield return new WaitForSeconds(1f);
        SceneManager.UnloadSceneAsync(BATTLE_SCENE);
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
    Battle
}
