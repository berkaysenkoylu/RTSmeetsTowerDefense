using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public Animator animController;

    private void Start()
    {
        TreeOfLife.onGameOver += FadeOut;
    }

    private void OnDestroy()
    {
        TreeOfLife.onGameOver -= FadeOut;

    }

    void FadeIn()
    {
        animController.SetTrigger("fadeIn");
    }

    void FadeOut()
    {
        animController.SetTrigger("fadeOut");
    }
}
