using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform wallParent;
    public string displayName;
    
    public GameObject wallPrefab;

    public Vector2Int boardDimensions = new Vector2Int(10, 10);

    private Player _player;
    private Goal _goal;

    private List<Vector2Int> _cardinalDirections = new List<Vector2Int>
        {Vector2Int.down, Vector2Int.up, Vector2Int.right, Vector2Int.left};
    
    private void Start()
    {
        _player = GetComponentInChildren<Player>();
        if (_player == null)
        {
            Debug.LogError("No player in this level!");
        }
        _player.level = this;
        _goal = GetComponentInChildren<Goal>();
        if (_goal == null)
        {
            Debug.LogError("No goal in this level!");
        }

        for (int x = 0; x <= boardDimensions.x + 1; x++)
        {
            Instantiate(wallPrefab, new Vector3(x, 0, 0), Quaternion.identity, wallParent);
            Instantiate(wallPrefab, new Vector3(x, boardDimensions.y + 1, 0), Quaternion.identity, wallParent);
        }
        
        for (int y = 1; y <= boardDimensions.y; y++)
        {
            Instantiate(wallPrefab, new Vector3(0, y, 0), Quaternion.identity, wallParent);
            Instantiate(wallPrefab, new Vector3(boardDimensions.x + 1, y, 0), Quaternion.identity, wallParent);
        }
    }

    public void StartLevel()
    {
        Camera mainCamera = Camera.main;
        mainCamera.transform.position =
            new Vector3((boardDimensions.x + 1) / 2f, (boardDimensions.y + 1) / 2f, mainCamera.transform.position.z);
        mainCamera.orthographicSize = Math.Max(boardDimensions.x, boardDimensions.y) / 2f + 2;
    }

    public void HandleMovement(Vector2Int playerInput)
    {
        if (!_cardinalDirections.Contains(playerInput))
        {
            Debug.LogWarning("Player is moving in a weird direction! " + playerInput);
        }
        
        Vector2 oldPoint = _player.transform.position;
        Vector2 inputPosition = oldPoint + playerInput;
        
        _player.SetCollidersActive(false);
        
        RaycastHit2D hit = Physics2D.Linecast(oldPoint, inputPosition, 1 << LayerMask.NameToLayer("Default"));
        
        bool canMovePlayer = hit.transform == null;
        if (canMovePlayer && _player.IsBound())
        {
            foreach (Bindable boundItem in _player.BoundItems())
            {
                Vector2 itemPoint = boundItem.transform.position;
                Vector2 itemVector = itemPoint + playerInput;
                RaycastHit2D itemHit = Physics2D.Linecast(itemPoint, itemVector, 1 << LayerMask.NameToLayer("Default"));
                if (itemHit.transform != null)
                {
                    canMovePlayer = false;
                    break;
                }
            }
        }
        _player.SetCollidersActive(true);
        
        if (canMovePlayer)
        {
            _player.MovePlayer(playerInput);
        }

        CheckForWin();
    }

    public void HandleAction()
    {
        if (_player.IsBound())
        {
            _player.UnbindAllItems();
        }
        else
        {
            List<Bindable> itemsToBind = CheckBindToPlayer();
            if (itemsToBind.Count > 0)
            {
                _player.BindItems(itemsToBind);
            }
        }
    }

    private void CheckForWin()
    {
        Vector2 distanceToGoal = _player.transform.position - _goal.transform.position;
        if (distanceToGoal.Equals(Vector2.zero))
        {
            WinLevel();
        }
    }

    private void WinLevel()
    {
        LevelManager.instance.StartNextLevel();
    }

    private List<Bindable> CheckBindToPlayer()
    {
        List<Bindable> itemsToBind = new List<Bindable>();
        
        _player.SetCollidersActive(false);
        // Check in each direction and add to the itemsToBind if item is bindable
        foreach (Vector2Int direction in _cardinalDirections)
        {
            Vector2 playerPosition = _player.transform.position;
            Vector2 vector = playerPosition + direction;
            RaycastHit2D hit = Physics2D.Linecast(playerPosition, vector, 1 << LayerMask.NameToLayer("Default"));

            if (hit.transform != null)
            {
                Bindable bindableItem = hit.collider.gameObject.GetComponent<Bindable>();
                if (bindableItem != null)
                {
                    itemsToBind.Add(bindableItem);
                }
            }
        }
        _player.SetCollidersActive(true);

        return itemsToBind;
    }
    
#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        for (int x = 0; x <= boardDimensions.x + 1; x++)
        {
            Gizmos.DrawCube(new Vector3(x, 0, 0), Vector3.one);
            Gizmos.DrawCube(new Vector3(x, boardDimensions.y + 1, 0), Vector3.one);
        }
        
        for (int y = 1; y <= boardDimensions.y; y++)
        {
            Gizmos.DrawCube(new Vector3(0, y, 0), Vector3.one);
            Gizmos.DrawCube(new Vector3(boardDimensions.x + 1, y, 0), Vector3.one);
        }
    }
#endif
}
