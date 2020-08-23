using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Tentacle"))
            Destroy(other.gameObject);
    }
}