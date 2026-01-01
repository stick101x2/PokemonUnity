using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class Menu : SerializedMonoBehaviour
{
    [TableMatrix(HorizontalTitle = "Menu Layout")]
    [SerializeField] string[,] LabledTable = new string[2, 2];

    [Space(5)]
    [SerializeField] protected List<RectTransform> items;
    [SerializeField] protected List<MenuOption> actions;
    [SerializeField] protected GameObject gameObjectToActivate;

    protected int selectedActionX;
    protected int selectedActionY;

    protected MenuOption selected;
    protected bool moved;

    public UiSound menuNavigationSound;
    public List<RectTransform> MenuItems { get { return items; } }
    public List<MenuOption> MenuActions { get { return actions; } }

    int tableLengthX;
    int tableLengthY;
    public virtual void Open()
    {
        SelectOptionInTable(selectedActionX, selectedActionY);

        moved = false;

        SetActiveState(true);
        selected.HighLight();
    }
    public virtual void Close()
    {
        SelectOptionInTable(selectedActionX, selectedActionY);

        moved = false;
        SetActiveState(false);
    }
   
    public virtual void Update()
    {
        if (BattleManager.instance != null && BattleManager.instance.GetMenu() == BattleMenu.POKE_SELECT)
            return;

        Move();
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i].IsSelected())
                {
                    actions[i].Press();
                }
            }
        }
    }
    public void ChangeLableInTable(string lableToFind,string nameToSetLable)
    {
        for (int row = 0; row < LabledTable.GetLength(0); row++)
        {
            for (int col = 0; col < LabledTable.GetLength(1); col++)
            {
                if (LabledTable[row,col] == lableToFind)
                {
                    LabledTable[row, col] = nameToSetLable;
                    return;
                }
            }
        }
    }
    public void ChangeLableInTable(int x,int y, string nameToSetLable)
    {
        LabledTable[x, y] = nameToSetLable;
    }
    void Move()
    {
        Vector2 dir = UserInput.GetDpad();

        dir.y *= -1;

        tableLengthX = LabledTable.GetLength(0);
        tableLengthY = LabledTable.GetLength(1);

        if (dir.magnitude < 0.1f)
            moved = false;

        if (moved)
            return;

        if (dir.magnitude > 0.1f)
        {                   
            moved = true;

            int intiSelectionX = selectedActionX;
            int intiSelectionY = selectedActionY;

            MenuOption menuOption = null;


            if (dir.x > 0.1f)
            {
                ChangeSelectionPosition(1, 0, false);
            }
            else if (dir.x < -0.1f)
            {
                ChangeSelectionPosition(-1, 0, false);

            }
            else if(dir.y > 0.1f)
            {
                ChangeSelectionPosition(0, 1, false);

            }
            else if(dir.y < -0.1f)
            {
                ChangeSelectionPosition(0, -1, false);

            }

            menuOption = GetOptionInTable();

            if (menuOption == null)
            {
                selectedActionX = intiSelectionX;
                selectedActionY = intiSelectionY;

                return;
            }

            if(menuNavigationSound) 
                menuNavigationSound.Play();
            selected = menuOption;
            selected.HighLight();
        }
    }

    void SetSelectionPosition(int x, int y)
    {
        selectedActionX = x;
        selectedActionY = y;
    }
    protected void SelectOptionInTable(int x, int y)
    {
        selectedActionX = x;
        selectedActionY = y;

        selected = transform.Find(LabledTable[selectedActionX, selectedActionY]).GetComponent<MenuOption>();
    }

    public void ChangeSelectionPosition(int xAmount, int yAmount, bool loop)
    {
        int intiSelectionX = selectedActionX;
        int intiSelectionY = selectedActionY;

        selectedActionY += yAmount;
        selectedActionX+= xAmount;

        if (loop)
        {
            if (selectedActionX >= tableLengthX)
                selectedActionX = 0;

            else if (selectedActionX < 0)
                selectedActionX = tableLengthY - 1;

            if (selectedActionY >= tableLengthY)
                selectedActionY = 0;

            else if (selectedActionY < 0)
                selectedActionY = tableLengthY - 1;
        }
        else
        {
            if (selectedActionX >= tableLengthX)
                selectedActionX = tableLengthX - 1;

            else if (selectedActionX < 0)
                selectedActionX = 0;

            if (selectedActionY >= tableLengthY)
                selectedActionY = tableLengthY - 1;

            else if (selectedActionY < 0)
                selectedActionY = 0;
        }

        string intable = LabledTable[selectedActionX, selectedActionY];

        int counter = 1024;
        bool arrow = intable == "←" || intable == "→" || intable == "↓" || intable == "↑";

        while(arrow)
        {
            counter--;
            if (counter < 0)
            {
                Debug.LogError("Infinte Loop Found");
                break;
            }

            if (intable == "←") selectedActionX -= 1;
            if (intable == "→") selectedActionX += 1;
            if (intable == "↓") selectedActionY += 1;
            if (intable == "↑") selectedActionY -= 1;

            if (selectedActionX >= tableLengthX || selectedActionX < 0)
                break;


            if (selectedActionY >= tableLengthY || selectedActionY < 0)
                break; 

            intable = LabledTable[selectedActionX, selectedActionY];

            if (intable == "←" || intable == "→" || intable == "↓" || intable == "↑")
                continue;
            else
            {
                break;
            }
            
        }

        

        if (intable == "SKIP")
        {
            selectedActionY += yAmount;
            selectedActionX += xAmount;
        }

        if (selectedActionX >= tableLengthX || selectedActionX < 0)
            selectedActionX = intiSelectionX;


        if (selectedActionY >= tableLengthY || selectedActionY < 0)
            selectedActionY = intiSelectionY;


    }
    protected void SetActiveState(bool active)
    {
        if (!gameObjectToActivate)
            gameObjectToActivate = gameObject;

        gameObjectToActivate.SetActive(active);
    }
    protected MenuOption GetOptionInTable() //Using Selected Action Position
    {
        string find = LabledTable[selectedActionX, selectedActionY];

        if(find == null || find == "")
        {
            return null;
        }

        MenuOption menuOption = transform.Find(find).GetComponent<MenuOption>();

        if (menuOption == null)
            return null;

        if (menuOption.GetActive()) return menuOption;
        else return null;
    }
    protected MenuOption GetOptionInTableAtPosition(int x, int y)
    {
        return transform.Find(LabledTable[x, y]).GetComponent<MenuOption>();
    }
    protected virtual void OnSwitch(int index)
    {

    }
}
