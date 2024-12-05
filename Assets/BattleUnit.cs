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
    public void Setup(BattleUnitInfo UI)
    {
        this.UI = UI;
        sprite.sprite = pokemon.GetBattleSprite();
    }

    public Damage DealDamage(Move move, Pokemon attacker)
    {
        Damage damageInfo = pokemon.Data().TakeDamage(move, attacker);
        int targetValue = pokemon.Data().HP;
        UI.HealthBar.ChangeHPBar(targetValue);
        return damageInfo;
        
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
