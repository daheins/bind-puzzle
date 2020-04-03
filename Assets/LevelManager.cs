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
            level.gameObject.SetActive(false);
            _levels.Add(level);
        }
        
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
            _currentLevel.gameObject.SetActive(false);
        }

        _currentLevelIndex += 1;
        Level nextLevel = _levels[_currentLevelIndex];
        nextLevel.gameObject.SetActive(true);
        _currentLevel = nextLevel;
        _currentLevel.StartLevel();
    }
}
