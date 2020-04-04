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
        
        levelParent.SetActive(false);
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
            UIManager.instance.victoryText.gameObject.SetActive(true);
            return;
        }
        
        if (_currentLevelIndex != -1)
        {
            Destroy(_currentLevel.gameObject);
        }

        _currentLevelIndex += 1;
        Level nextLevel = Instantiate(_levels[_currentLevelIndex]);
        _currentLevel = nextLevel;
        _currentLevel.StartLevel();
        
        // display
        int levelIndex = _currentLevelIndex + 1;
        int totalLevels = _levels.Count;
        string levelName = _currentLevel.displayName;
        UIManager.instance.levelText.text = "Level " + levelIndex + "/" + totalLevels + ": " + levelName;
    }

    public void ResetLevel()
    {
        Destroy(_currentLevel.gameObject);
        Level nextLevel = Instantiate(_levels[_currentLevelIndex]);
        _currentLevel = nextLevel;
        _currentLevel.StartLevel();
    }
}
