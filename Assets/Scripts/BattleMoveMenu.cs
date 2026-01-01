using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleMoveMenu : BattleUIActions
{
    [SerializeField] TextMeshProUGUI ppText;
    [SerializeField] TextMeshProUGUI typeText;

    public override void Open()
    {
        transform.parent.gameObject.SetActive(true);
        base.Open();
        InfoTextReset();
        SetMoves();
    }
    public override void Close()
    {
        transform.parent.gameObject.SetActive(false);
        base.Close();
        ResetMoves();
        InfoTextReset();
    }
    protected override void OnSwitch(int index)
    {
        if (index == 0)
        {
            InfoTextReset();
        }
        else
        {
            BattleMoveOption moveOption = actions[index] as BattleMoveOption;
            if (!moveOption.setup)
            {
                InfoTextReset();
                return;
            }

            SetInfo(moveOption);
        }
    }

    public void SetInfo(BattleMoveOption moveOption)
    {
        if(moveOption.GetMove() == null)
        {
            ppText.text = "-/-";
            typeText.text = "-";
            return;
        }
        ppText.text = $"{moveOption.GetMove().PP}/{moveOption.GetMove().Base.PP}";
        typeText.text = $"{moveOption.GetMove().Base.Type}";
    }
    public void SetInfo()
    {
        ppText.text = "-/-";
        typeText.text = "-";
    }
    void InfoTextReset()
    {
        ppText.text = "-/-";
        typeText.text = "-";
    }
    void ResetMoves()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i].gameObject.name == "BACK")
                continue;
            BattleMoveOption moveOption = actions[i] as BattleMoveOption;
            moveOption.ResetOption();
        }
    }
    void SetMoves()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i].gameObject.name == "BACK")
            {
                actions[i].SetActive(true, true);
                continue;
            }

            BattleMoveOption moveOption = actions[i] as BattleMoveOption;
            if (i >= player.pokemon.Data().Moves.Count)
            {
                ChangeLableInTable(actions[i].gameObject.name, "SKIP");
                actions[i].gameObject.name = "SKIP";
                actions[i].SetActive(false, true);
                continue;
            }
            actions[i].SetActive(true, true);
            moveOption.Setup(player.pokemon.Data().Moves[i], i);
        }
    }

    public void OnMoveSelected(int index)
    {
        GameManager.instance.battleManager.PerformPlayerMove(player.pokemon.Data().Moves[index]);
    }
}
