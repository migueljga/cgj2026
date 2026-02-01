using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvasController : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainCanvasGroup;
    [SerializeField] private CanvasGroup creditsCanvasGroup;
    [SerializeField] private CanvasGroup infoCanvasGroup;

    public enum CanvasType
    {
        Main, 
        Credits,
        Info
    }

    private void Start()
    {
        ShowHideCanvas(mainCanvasGroup, true);
        ShowHideCanvas(creditsCanvasGroup, false);
        ShowHideCanvas(infoCanvasGroup, false);
    }

    public void StartGame ()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowCanvas(string typeString)
    {
        CanvasType type = (CanvasType)Enum.Parse(typeof(CanvasType), typeString, true);

        switch (type)
        {
            case CanvasType.Credits:
                ShowHideCanvas(mainCanvasGroup, false);
                ShowHideCanvas(creditsCanvasGroup, true);
                ShowHideCanvas(infoCanvasGroup, false);
                break;
            case CanvasType.Info:
                ShowHideCanvas(mainCanvasGroup, false);
                ShowHideCanvas(creditsCanvasGroup, false);
                ShowHideCanvas(infoCanvasGroup, true);
                break;
            case CanvasType.Main:
            default:
                ShowHideCanvas(mainCanvasGroup, true);
                ShowHideCanvas(creditsCanvasGroup, false);
                ShowHideCanvas(infoCanvasGroup, false);
                break;
        }
    }

    private void ShowHideCanvas(CanvasGroup canvas, bool switchToShow)
    {
        canvas.alpha = switchToShow ? 1 : 0;
        canvas.blocksRaycasts = switchToShow;
        canvas.interactable = switchToShow;
    }
}
