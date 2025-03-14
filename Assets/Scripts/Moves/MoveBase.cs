using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move", menuName = "PokemonUE/Move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] PokemonType type;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;

    public string Name { get { return name; } }
    public string Description { get { return description; } }
    public int Power { get { return power; } }
    public int Accuracy { get { return accuracy; } }
    public int PP { get { return pp; } }
    public PokemonType Type { get { return type; } }

    

    public virtual IEnumerator Act(BattleManager manager,Move move, BattleUnit offense, BattleUnit defense)
    {
        if(move.Base.Power <= 0)
        {
            yield return new WaitForSeconds(0.25f);
            yield return manager.Messenger.TypeDialog("Nothing Happend!");
            yield return new WaitForSeconds(0.25f);
            yield break;
        }


        manager.damagedInfo = defense.DealDamage(move, offense.pokemon.Data());
        Damage info = manager.damagedInfo;

        defense.Animate("damaged");
        defense.UI.AnimDamage();

        PlayEffectiveSound(info);

        yield return new WaitForSeconds(0.5f);
        defense.UpdateHPBar(defense.pokemon.Data().HP);
        yield return defense.HPDrain();
        if (info.fainted)
        {
            AudioManager.Play(defense.pokemon.Data().Cry, Constants.POKE, 0.8f);
        }
        defense.Animate("wait");
        yield return ShowEffecitveMessage(manager, info);

    }
   
    protected IEnumerator ShowEffecitveMessage(BattleManager manager,Damage info)
    {
        if (info.effective <= 0)
        {
            yield return manager.Messenger.TypeDialog("The move has no effect!");
            yield return new WaitForSeconds(0.5f);
            yield break;
        }

        if (info.criticalHit)
        {
            yield return new WaitForSeconds(0.25f);
            yield return manager.Messenger.TypeDialog("Critcal hit!");
            yield return new WaitForSeconds(0.25f);
        }

        if (info.effective < 0.9f)
        {
            yield return new WaitForSeconds(0.25f);
            yield return manager.Messenger.TypeDialog("Its not very effective!");
            yield return new WaitForSeconds(0.25f);
        }
        else if (info.effective > 1.1f)
        {
            yield return new WaitForSeconds(0.25f);
            yield return manager.Messenger.TypeDialog("Its super effective!");
            yield return new WaitForSeconds(0.25f);
        }
    }
    protected void PlayEffectiveSound(Damage info)
    {
        if (info.effective <= 0)
            return;
        else if (info.effective < 0.9f)
            AudioManager.Play("not_effective", Constants.MISC1);
        else if (info.effective > 1.1f)
            AudioManager.Play("super_effective", Constants.MISC1);
        else
            AudioManager.Play("effective", Constants.MISC1);
    }
}
