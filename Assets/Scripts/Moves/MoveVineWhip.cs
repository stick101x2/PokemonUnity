using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVineWhip : MoveBase
{
    public override IEnumerator Act(BattleManager manager, Move move, BattleUnit offense, BattleUnit defense)
    {
        offense.Animate("tackle");
        yield return new WaitForSeconds(0.1f);
        AudioManager.Play("vine_whip", Constants.MISC1);
        defense.Animate("vine_damaged");
        yield return new WaitForSeconds(0.4f);
        yield return base.Act(manager, move, offense, defense);
    }
}
