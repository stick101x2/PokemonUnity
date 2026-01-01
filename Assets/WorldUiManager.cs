using UnityEngine;
using UnityEngine.InputSystem;


public class WorldUiManager : MonoBehaviour
{
    MainMenu menu;

    private void Awake()
    {
        menu = GetComponentInChildren<MainMenu>(true);
    }
    private void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            GameManager.inMenu = !GameManager.inMenu;
            if(GameManager.inMenu) menu.Open();
            else menu.Close();  
        }
    }
}
