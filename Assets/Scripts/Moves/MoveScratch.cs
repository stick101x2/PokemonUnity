using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScratch : MoveBase
{
    public override IEnumerator Act(BattleManager manager, Move move, BattleUnit offense, BattleUnit defense)
    {
        Debug.Log("scratched");
        yield return new WaitForSeconds(0.5f);
        yield return base.Act(manager, move, offense, defense);
    }
}
