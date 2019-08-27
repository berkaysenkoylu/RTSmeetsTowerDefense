using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Animator mainMenuAnimController;
    public Animator fadeInOutAnimController;

    public void PlayGame()
    {
        StartCoroutine(SceneTransitionProcess("isGameStarted", "Main"));
    }

    public void OpenTutorial()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }

    IEnumerator SceneTransitionProcess(string triggerName, string sceneName)
    {
        mainMenuAnimController.SetTrigger(triggerName);

        fadeInOutAnimController.SetTrigger("fadeOut");

        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene(sceneName);
    }
}
