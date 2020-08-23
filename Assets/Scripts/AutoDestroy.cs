using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    void Start()
    {
        this.transform.parent = null;
    }
    public void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}
