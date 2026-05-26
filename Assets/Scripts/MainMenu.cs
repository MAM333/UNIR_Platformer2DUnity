using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPage;
    public GameObject credits;
    public GameObject levels;

    void Start()
    {
        mainPage.SetActive(true);
        credits.SetActive(false);
        levels.SetActive(false);
    }

    public void Continue()
    {
        SceneManagement.instance.GoToNextLevel();
    }

    public void Credits()
    {
        credits.SetActive(true);
        mainPage.SetActive(false);
        levels.SetActive(false);
    }

    public void MainPage()
    {
        mainPage.SetActive(true);
        credits.SetActive(false);
        levels.SetActive(false);
    }

    public void Levels()
    {
        mainPage.SetActive(false);
        credits.SetActive(false);
        levels.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
