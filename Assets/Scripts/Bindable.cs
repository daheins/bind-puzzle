using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Bindable : MonoBehaviour
{
    public SpriteRenderer spriteDefault;
    public SpriteRenderer spriteBound;
    
    private Collider2D _collider2D;
    private bool _isBound;
    
    void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        UpdateSprites();
    }

    public Collider2D GetCollider()
    {
        return _collider2D;
    }

    public void SetBound(bool isBound)
    {
        _isBound = isBound;
        UpdateSprites();
    }
    
    private void UpdateSprites()
    {
        spriteDefault.enabled = !_isBound;
        spriteBound.enabled = _isBound;
    }
}
