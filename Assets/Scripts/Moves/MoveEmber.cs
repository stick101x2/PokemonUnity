using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEmber : MoveBase
{
    public override IEnumerator Act(BattleManager manager, Move move, BattleUnit offense, BattleUnit defense)
    {
        offense.Animate("ember");
        AudioManager.Play("ember", Constants.MISC1);

        yield return new WaitForSeconds(0.4f);
        AudioManager.Play("ember2", Constants.MISC1);

        yield return new WaitForSeconds(0.4f);
        yield return base.Act(manager, move, offense, defense);
    }
}
