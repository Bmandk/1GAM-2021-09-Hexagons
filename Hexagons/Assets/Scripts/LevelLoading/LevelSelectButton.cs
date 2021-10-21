using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{
    public int levelNumber;
    
    public void LoadLevel()
    {
        LevelManager.LoadLevel(levelNumber);
    }
}
