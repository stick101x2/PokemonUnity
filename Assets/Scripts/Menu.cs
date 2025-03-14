using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    public bool allowsSkips = true;
    [Space(5)]
    public List<RectTransform> items;
    public List<BattleUIActionOption> actions;

    protected int selectedAction;
    protected BattleUIActionOption selected;
    protected bool moved;
    public virtual void Open()
    {
        selectedAction = 0;
        selected = actions[selectedAction];
        moved = false;
        gameObject.SetActive(true);
        selected.HighLight();
    }
    public virtual void Close()
    {
        selectedAction = 0;
        selected = actions[selectedAction];
        moved = false;
        gameObject.SetActive(false);     
    }
    public virtual void Update()
    {
        Move();
        if (Keyboard.current.pKey.wasPressedThisFrame)
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
    public void NavigateSkip(BattleUIActionOption neighbor, Vector2 dir)
    {
        int selected = -1;
        // JoyX right
        if (dir.x > 0)
        {
            if (neighbor.neighborRight != null && neighbor.neighborRight.GetActive())
            {
                selectedAction = actions.IndexOf(neighbor.neighborRight);
                selected = selectedAction;
            }
            else if (neighbor.neighborBottom != null && neighbor.neighborBottom.GetActive())
            {
                selectedAction = actions.IndexOf(neighbor.neighborBottom);
                selected = selectedAction;

            }
            else if (neighbor.neighborTop != null && neighbor.neighborTop.GetActive())
            {
                selectedAction = actions.IndexOf(neighbor.neighborTop);
                selected = selectedAction;

            }
        }
        // JoyX left
        if (dir.x < 0)
        {
            if (neighbor.neighborLeft != null && neighbor.neighborLeft.GetActive())
            {
                selectedAction = actions.IndexOf(neighbor.neighborLeft);
                selected = selectedAction;
            }
            else if (neighbor.neighborTop != null && neighbor.neighborTop.GetActive())
            {
                selectedAction = actions.IndexOf(neighbor.neighborTop);
                selected = selectedAction;

            }
            else if (neighbor.neighborBottom != null && neighbor.neighborBottom.GetActive())
            {
                selectedAction = actions.IndexOf(neighbor.neighborBottom);
                selected = selectedAction;

            }
        }
        // JoyX Top
        if (dir.y > 0)
        {
            if (neighbor.neighborTop != null && neighbor.neighborTop.GetActive())
            {
                selectedAction = actions.IndexOf(neighbor.neighborTop);
                selected = selectedAction;
            }
            else if (neighbor.neighborLeft != null && neighbor.neighborLeft.GetActive())
            {
                selectedAction = actions.IndexOf(neighbor.neighborLeft);
                selected = selectedAction;

            }
            else if (neighbor.neighborRight != null && neighbor.neighborRight.GetActive())
            {
                selectedAction = actions.IndexOf(neighbor.neighborRight);
                selected = selectedAction;

            }
        }
        // JoyX Bottom
        if (dir.y < 0)
        {
            if (neighbor.neighborBottom != null && neighbor.neighborBottom.GetActive())
            {
                selectedAction = actions.IndexOf(neighbor.neighborBottom);
                selected = selectedAction;
            }
            else if (neighbor.neighborRight != null && neighbor.neighborRight.GetActive())
            {
                selectedAction = actions.IndexOf(neighbor.neighborRight);
                selected = selectedAction;

            }
            else if (neighbor.neighborLeft != null && neighbor.neighborLeft.GetActive())
            {
                selectedAction = actions.IndexOf(neighbor.neighborLeft);
                selected = selectedAction;
            }
        }

        if (selected == selectedAction)
            AudioManager.Play("select", Constants.UI2);
    }
    public void Navigate(BattleUIActionOption next)
    {
        if (next != null)
        {
            Vector2 dir = UserInput.GetDpad();
            BattleUIActionOption n = next;
            if (!n.GetActive() && allowsSkips)
            {
                NavigateSkip(n, dir);
                return;
            }
            if (n.GetActive())
            {
                selectedAction = actions.IndexOf(n);
                AudioManager.Play("select", Constants.UI2);

            }
        }
    }
    void Move()
    {
        if (UserInput.GetDpad().magnitude < 0.1f)
            moved = false;

        if (moved)
            return;

        if (UserInput.GetDpad().magnitude > 0.1f)
        {
            Debug.Log("x " + UserInput.GetDpad().x + "y " +  UserInput.GetDpad().y);
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
}
