using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMenu
{
    Main,
    GameHUD,
    Pause,
    Options,
    Credits,
    GameOver
}

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Menus;
    public GameMenu startingMenu;
    public GameMenu lastMenu;
    public GameMenu currentMenu;
    public KeyCode nextKey;
    public KeyCode[] menuKeys;

    // Start is called before the first frame update
    void Start()
    {
        //Menus = GameObject.FindGameObjectsWithTag( "Menu" );
        foreach (GameObject menu in Menus)
        {
            if (menu.GetComponent<MenuManager>() == null)
                Debug.LogError("No MenuManager found on " + menu.name);
            menu.GetComponent<MenuManager>().CloseMenu(); //make sure all menus start closed
        }
        OpenMenu(startingMenu); //open a starting menu if one is declared

    }

    // Update is called once per frame
    void Update()
    {
        CheckMenuInput();
    }

    public void CheckMenuInput()
    {
        // Quick debug solution for menu changing. Would recommend adding Input Manager buttons
        if (Input.GetKeyDown(nextKey))
        {
            if ((int)currentMenu == Menus.Length - 1)
            {
                GoToMenu((GameMenu)0);
                return;
            }
            GoToMenu((GameMenu)(int)currentMenu + 1);
            return;
        }
        for (int n = 0; n < menuKeys.Length; n++)
        {
            if (Input.GetKeyDown(menuKeys[n]))
            {
                GoToMenu((GameMenu)n);
                return;
            }
        }
    }

    private bool CloseMenu(GameMenu Menu)
    {
        Menus[(int)Menu].GetComponent<MenuManager>().CloseMenu();

        return true;
    }

    private bool OpenMenu(GameMenu Menu)
    {
        Menus[(int)Menu].GetComponent<MenuManager>().OpenMenu();

        return true;
    }

    public void GoToMenu(GameMenu Menu)
    {
        if (currentMenu == Menu)
        {
            Menu = lastMenu;
            Debug.LogWarning("Cannot move to " + Menu + ", currently on");
        }
        lastMenu = currentMenu;
        CloseMenu(currentMenu);
        currentMenu = Menu;
        OpenMenu(currentMenu);
    }
    public void GoToLast()
    {
        GoToMenu(lastMenu);
    }
}
