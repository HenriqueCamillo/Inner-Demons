using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : MonoBehaviour
{
    [SerializeField] float reflectionForce;
    [SerializeField] Player player;
    [SerializeField] Collider2D rightCollider;
    [SerializeField] Collider2D leftCollider;

    void OnEnable() 
    {
        SetupColliders();
    }

    public void SetupColliders()
    {
        if (player.currrentArea == Power.Type.Body)
        {
            rightCollider.gameObject.SetActive(false);
            leftCollider.gameObject.SetActive(true);
        }
        else if (player.currrentArea == Power.Type.Mind)
        {
            rightCollider.gameObject.SetActive(true);
            leftCollider.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other.name);
        // if (other.CompareTag("Parryable Prop"))
        // {
            other.GetComponent<Prop>().Reflect(reflectionForce);
        // }
    }
}
