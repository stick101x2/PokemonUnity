using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Pokemon  //Data
{
    [SerializeField] PokemonBase pBase;
    [SerializeField] string name; //NickName
    [SerializeField] int level;
    [SerializeField] int exp;
    [SerializeField] int hP;
    [SerializeField] List<Move> moves;
    [SerializeField] int gender; //0 == Male, 1 == Female,-1 == Genderless
    [SerializeField] int personality;
    [SerializeField] IndiviualValues IVs;
    [SerializeField] EffortValues EVs;
    [SerializeField] StatModifiers modifiers;
    [Space(10)]
    [SerializeField] byte lowWordLowByte;
    [SerializeField] byte lowWordHighByte;
    [SerializeField] byte highWordLowByte;
    [SerializeField] byte highWordHighByte;

    [ShowInInspector]public PokemonBase Base { get { return pBase; } }
    [ShowInInspector]public int Level { get { return level; } }
    [ShowInInspector]public int HP { get { return hP; } } 
    [ShowInInspector]public List<Move> Moves { get { return moves; } }
    [ShowInInspector]public int MaxHP { get { return Mathf.FloorToInt( ( 2* pBase.MaxHP+ IVs.HP + Mathf.Floor(EVs.hP/4) ) * level / 100) + level + 10; } }
    [ShowInInspector]public int Attack { get { return Mathf.FloorToInt( (Mathf.Floor( (2 * pBase.Attack + IVs.Attack + Mathf.Floor(EVs.attack / 4) ) * level / 100) +5) * 1/*Nature*/ ); } }
    [ShowInInspector]public int Defense { get { return Mathf.FloorToInt((Mathf.Floor((2 * pBase.Defense + IVs.Defense + Mathf.Floor(EVs.defense / 4)) * level / 100) + 5) * 1/*Nature*/ ); } }
    [ShowInInspector]public int SpAttack { get { return Mathf.FloorToInt((Mathf.Floor((2 * pBase.SpAttack + IVs.SpecialAttack + Mathf.Floor(EVs.specialAttack / 4)) * level / 100) + 5) * 1/*Nature*/ ); } }
    [ShowInInspector]public int SpDefense { get { return Mathf.FloorToInt((Mathf.Floor((2 * pBase.SpDefense + IVs.SpecialDefense + Mathf.Floor(EVs.specialDefense / 4)) * level / 100) + 5) * 1/*Nature*/ ); } }
    [ShowInInspector]public int Speed { get { return Mathf.FloorToInt((Mathf.Floor((2 * pBase.Speed + IVs.Speed + Mathf.Floor(EVs.speed / 4)) * level / 100) + 5) * 1/*Nature*/ ); } }
    [ShowInInspector]public StatModifiers Modifiers { get { return modifiers; } }

    [ShowInInspector]public int Gender { get { return gender; } }
    [ShowInInspector]public string Name { get { return GetName(); } }
    [ShowInInspector]public PokemonType Type1 { get { return Base.TYPE1; } }
    [ShowInInspector]public PokemonType Type2 { get { return Base.TYPE2; } }
    [ShowInInspector]public Sound Cry { get { return Base.Cry; } }

    //public int CompareTo(object obj)
    //{
    //    var a = this;
    //    var b = obj as Pokemon;

    //    if (a.Speed < b.Speed)
    //        return -1;

    //    if (a.Speed > b.Speed)
    //        return 1;

    //    return 0;
    //}

    public Pokemon(PokemonBase _base, int pLevel)
    {
        IVs = new IndiviualValues();
        EVs = new EffortValues();
        modifiers = new StatModifiers();

        pBase = _base;
        name = pBase.name;
        level = pLevel;
        hP = MaxHP;
        exp = LevelTable.GetBaseLevelExp(level, pBase.ExpGroup);
        moves = new List<Move>();



        personality = UnityEngine.Random.Range(0, int.MaxValue);

        lowWordLowByte = (byte)(personality & 0xFF);
        lowWordHighByte = (byte)((personality >> 8) & 0xFF);
        highWordLowByte = (byte)((personality >> 16) & 0xFF);
        highWordHighByte = (byte)((personality >> 24) & 0xFF);

        gender = GetGender(lowWordLowByte);

        foreach (var move in pBase.LearnableMoves)
        {
            if (move.Level <= level)
            {
                Moves.Add(new Move(move.Base));
            }

            if (Moves.Count >= 4)
                break;
        }
    }

    int GetGender(int random)
    {
        if (pBase.GenderThreshold == 255)
        {
            return 2;
        }

        if (pBase.GenderThreshold == 254 || pBase.GenderThreshold > random)
        {
            return 1;
        }

        return 0;
    }

    string GetName()
    {
        if(name.Equals(Base.Name))
        {
            return Base.Name.ToUpper();
        }
        return name;
    }

    public Damage TakeDamage(Move move, Pokemon attacker)
    {
        float type = TypeChart.GetEffectiveness(move.Base.Type, Type1) * TypeChart.GetEffectiveness(move.Base.Type, Type2);
        if(type <= 0)
        {
            Damage damageInfo = new Damage()
            {
                damageDealt = 0,
                fainted = false,
                criticalHit = false,
                effective = 0
            };

            return damageInfo;
        }

        float attackMod = attacker.modifiers.AttackModifier;
        float defenseMod = modifiers.DefenseModifier;

        float critical = 1;
        if (UnityEngine.Random.value * 100f <= 6.25f)
        {
            attackMod = 1;
            defenseMod = 1;

            critical = 2;
        }

       
        
        float stab = (move.Base.Type == attacker.Base.TYPE1 || move.Base.Type == attacker.Base.TYPE2) ? 1.5f : 1f;
        float mod = UnityEngine.Random.Range(0.85f, 1f);
        float l = 2 * attacker.Level / 5f + 2;
        float a = l * attacker.Attack * move.Base.Power * attackMod;
        float d = ((a / Defense/ 50) + 2) * defenseMod  * type  * stab * critical;
        int damage = Mathf.FloorToInt(d * mod);

        hP -= damage;

        Damage DamageInfo = new Damage()
        {
            damageDealt = damage,
            fainted = HP <= 0,
            criticalHit = critical > 1f,
            effective = type
        };

        return DamageInfo;
    }
    public void Heal()
    {
        hP = MaxHP;
    }
    public Move GetRandomMove()
    {
        return moves[UnityEngine.Random.Range(0, moves.Count)];
    }


}
[System.Serializable]
public class IndiviualValues
{
    [SerializeField] int hP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int speed;
    [SerializeField] int specialAttack;
    [SerializeField] int specialDefense;

