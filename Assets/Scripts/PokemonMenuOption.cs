using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PokemonMenuOption : BattleUIActionOption
{
    Pokemon poke;
    HPBar hpbar;
    [SerializeField] TextMeshProUGUI pokemonName;
    [SerializeField] TextMeshProUGUI lvl;
    [SerializeField] TextMeshProUGUI gender;
    [SerializeField] Image icon;
    Transform box;
    Sprite[] icons;
    Vector2[] positions = new Vector2[2];

    public void Setup(Pokemon pokemon, bool activate)
    {
        positions[0] = icon.rectTransform.localPosition;
        positions[1] = new Vector2(positions[0].x, positions[0].y-4f);

        box = transform.GetChild(1);
        SetActive(activate, false);
        if(activate)
        {
            hpbar = GetComponentInChildren<HPBar>();
            poke = pokemon;
            SetData(poke);
            return;
        }
        box.gameObject.SetActive(false);
    }
    public void SetData(Pokemon pokemonData)
    {
        hpbar.Setup(pokemonData.MaxHP, pokemonData.HP);
        string gender = "";
        if (pokemonData.Gender == 0)
            gender = Constants.MALE;
        else if (pokemonData.Gender == 1)
            gender = Constants.FEMALE;
        pokemonName.text = pokemonData.Name.ToUpper();
        this.gender.text = gender;
        lvl.text = "_" + pokemonData.Level;

        icons = pokemonData.Base.IconSprites;
        icon.sprite = icons[0];
        icon.material = new Material(icon.material);

        Material mat = icon.material;
        mat.SetTexture("_PaletteIn", pokemonData.Base.NormalPalette.palette);
        pokemonData.Base.NormalPalette.ApplyPaletteToMaterial(mat);

    }

    public void AnimateIcon(int index)
    {
        if(!GetActive()) return;
        icon.sprite = icons[index];
        if(IsSelected())
            icon.transform.localPosition = positions[index];
        else
            icon.transform.localPosition = positions[0];

    }
}
