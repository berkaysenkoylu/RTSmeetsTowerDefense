using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public EnemySpawnManager enemySpawnManager;
    public Sun sun;
    public GameObject player;
    public Transform zombiePool;
    public Animator gameOverScreenAnimController;

    public static GameManager instance
    {
        get
        {
            if (!gameManager)
            {
                gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (!gameManager)
                {
                    Debug.LogError("There needs to be one active GameManager script on a GameObject in your scene.");
                }
            }

            return gameManager;
        }
    }

    private void Start()
    {
        TreeOfLife.onGameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        TreeOfLife.onGameOver -= OnGameOver;
    }

    private void Update()
    {

    }

    void OnGameOver()
    {
        Debug.Log("Game Over");

        // Clean the scene
        KillAllEnemies();

        gameOverScreenAnimController.SetTrigger("gameOver");

        // Deactivate the necessary elements
        DeactivateElements();
    }

    void KillAllEnemies()
    {
        // First kill all the zombies
        for (int i = 0; i < zombiePool.childCount; i++)
        {
            if (!zombiePool.GetChild(i).gameObject.activeSelf)
                continue;
            else
                zombiePool.GetChild(i).GetComponent<Zombie>().InvokeDeathEvent();
        }
    }

    void DeactivateElements()
    {
        enemySpawnManager.enabled = false;
        sun.enabled = false;
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<PlayerMotor>().enabled = false;
    }

    public void RestartLevel()
    {
        RuntimeAnimatorController ac = gameOverScreenAnimController.runtimeAnimatorController;

        float duration = 2f;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == "GameOver_Restarted")
            {
                duration = ac.animationClips[i].length;
            }
        }

        StartCoroutine(Restart(duration));
    }

    IEnumerator Restart(float delay)
    {
        gameOverScreenAnimController.SetTrigger("isRestarted");

        yield return new WaitForSeconds(delay + 0.5f); // TODO: Tweak this?

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
