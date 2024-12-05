using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;

public class BattleUIActionOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [LabelText("Top")]
    [LabelWidth(30)]
    public BattleUIActionOption neighborTop;

    [LabelText("Left")]
    [LabelWidth(30)]
    [HorizontalGroup]
    public BattleUIActionOption neighborLeft;

    [LabelText("Right")]
    [LabelWidth(30)]
    [HorizontalGroup]
    public BattleUIActionOption neighborRight;

    [LabelText("Bottom")]
    [LabelWidth(30)]
    public BattleUIActionOption neighborBottom;

    [Space(10)]
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] Button button;

    
    private void Awake()
    {
        if(!button)
        button = GetComponent<Button>();
    }
    public virtual void OnSelect()
    {
        Debug.Log(gameObject.name);
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log(gameObject.name + " was touched in a funny way!");
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
       /// Debug.Log(gameObject.name + " is safe for now!");
    }
    public void Press()
    {
        button.onClick.Invoke();
        //OnSelect();
    }
    public void HighLight()
    {
        button.Select();
    }

    public bool IsSelected()
    {
        return EventSystem.current.currentSelectedGameObject == gameObject;
    }

    
}