    public int HP { get { return hP; } }
    public int Attack { get { return attack; } }
    public int Defense { get { return defense; } }
    public int Speed { get { return speed; } }
    public int SpecialAttack { get { return specialAttack; } }
    public int SpecialDefense { get { return specialDefense; } }

    public IndiviualValues()
    {
        hP = UnityEngine.Random.Range(0, 32);
        attack = UnityEngine.Random.Range(0, 32);
        defense = UnityEngine.Random.Range(0, 32);
        speed = UnityEngine.Random.Range(0, 32);
        specialAttack = UnityEngine.Random.Range(0, 32);
        specialDefense = UnityEngine.Random.Range(0, 32);
    }
}
[System.Serializable]
public class EffortValues
{
    public int hP;
    public int attack;
    public int defense;
    public int speed;
    public int specialAttack;
    public int specialDefense;

    public EffortValues()
    {
        hP = 0;
        attack = 0;
        defense = 0;
        speed = 0;
        specialAttack = 0;
        specialDefense = 0;
    }

   
}
[System.Serializable]
public class StatModifiers
{
    [SerializeField] int hP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int speed;
    [SerializeField] int specialAttack;
    [SerializeField] int specialDefense;

    public void ChangeHPStage(int amount) { StageChage(ref hP, amount); }
    public void ChangeAttackStage(int amount) { StageChage(ref attack, amount); }
    public void ChangeDefenseStage(int amount) { StageChage(ref defense, amount); }
    public void ChangeSpeedStage(int amount) { StageChage(ref speed, amount); }
    public void ChangeSpecialAttackStage(int amount) { StageChage(ref specialAttack, amount); }
    public void ChangeSpecialDefenseStage(int amount) { StageChage(ref specialDefense, amount); }

    void StageChage(ref int modifier, int amount)
    {
        int mod = modifier;
        mod += amount;

        if (mod > 6) mod = 6;
        if (mod < -6) mod = -6;

        modifier = mod;
    }

    public int CurrentHPStage { get { return hP; } }
    public int CurrentAttackStage { get { return attack; } }
    public int CurrentDefenseStage { get { return defense; } }
    public int CurrentSpeedStage { get { return speed; } }
    public int CurrentSpecialAttackStage { get { return specialAttack; } }
    public int CurrentSpecialDefenseStage { get { return specialDefense; } }

    [ShowInInspector]public float HPModifier { get { return GetStatModifcation(hP); } }
    [ShowInInspector]public float AttackModifier { get { return GetStatModifcation(attack); } }
    [ShowInInspector]public float DefenseModifier { get { return GetStatModifcation(defense); } }
    [ShowInInspector]public float SpeedModifier { get { return GetStatModifcation(speed); } }
    [ShowInInspector]public float SpecialAttackModifier { get { return GetStatModifcation(specialAttack); } }
    [ShowInInspector]public float SpecialDefenseModifier { get { return GetStatModifcation(specialDefense); } }

    float GetStatModifcation(int stage)
    {
        switch (stage)
        {
            case 6:
                return 8f / 2f;
            case 5:
                return 7f / 2f;
            case 4:
                return 6f / 2f;
            case 3:
                return 5f / 2f;
            case 2:
                return 4f / 2f;
            case 1:
                return 3f / 2f;

            case 0:
                return 2f / 2f;

            case -6:
                return 2f / 8f;
            case -5:
                return 2f / 7f;
            case -4:
                return 2f / 6f;
            case -3:
                return 2f / 5f;
            case -2:
                return 2f / 4f;
            case -1:
                return 2f / 3f;

            default:
                return 1;
        }
    }
    public StatModifiers()
    {
        hP = 0;
        attack = 0;
        defense = 0;
        speed = 0;
        specialAttack = 0;
        specialDefense = 0;
    }
}
[System.Serializable]
public struct Damage
{
    public int damageDealt;
    public bool fainted;
    public bool criticalHit;
    public float effective; //-1 == NO_EFFECT, 0 == NORMAL
}


