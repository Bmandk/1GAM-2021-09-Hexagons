using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject _buttonPrefab;
    
    private void OnEnable()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < LevelManager.LevelData.levels; i++)
        {
            GameObject button = Instantiate(_buttonPrefab, transform);
            button.GetComponent<LevelSelectButton>().SetLevel(i);
        }
    }
}
