using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bindable : MonoBehaviour
{
    private Collider2D _collider2D;
    
    void Start()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    public Collider2D GetCollider()
    {
        return _collider2D;
    }
}
