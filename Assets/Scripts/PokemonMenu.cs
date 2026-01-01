using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonMenu : Menu
{
    [Range(1,60)]
    public int animateEvery = 1;
    int frame;
    bool secondFrame = false;
    PokemonMenuOption[] menuOptions;
    public override void Open()
    {
        base.Open();
        frame = 0;
        menuOptions = GetComponentsInChildren<PokemonMenuOption>();

        for (int i = 0; i < menuOptions.Length; i++)
        {
            if(GameManager.instance.playerPokemons.Count < 7)
            {
                if (i < GameManager.instance.playerPokemons.Count)
                {
                    menuOptions[i].Setup(GameManager.instance.playerPokemons[i].Data(),true);
                }else
                {
                    ChangeLableInTable(menuOptions[i].gameObject.name, "↑");
                    menuOptions[i].gameObject.name = "↑";
                    menuOptions[i].Setup(null, false);

                }

            }
        }
    }
    public void FixedUpdate()
    {
        frame++;
        if(frame % animateEvery == 0)
        {
            secondFrame = !secondFrame;
        }

        for (int i = 0;i < menuOptions.Length;i++)
        {
            int index = secondFrame ? 1 : 0;
            menuOptions[i].AnimateIcon(index);
        }
    }
}
