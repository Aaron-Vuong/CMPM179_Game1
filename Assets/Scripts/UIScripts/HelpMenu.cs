using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenu : MenuManager
{
    public bool isOpen = false;
    private void Start()
    {
        CloseMenu();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isOpen)
        {
            PauseGame();
            OpenMenu();
            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            UnpauseGame();
            CloseMenu();
            isOpen = false;
        }

    }
    private void PauseGame()
    {
        Time.timeScale = 0;
    }
    private void UnpauseGame()
    {
        Time.timeScale = 1;
    }
}
