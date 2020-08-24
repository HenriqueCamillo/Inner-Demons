using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float time;

    private void Start()
    {
        Invoke(nameof(Goodbye), time);
    }

    private void Goodbye()
    {
        Destroy(this.gameObject);
    }
}
