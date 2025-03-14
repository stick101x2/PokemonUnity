using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public PokemonInstance pokemon { get; set; }
    public BattleUnitInfo UI { get; set; }

    [SerializeField] SpriteRenderer sprite;

    Animator anim;
    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    public void Setup(BattleUnitInfo UI, PokemonInstance pokemon)
    {
        this.UI = UI;
        this.pokemon = pokemon;

        sprite.sprite = pokemon.GetBattleSprite();
        Material mat = sprite.material;
        mat.SetTexture("_PaletteIn", pokemon.Data().Base.NormalPalette.palette);
        pokemon.Data().Base.NormalPalette.ApplyPaletteToMaterial(mat);
    }

  

    public Damage DealDamage(Move move, Pokemon attacker)
    {
        Damage damageInfo = pokemon.Data().TakeDamage(move, attacker);
        int targetValue = pokemon.Data().HP;
        return damageInfo;
        
    }

    public void UpdateHPBar(int targetValue)
    {
        UI.HealthBar.ChangeHPBar(targetValue);

    }

    public IEnumerator HPDrain()
    {
        while (UI.HealthBar.enabled)
        {
            yield return null;
        }

    }

    public void Animate(string animation)
    {
        anim.Play(animation, 0, 0);
    }
}
