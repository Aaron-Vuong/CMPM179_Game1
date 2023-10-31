using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MenuManager
{
    public PlayerController playerController;
    public int hideTime = 5;
    [SerializeField]
    private GameObject _toolTipContainer;
    [SerializeField]
    private TMP_Text _tooltipText;
    [SerializeField]
    private TMP_Text _stateText;
    [SerializeField]
    private Slider slider;
    
    public void DisplayToolTip(string msg)
    {
        _tooltipText.text = msg;
        _toolTipContainer.SetActive(true);
        Invoke("HideToolTip", hideTime);
    }
    public void HideToolTip()
    {
        _toolTipContainer.SetActive(false);
    }
    public void UpdateStateText(string msg)
    {
        _stateText.text = $"State: {msg}";
    }
    public void Update()
    {
        UpdateWindowDisplay();
    }
    public void UpdateWindowDisplay()
    {
        if (playerController._windowStarted)
        {
            float elapsedWindow = (Time.time - playerController.windowStartTime) / playerController._inputWindow;
            Debug.Log(elapsedWindow);
            if (elapsedWindow > 1)
            {
                slider.value = 0;
            }
            slider.value = elapsedWindow;
        }
    }
}
