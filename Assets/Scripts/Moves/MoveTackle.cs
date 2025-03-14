using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTackle : MoveBase
{
    public override IEnumerator Act(BattleManager manager, Move move, BattleUnit offense, BattleUnit defense)
    {
        offense.Animate("tackle");
        yield return new WaitForSeconds(0.1f);
        AudioManager.Play("tackle", Constants.MISC1);
        defense.Animate("tackle_damaged");
        yield return new WaitForSeconds(0.4f);
        yield return base.Act(manager, move, offense, defense);
    }
}
