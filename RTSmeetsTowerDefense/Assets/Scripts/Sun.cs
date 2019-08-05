using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Sun : MonoBehaviour
{
    public float speed = 10f;

    int day = 0;
    float angle;
    bool canIncreaseDay = true;

    public delegate void dayTimeDeclaration(float dayTimePercentage);
    public static event dayTimeDeclaration whatTimeIsIt;

    public delegate void spawningCanHappen(bool canSpawn);
    public static event spawningCanHappen canEnemiesBeSpawned;

    public delegate void anotherDayGoesBy(int dayCount);
    public static event anotherDayGoesBy dayCountIncreased;

    void Update()
    {
        angle += Time.deltaTime * speed;

        Quaternion newQuaternion = Quaternion.Euler(angle % 360, 0f, 0f);

        transform.rotation = newQuaternion;
        
        whatTimeIsIt((angle % 360) / 360);

        if (angle % 360 >= 0f && angle % 360 <= 180f)
        {
            // Daytime
            canEnemiesBeSpawned(false);

            canIncreaseDay = true;
        } 
        else
        {
            // Nighttime
            canEnemiesBeSpawned(true);
        }
        
        if (angle % 360 >= 350f && angle % 360 <= 359f && canIncreaseDay)
        {
            day++;

            dayCountIncreased(day);

            canIncreaseDay = false;
        }
    }
}
