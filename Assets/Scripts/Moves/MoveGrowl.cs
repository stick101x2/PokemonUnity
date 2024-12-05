using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrowl : MoveBase
{
    public override IEnumerator Act(BattleManager manager, Move move, BattleUnit offense, BattleUnit defense)
    {
        offense.Animate("growl");
        yield return new WaitForSeconds(1f);
        yield return base.Act(manager, move, offense, defense);
    }
}
