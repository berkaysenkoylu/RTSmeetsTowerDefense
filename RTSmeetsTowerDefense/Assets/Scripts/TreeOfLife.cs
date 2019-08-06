using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeOfLife : MonoBehaviour
{
    public float maxHealthPoint = 200f;
    //public GameObject healthBar;
    public Slider healthBar;

    float healthPoint;

    void Start()
    {
        healthPoint = maxHealthPoint;

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

        UpdateHealthBar(healthPoint);
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
