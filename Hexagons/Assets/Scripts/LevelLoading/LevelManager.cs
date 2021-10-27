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
        // TODO: Rework so that it's not just based on scene level
        SceneManager.LoadSceneAsync($"Level {level + 1}");
        _currentLevel = level;
    }

    public static void LoadNextLevel()
    {
        LoadLevel(++_currentLevel);
    }

    public static void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
