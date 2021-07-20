using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColliderOrienter : MonoBehaviour
{
    Quaternion noRotation;
    void Start()
    {
        noRotation = Quaternion.Euler(Vector3.zero);
    }
    void Update()
    {
        transform.rotation = noRotation;
    }
}
