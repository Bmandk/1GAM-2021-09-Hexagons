using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObject/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public List<SceneAsset> levels;
    public SceneAsset mainMenu;
    public SceneAsset levelSelect;
}