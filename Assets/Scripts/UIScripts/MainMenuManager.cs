using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MenuManager
{
    public GameMenu mainMenu;
    public GameMenu gameMenu;
    public GameMenu optionsMenu;
    public GameMenu creditsMenu;
    public GameMenu gameOverMenu;
    public int gameDuration = 5;
    public float startTime;

    public UIManager uiManager;
    public void BackToMainMenu()
    {
        uiManager.GoToMenu(mainMenu);
    }
    public void StartGame()
    {
        startTime = Time.time;
        Invoke("GameOver", gameDuration);
        uiManager.GoToMenu(gameMenu);
    }
    public void Options()
    {
        uiManager.GoToMenu(optionsMenu);
    }
    public void Credits()
    {
        uiManager.GoToMenu(creditsMenu);
    }
    public void GameOver()
    {
        uiManager.GoToMenu(gameOverMenu);
    }
}
