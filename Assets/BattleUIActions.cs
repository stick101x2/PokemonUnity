using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
public class BattleUIActions : Menu
{
    public List<BattleUIActionOption> actions;
    int selectedAction;
    BattleUIActionOption selected;
    bool moved;
    protected BattleUnit player;
    public virtual void Open(BattleUnit p)
    {
        selectedAction = 0;
        selected = actions[selectedAction];
        moved = false;
        gameObject.SetActive(true);
        player = p;
    }
    public virtual void Close()
    {
        selectedAction = 0;
        selected = actions[selectedAction];
        moved = false;
        gameObject.SetActive(false);
        player = null;
        
    }
    public void Update()
    {
        if (GetBattleState() == BattleState.PLAYER_ACTION)
            DoPlayerActionState();
    }
    public void Navigate(BattleUIActionOption next)
    {    
        if (next != null)
        {
            Vector2 dir = UserInput.GetDpad();
            BattleUIActionOption n = next;
            if (!n.gameObject.activeSelf)
            {
                if (dir.x > 0.5)
                {
                    if(n.neighborRight != null)
                        selectedAction = actions.IndexOf(n.neighborRight);
                }
                else if (dir.x < -0.5)
                {
                    if (n.neighborLeft != null)
                        selectedAction = actions.IndexOf(n.neighborLeft);
                }
                else if (dir.y > 0.5)
                {
                    if (n.neighborTop != null)
                        selectedAction = actions.IndexOf(n.neighborTop);
                }
                else if (dir.y < -0.5)
                {
                    if (n.neighborBottom != null)
                        selectedAction = actions.IndexOf(n.neighborBottom);
                }

                return;
            }
            if(n.gameObject.activeSelf)
                selectedAction = actions.IndexOf(n);
        }
    }
    void Move()
    {
        if (UserInput.GetDpad().magnitude < 0.1f)
            moved = false;

        if (moved)
            return;

        if(UserInput.GetDpad().magnitude >0.1f)
        {
            moved = true;

            if (UserInput.GetDpad().x > 0.5f)
            {
                Navigate(selected.neighborRight);
            }
            else if (UserInput.GetDpad().x < -0.5f)
            {
                Navigate(selected.neighborLeft);
            }
            else if (UserInput.GetDpad().y > 0.5f)
            {
                Navigate(selected.neighborTop);
            }
            else if (UserInput.GetDpad().y < -0.5f)
            {
                Navigate(selected.neighborBottom);
            }
            selected = actions[selectedAction];
            OnSwitch(selectedAction);

            for (int i = 0; i < actions.Count; i++)
            {
                if (i == selectedAction)
                {
                    actions[i].HighLight();
                }
            }
        }
    }
    protected virtual void OnSwitch(int index)
    {

    }
    void DoPlayerActionState()
    {
        Move();
        if(Keyboard.current.pKey.wasPressedThisFrame)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].IsSelected())
                {
                    actions[selectedAction].Press();
                }
            }
            
        }
        
    }
    BattleState GetBattleState()
    {
        return GameManager.instance.battleManager.GetState();
    }

}
