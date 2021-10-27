using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    public int levelNumber;

    private TMP_Text _text;
    
    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();
    }

    public void LoadLevel()
    {
        LevelManager.LoadLevel(levelNumber);
    }

    public void SetLevel(int levelNumber)
    {
        this.levelNumber = levelNumber;

        _text.text = (levelNumber + 1).ToString();
    }
}
