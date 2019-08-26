using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeOfLife : MonoBehaviour
{
    public float maxHealthPoint = 200f;
    //public GameObject healthBar;
    public Slider healthBar;
    public GameObject deathEffect;
    public AudioClip getHitSound;

    float healthPoint;
    AudioSource audioSource;

    public delegate void GameOver();
    public static event GameOver onGameOver;

    void Start()
    {
        healthPoint = maxHealthPoint;

        audioSource = GetComponent<AudioSource>();

        Zombie.DealDamage += GetDamaged;
    }

    private void OnDestroy()
    {
        Zombie.DealDamage -= GetDamaged;
    }

    void Update()
    {
        //healthBar.transform.LookAt(Camera.main.transform);
    }

    void GetDamaged(float amount)
    {
        healthPoint -= amount;

        healthPoint = Mathf.Clamp(healthPoint, 0f, maxHealthPoint);

        audioSource.PlayOneShot(getHitSound);

        UpdateHealthBar(healthPoint);

        if (healthPoint <= 0f)
        {
            onGameOver();

            // Death
            Instantiate(deathEffect, transform.position, Quaternion.identity);

            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    void UpdateHealthBar(float currentHealth)
    {
        float healthPercentage = currentHealth / maxHealthPoint;

        //float xScale = healthPercentage;

        //healthBar.transform.GetChild(1).transform.localScale = new Vector3(xScale, healthBar.transform.GetChild(1).transform.localScale.y, healthBar.transform.GetChild(1).transform.localScale.z);

        //healthBar.transform.GetChild(1).transform.localPosition = new Vector3(1f - xScale, 0f, 0f);

        healthBar.value = healthPercentage;
    }
}
