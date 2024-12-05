using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleMoveOption : BattleUIActionOption
{
    [SerializeField] Move move;
    BattleMoveMenu menu;
    public bool setup { get; private set; }
    public int index { get; private set; }
    public void Setup(Move sMove,int sindex)
    {
        move = sMove;
        text.text = move.Base.Name;
        gameObject.name = move.Base.Name;
        setup = true;
        index = sindex;
    }
    public void ResetOption()
    {
        move = null;
        setup = false;
        text.text = "-";
        gameObject.name = "-";
    }
    public Move GetMove()
    {
        return move;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!menu)
            menu = FindObjectOfType<BattleMoveMenu>();
        menu.SetInfo(this);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!menu)
            menu = FindObjectOfType<BattleMoveMenu>();
        
        menu.SetInfo();
    }
    public override void OnSelect()
    {
        if (!menu)
            menu = FindObjectOfType<BattleMoveMenu>();

        menu.OnMoveSelected(index);
    }
}
