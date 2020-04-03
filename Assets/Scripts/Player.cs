using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class Player : MonoBehaviour
{
    public Level level;
    
    private KeyCode _actionKey = KeyCode.Z;
    private bool _isBound;
    private readonly List<Bindable> _boundItems = new List<Bindable>();
    private Collider2D _collider;

    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }
    
    void Update()
    {
        Vector2Int playerInput = Vector2Int.zero;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            playerInput = Vector2Int.down;
        } else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            playerInput = Vector2Int.up;
        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            playerInput = Vector2Int.right;
        } else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            playerInput = Vector2Int.left;
        }

        if (!playerInput.Equals(Vector2Int.zero))
        {
            level.HandleMovement(playerInput);
        }

        if (Input.GetKeyDown(_actionKey))
        {
            level.HandleAction();
        }
    }

    public bool IsBound()
    {
        return _isBound;
    }

    public List<Bindable> BoundItems()
    {
        return _boundItems;
    }

    public void SetCollidersActive(bool areCollidersActive)
    {
        _collider.enabled = areCollidersActive;
        List<Bindable> boundItems = BoundItems();
        foreach (Bindable boundItem in boundItems)
        {
            boundItem.GetCollider().enabled = areCollidersActive;
        }
    }

    public void MovePlayer(Vector2 playerMovement)
    {
        transform.position = new Vector3(transform.position.x + playerMovement.x, transform.position.y + playerMovement.y, 0);

        foreach (Bindable boundItem in BoundItems())
        {
            boundItem.transform.position = new Vector3(boundItem.transform.position.x + playerMovement.x, boundItem.transform.position.y + playerMovement.y, 0);
        }
    }

    public void BindItems(List<Bindable> items)
    {
        foreach (Bindable item in items)
        {
            _boundItems.Add(item);
        }
        _isBound = true;
    }

    public void UnbindAllItems()
    {
        _boundItems.Clear();
        _isBound = false;
    }
}
