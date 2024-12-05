using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pokemon", menuName = "PokemonUE/Pokemon")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite front;
    [SerializeField] Sprite back;

    [SerializeField] PokemonType type1;
    [SerializeField] PokemonType type2;
    [Range(0,255)] 
    [SerializeField] int genderThreshold;
    // 0   = 100%   male; 0%     female;
    // 31  = 87.89% male; 12.11% female;
    // 63  = 75.39% male; 24.61% female;
    // 127 = 50.39% male; 49.61% female;
    // 191 = 25.39% male; 74.61% female;
    // 225 = 12.11% male; 87.89  female;
    // 254 = 0%     male; 100%   female; 
    // 255 = Unknown
    [Space(5)]
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;
    
   

    [SerializeField] List<LearnableMove> learnableMoves;

    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public PokemonType TYPE1 { get { return type1; } }
    public PokemonType TYPE2 { get { return type2; } }
    public int MaxHP { get { return maxHp; } }
    public int Attack { get { return attack; } }
    public int Defense { get { return defense; } }
    public int SpAttack { get { return spAttack; } }
    public int SpDefense { get { return spDefense; } }
    public int Speed { get { return speed; } }
    public int GenderThreshold { get { return genderThreshold; } }
    public List<LearnableMove> LearnableMoves { get { return learnableMoves; } }

    public Sprite FrontSprite { get { return front; } }
    public Sprite BackSprite { get { return back; } }
}
[Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base { get { return moveBase; } }
    public int Level { get { return level; } }
}
public enum PokemonType
{
    None,
    Normal,
    Fire,
    Water,
    Electric,
    Grass,
    Ice,
    Fighting,
    Poison,
    Ground,
    Flying,
    Psychic,
    Bug,
    Rock,
    Ghost,
    Dragon,
    Dark,
    Steel,
    Fairy
}

public class TypeChart
{
    const float SUPER = 2f; // SUPER_EFFECTIVE
    const float NORMA = 1f; // NORMAAL
    const float NOT_E = 0.5f; // NOT_EFFECTIVE
    const float NO_EF = 0f; // NO_EFFECT
    static float[][] chart =
    {
    //ATTACKER  DEFENDER ->   Normal Fire   Water  Electr Grass  Ice    Fight  Poison Ground Flying Psych  Bug    Rock   Ghost  Dragon Dark   Steel  Fairy
    /*Normal*/  new float[] { NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NOT_E, NOT_E, NORMA, NORMA, NOT_E, NORMA},
    /*Fire*/    new float[] { NORMA, NOT_E, NOT_E, NORMA, SUPER, SUPER, NORMA, NORMA, NORMA, NORMA, NORMA, SUPER, NOT_E, NORMA, NOT_E, NORMA, SUPER, NORMA },
    /*Water*/   new float[] { NORMA, SUPER, NOT_E, NORMA, NOT_E, NORMA, NORMA, NORMA, SUPER, NORMA, NORMA, NORMA, SUPER, NORMA, NOT_E, NORMA, NORMA, NORMA },
    /*Electric*/new float[] { NORMA, NORMA, SUPER, NOT_E, NOT_E, NORMA, NORMA, NORMA, NO_EF, SUPER, NORMA, NORMA, NORMA, NORMA, NOT_E, NORMA, NORMA, NORMA },
    /*Grass*/   new float[] { NORMA, NOT_E, SUPER, NORMA, NOT_E, NORMA, NORMA, NOT_E, SUPER, NOT_E, NORMA, NOT_E, SUPER, NORMA, NOT_E, NORMA, NOT_E, NORMA },
    /*Ice*/     new float[] { NORMA, NOT_E, NOT_E, NORMA, SUPER, NOT_E, NORMA, NORMA, SUPER, SUPER, NORMA, NORMA, NORMA, NORMA, SUPER, NORMA, NOT_E, NORMA },
    /*Fighting*/new float[] { SUPER, NORMA, NORMA, NORMA, NORMA, SUPER, NORMA, NOT_E, NORMA, NOT_E, NOT_E, NOT_E, SUPER, NO_EF, NORMA, SUPER, SUPER, NOT_E },
    /*Poison*/  new float[] { NORMA, NORMA, NORMA, NORMA, SUPER, NORMA, NORMA, NOT_E, NOT_E, NORMA, NORMA, NORMA, NOT_E, NOT_E, NORMA, NORMA, NO_EF, SUPER },
    /*Ground*/  new float[] { NORMA, SUPER, NORMA, SUPER, NOT_E, NORMA, NORMA, SUPER, NORMA, NO_EF, NORMA, NOT_E, SUPER, NORMA, NORMA, NORMA, SUPER, NORMA },
    /*Flying*/  new float[] { NORMA, NORMA, NORMA, NOT_E, SUPER, NORMA, SUPER, NORMA, NORMA, NORMA, NORMA, SUPER, NOT_E, NORMA, NORMA, NORMA, NOT_E, NORMA },
    /*Psychic*/ new float[] { NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, SUPER, SUPER, NORMA, NORMA, NOT_E, NORMA, NORMA, NORMA, NORMA, NO_EF, NOT_E, NORMA },
    /*Bug*/     new float[] { NORMA, NOT_E, NORMA, NORMA, SUPER, NORMA, NOT_E, NOT_E, NORMA, NOT_E, SUPER, NORMA, NORMA, NOT_E, NORMA, SUPER, NOT_E, NOT_E },
    /*Rock*/    new float[] { NORMA, SUPER, NORMA, NORMA, NORMA, SUPER, NOT_E, NORMA, NOT_E, SUPER, NORMA, SUPER, NORMA, NORMA, NORMA, NORMA, NOT_E, NORMA },
    /*Ghost*/   new float[] { NO_EF, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NOT_E, NORMA, NORMA, SUPER, NORMA, NOT_E, NORMA, NORMA },
    /*Dragon*/  new float[] { NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, SUPER, NORMA, NOT_E, NO_EF },
    /*Dark*/    new float[] { NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, NOT_E, NORMA, NORMA, NORMA, SUPER, NORMA, NORMA, SUPER, NORMA, NOT_E, NORMA, NOT_E },
    /*Steel*/   new float[] { NORMA, NOT_E, NOT_E, NOT_E, NORMA, SUPER, NORMA, NORMA, NORMA, NORMA, NORMA, SUPER, NOT_E, NORMA, NORMA, NORMA, NOT_E, SUPER },
    /*Fairy*/   new float[] { NORMA, NOT_E, NORMA, NORMA, NORMA, NORMA, SUPER, NOT_E, NORMA, NORMA, NORMA, NORMA, NORMA, NORMA, SUPER, SUPER, NOT_E, NORMA },
    };
    
    public static float GetEffectiveness(PokemonType attacker, PokemonType defender)
    {
        if (attacker == PokemonType.None || defender == PokemonType.None)
            return NORMA;

        int row = (int)attacker - 1;
        int col = (int)defender - 1;

        return chart[row][col];
    }
}
