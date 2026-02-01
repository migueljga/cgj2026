using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvasController : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainCanvasGroup;
    [SerializeField] private CanvasGroup difficultyCanvasGroup;
    [SerializeField] private CanvasGroup creditsCanvasGroup;
    [SerializeField] private CanvasGroup infoCanvasGroup;
    [SerializeField] private CanvasGroup EnglishCanvasGroup; 
    [SerializeField] private CanvasGroup SpanishCanvasGroup;

    private int lang = 0; //0 Spa, 1 Eng

    public enum CanvasType
    {
        Main, 
        Difficulty,
        Credits,
        Info,
        English,
        Spanish
    }

    private void Start()
    {
        ShowHideCanvas(mainCanvasGroup, true);
        ShowHideCanvas(creditsCanvasGroup, false);
        ShowHideCanvas(infoCanvasGroup, false);
        ShowHideCanvas(difficultyCanvasGroup, false);
        ShowHideCanvas(EnglishCanvasGroup, false);
        ShowHideCanvas(SpanishCanvasGroup, true);
    }

    public void StartGame ()
    {
        //Difficulty selection
        ShowHideCanvas(difficultyCanvasGroup, true);
        //SceneManager.LoadScene(1);
    }

    public void ExitGame ()
    {
        Application.Quit(); 
    }

    public void PlayEasy()
    {
        GameSettings.selectedDifficulty = 0;
        SceneManager.LoadScene(1);
    }
    public void PlayHard()
    {
        GameSettings.selectedDifficulty = 1;
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
                ShowHideCanvas(difficultyCanvasGroup, false);
                break;
            case CanvasType.Info:
                ShowHideCanvas(mainCanvasGroup, false);
                ShowHideCanvas(creditsCanvasGroup, false);
                ShowHideCanvas(infoCanvasGroup, true);
                ShowHideCanvas(difficultyCanvasGroup, false);
                break;
            case CanvasType.Main:
            default:
                ShowHideCanvas(mainCanvasGroup, true);
                ShowHideCanvas(creditsCanvasGroup, false);
                ShowHideCanvas(infoCanvasGroup, false);
                ShowHideCanvas(difficultyCanvasGroup, false);
                break;
        }
    }

    public void SwitchLangInfo()
    {
        if (lang == 0)
        {
            ShowHideCanvas(EnglishCanvasGroup, true);
            ShowHideCanvas(SpanishCanvasGroup, false);
            lang = 1;
        }
        else
        {
            ShowHideCanvas(EnglishCanvasGroup, false);
            ShowHideCanvas(SpanishCanvasGroup, true);
            lang = 0;
        }
    }

    private void ShowHideCanvas(CanvasGroup canvas, bool switchToShow)
    {
        canvas.alpha = switchToShow ? 1 : 0;
        canvas.blocksRaycasts = switchToShow;
        canvas.interactable = switchToShow;
    }
}
