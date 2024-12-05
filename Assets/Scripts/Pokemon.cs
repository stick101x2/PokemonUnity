using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBase pBase;
    [SerializeField] string name;
    [SerializeField] int level;
    [SerializeField] int hP;
    [SerializeField] List<Move> moves;
    [SerializeField] public int gender; //0 == Male, 1 == Female,-1 == Genderless
    [SerializeField] public int personality;
   
    public Pokemon(PokemonBase _base, int pLevel)
    {
        pBase = _base;
        name = pBase.name;
        level = pLevel;
        hP = MaxHP;
        moves = new List<Move>();

        personality = Random.Range(0, int.MaxValue);
        byte lower8 = (byte)personality;

        gender = GetGender(lower8);

        foreach (var move in pBase.LearnableMoves)
        {
            if(move.Level <= level)
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


    public PokemonBase Base { get { return pBase; } }
    public int Level { get { return level; } }
    public int HP { get { return hP; } }
    public List<Move> Moves { get { return moves; } }
    public int MaxHP { get { return Mathf.FloorToInt((pBase.MaxHP) * level / 100 + 5 + 10); } }
    public int Attack { get { return Mathf.FloorToInt(Mathf.Floor(2 * pBase.Attack)* level / 100 + 5); } }
    public int Defense { get { return Mathf.FloorToInt(Mathf.Floor(2 * pBase.Defense) * level / 100 + 5); } }
    public int SpAttack { get { return Mathf.FloorToInt(Mathf.Floor(2 * pBase.SpAttack) * level / 100 + 5); } }
    public int SpDefense { get { return Mathf.FloorToInt(Mathf.Floor(2 * pBase.SpDefense) * level / 100 + 5); } }
    public int Speed { get { return Mathf.FloorToInt(Mathf.Floor(2 * pBase.Speed) * level / 100 + 5); } }
    public int Gender { get { return gender; } }
    public string Name { get { return name; } }
    public PokemonType Type1 { get { return Base.TYPE1; } }
    public PokemonType Type2 { get { return Base.TYPE2; } }

    public Damage TakeDamage(Move move, Pokemon attacker)
    {
        float type = TypeChart.GetEffectiveness(move.Base.Type, Type1) * TypeChart.GetEffectiveness(move.Base.Type, Type2);
        if(type <= 0)
        {
            Damage damageInfo = new Damage()
            {
                damageDealt = 0,
                isDead = false,
                criticalHit = false,
                effective = 0
            };

            return damageInfo;
        }

        float critical = 1;
        if (Random.value * 100f <= 6.25f)
            critical = 2;

       
        
        float stab = (move.Base.Type == attacker.Base.TYPE1 || move.Base.Type == attacker.Base.TYPE2) ? 1.5f : 1f;
        float mod = Random.Range(0.85f, 1f);
        float l = 2 * attacker.Level / 5f + 2;
        float a = l * attacker.Attack * move.Base.Power;
        float d = ((a / Defense/ 50) + 2)  * type  * stab * critical;
        int damage = Mathf.FloorToInt(d * mod);

        hP -= damage;

        Damage DamageInfo = new Damage()
        {
            damageDealt = damage,
            isDead = HP <= 0,
            criticalHit = critical > 1f,
            effective = type
        };

        return DamageInfo;
    }

    public Move GetRandomMove()
    {
        return moves[Random.Range(0, moves.Count)];
    }


}

public struct Damage
{
    public int damageDealt;
    public bool isDead;
    public bool criticalHit;
    public float effective; //-1 == NO_EFFECT, 0 == NORMAL
}


