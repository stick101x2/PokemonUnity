using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrowl : MoveBase
{
    public override IEnumerator Act(BattleManager manager, Move move, BattleUnit offense, BattleUnit defense)
    {
        offense.Animate("growl");
        AudioManager.Play(offense.pokemon.Data().Cry, Constants.POKE);
        yield return new WaitForSeconds(1f);
        AudioManager.Play("status_down", Constants.MISC1);
        defense.SetStatusSprite(manager.GetStatusEffect());
        defense.Animate("status_down");
        defense.ChangeAttackModifier(-1);
        yield return new WaitForSeconds(1.3f);

        yield return NothingHappened(manager, move);
    }
}
