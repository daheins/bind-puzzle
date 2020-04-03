using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject levelParent;

    private Level _currentLevel;
    private int _currentLevelIndex = -1;
    private readonly List<Level> _levels = new List<Level>();

    public static LevelManager instance;
    
    private void Start()
    {
        instance = this;
        
        foreach (Transform levelObject in levelParent.transform)
        {
            Level level = levelObject.GetComponent<Level>();
            _levels.Add(level);
        }
        
        levelParent.SetActive(false);
        
        StartNextLevel();
    }

    public void StartNextLevel()
    {
        if (_currentLevelIndex == _levels.Count - 1)
        {
            return;
        }
        
        if (_currentLevelIndex != -1)
        {
            Debug.Log("destroying " + _currentLevel);
            Destroy(_currentLevel.gameObject);
        }

        _currentLevelIndex += 1;
        Level nextLevel = Instantiate(_levels[_currentLevelIndex]);
        _currentLevel = nextLevel;
        _currentLevel.StartLevel();
    }

    public void ResetLevel()
    {
        Destroy(_currentLevel.gameObject);
        Level nextLevel = Instantiate(_levels[_currentLevelIndex]);
        _currentLevel = nextLevel;
        _currentLevel.StartLevel();
        
    }
}
