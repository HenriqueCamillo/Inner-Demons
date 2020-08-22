using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody2D rBody;
    [SerializeField] float speed;

    void Start()
    {
        rBody.velocity = this.transform.right * speed;
    }
}
