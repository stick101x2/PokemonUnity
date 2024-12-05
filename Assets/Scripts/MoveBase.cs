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
        //play effective sound
        if(move.Base.Power <= 0)
        {
            yield return manager.Messenger.TypeDialog("Nothing Happend!");
            yield return new WaitForSeconds(0.5f);
            yield break;
        }

        defense.Animate("damaged");
        defense.UI.AnimDamage();
        yield return new WaitForSeconds(0.5f);
        manager.damagedInfo = defense.DealDamage(move, offense.pokemon.Data());
        Damage info = manager.damagedInfo;
        yield return defense.HPDrain();
        defense.Animate("wait");
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
}
