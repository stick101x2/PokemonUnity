using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
public class BattleUIActions : Menu
{   
    public BattleUnit player;

    public override void Close()
    {
        base.Close();
        player = null;
    }

    public override void Update()
    {
        if (GetBattleState() == BattleState.PLAYER_ACTION)
            base.Update();
    }
    
    BattleState GetBattleState()
    {
        return GameManager.instance.battleManager.GetState();
    }

}
