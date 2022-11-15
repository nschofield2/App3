using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public BoxCollider2D m_BC;

    void Start()
    {
        m_BC = gameObject.AddComponent<BoxCollider2D>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(this.gameObject);
    }
}
