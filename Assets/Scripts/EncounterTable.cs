using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PokemonUE/EncouterTable")]
public class EncounterTable : ScriptableObject
{
    public PokemonEncounter[] encounters;

    public PokemonEncounter GetPokemon()
    {
        PokemonEncounter encounter = Utilities.GetWeightedProbability(encounters) as PokemonEncounter;
        return encounter;
    }
}

[System.Serializable]
public class PokemonEncounter : Probability
{
    public PokemonBase pokemon;
    public int[] levelRange = new int[1];

    public int GetLevel()
    {
        int ran = Random.Range(0, levelRange.Length);
        return levelRange[ran];
    }
}
