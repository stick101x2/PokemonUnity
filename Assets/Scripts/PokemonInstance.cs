using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonInstance : MonoBehaviour //Pokemon GameObject
{
    [SerializeField] Pokemon pokemon;
    [SerializeField] bool isPlayerPokemon = false;
    public bool setup { get; private set; }
    public void Setup(Pokemon _pokemon)
    {
        pokemon = _pokemon; 
        setup = true;
    }

    public bool IsAlly()
    {
        return isPlayerPokemon;
    }

    public Pokemon Data()
    {
        return pokemon;
    }

    public Sprite GetBattleSprite()
    {
        if (isPlayerPokemon)
            return pokemon.Base.BackSprite;
        else
            return pokemon.Base.FrontSprite;
    }

    public void SetAllyStatus(bool ally)
    {
        isPlayerPokemon = ally;
    }
}
