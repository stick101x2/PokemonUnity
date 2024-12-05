using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public MoveBase Base { get; set; }
    public int PP { get; set; }
    public Move(MoveBase pbase)
    {
        Base = pbase;
        PP = pbase.PP;
    }
    //generic move action
    public virtual IEnumerator Act(BattleManager manager, BattleUnit offense, BattleUnit defense)
    {
        yield return Base.Act(manager,this,offense,defense);
    }
    //generic move functions
}
