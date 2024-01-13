using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }
}


