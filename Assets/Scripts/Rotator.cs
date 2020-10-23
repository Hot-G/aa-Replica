using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float RotateSpeed = 100f;

    void FixedUpdate()
    {
        transform.Rotate(0, 0, RotateSpeed * Time.fixedDeltaTime);
    }
}
