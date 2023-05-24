using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float t = 1f;
    // Start is called before the first frame update
    void Awake()
    {
        Invoke("DestroyThisGameObject", t);
    }

    public void DestroyThisGameObject()
    {
        Destroy(gameObject);
    }
}
