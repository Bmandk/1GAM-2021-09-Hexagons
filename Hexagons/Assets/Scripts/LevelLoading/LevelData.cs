using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObject/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    // TODO: Rework so they don't use strings
    public int levels;
    public string mainMenu;
    public string levelSelect;
}