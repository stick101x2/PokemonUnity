using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public MenuState state;

    public PokemonMenu pokemonMenu;

    public void SetupMenu(MenuState type)
    {
        state = type;

        switch (type)
        {
            case MenuState.NONE:
                break;
            case MenuState.POKEDEX:
                break;
            case MenuState.POKEMON:
                pokemonMenu.Open();
                break;
            case MenuState.BAG:
                break;
            case MenuState.SAVE:
                break;
            default:
                break;
        }
    }
}
public enum MenuState
{
    NONE,
    POKEDEX,
    POKEMON,
    BAG,
    SAVE
}