using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public Animator animController;

    void Start()
    {
        animController.SetTrigger("fadeIn");
    }

    public void GoBackToMenu()
    {
        StartCoroutine("SceneTransition");
    }

    IEnumerator SceneTransition()
    {
        animController.SetTrigger("fadeOut");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("Menu");
    }
}
