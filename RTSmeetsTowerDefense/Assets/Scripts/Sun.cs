using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Sun : MonoBehaviour
{
    public float speed = 10f;

    float angle;

    public delegate void dayTimeDeclaration(float dayTimePercentage);
    public static event dayTimeDeclaration whatTimeIsIt;

    void Update()
    {
        angle += Time.deltaTime * speed;

        Quaternion newQuaternion = Quaternion.Euler(angle % 360, 0f, 0f);

        transform.rotation = newQuaternion;

        whatTimeIsIt((angle % 360) / 360);
    }
}
