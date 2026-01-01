using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [ShowInInspector]public PokemonInstance pokemon { get; private set; }
    public BattleUnitInfo UI { get; set; }

    [SerializeField] SpriteHandler sprite;
    [SerializeField] SpriteRenderer status_sprite;
    [SerializeField] SpriteMask mask;

    public int SpeedScore;

    public Move moveToUse;
    public BattleUnit unitToTarget;
    Animator anim;
    public void Setup(BattleUnitInfo UI, PokemonInstance pokemon,Vector3 position)
    {
        gameObject.SetActive(true);

        if (!sprite) sprite = GetComponentInChildren<SpriteHandler>();
        anim = GetComponent<Animator>();

        transform.localPosition = position;

        this.UI = UI;
        this.pokemon = pokemon;

        PokemonBase pkmnData = pokemon.Data().Base;

        Palette palette = pokemon.IsAlly() ? pkmnData.ShinyPalette : pkmnData.NormalPalette;
        Sprite[] sprites = pokemon.IsAlly() ? pkmnData.BackSprites : pkmnData.FrontSprites;
        sprite.Setup(sprites, palette, pkmnData.Animation);
        sprite.ApplyPalette(pkmnData.NormalPalette);
    }

    public void ChangeAttackModifier(int amount)
    {
        pokemon.Data().Modifiers.ChangeAttackStage(amount);
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
    public void SetStatusSprite(Sprite statusSprite)
    {
        status_sprite.sprite = statusSprite;
    }
    public void Animate(string animation)
    {
        anim.Play(animation, 0, 0);
    }

    public void PlayPokemonAnimation()
    {
        sprite.PlayAnimation();
    }
    public int GetSpeed()
    {
        return pokemon.Data().Speed;
    }
    public Pokemon GetPokemon()
    {
        return pokemon.Data();
    }

    public PokemonBase GetBasePokemonData()
    {
        return pokemon.Data().Base;
    }
}
