using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    private static LevelData _levelData;

    public static LevelData LevelData
    {
        get
        {
            if (_levelData == null)
                _levelData = Resources.Load<LevelData>("LevelData");
            return _levelData;
        }
    }

    private static int _currentLevel;

    public static void LoadLevel(int level)
    {
        SceneManager.LoadSceneAsync(LevelData.levels[level].name);
        _currentLevel = level;
    }

    public static void LoadNextLevel()
    {
        LoadLevel(++_currentLevel);
    }
}
