using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject winPanel;
    
    public static UIManager Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }

    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
    }
}
