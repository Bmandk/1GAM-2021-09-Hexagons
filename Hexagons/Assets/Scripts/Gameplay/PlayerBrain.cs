using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(HexagonController))]
public class PlayerBrain : MonoBehaviour, IHexagonBrain
{
    public enum InputType { GridKeyboard }

    public InputType inputType;
    
    private HexagonController _hexagonController;
    private HexagonController _hexagonControllerGrid;

    private bool didWin;

    private void Awake()
    {
        _hexagonController = GetComponent<HexagonController>();
        _hexagonController.priority = 100;

        _hexagonControllerGrid = GetComponent<HexagonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (didWin)
            return;
        
        CheckInput();
    }

    // Checks whether the player is overlapping with the goal
    private void CheckGoal()
    {
        List<Hexagon> goals = GameObject.FindGameObjectsWithTag("Goal").Select(go => go.GetComponent<Hexagon>()).ToList();
        var attachedHexagons = _hexagonController.GetConnectedHexagonsBFS();

        int overlap = 0;

        if (goals.Count <= 0) return;

        foreach (Hexagon hexagon in goals)
        {
            foreach (HexagonController attachedHexagon in attachedHexagons)
            {
                if (attachedHexagon.GridPosition == hexagon.gridPosition)
                {
                    overlap++;
                }
            }
        }
        
        if (overlap == goals.Count && overlap == attachedHexagons.Count)
        {
            didWin = true;
            UIManager.Instance.ShowWinPanel();
        }
    }

    private void CheckInput()
    {
        switch (inputType)
        {
            case InputType.GridKeyboard:
                MoveGridKeyboard();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void MoveGridKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TryMove(0);
        if (Input.GetKeyDown(KeyCode.W))
            TryMove(1);
        if (Input.GetKeyDown(KeyCode.Q))
            TryMove(2);
        if (Input.GetKeyDown(KeyCode.A))
            TryMove(3);
        if (Input.GetKeyDown(KeyCode.S))
            TryMove(4);
        if (Input.GetKeyDown(KeyCode.D))
            TryMove(5);

        if (Input.GetKeyDown(KeyCode.R))
            LevelManager.ReloadCurrentLevel();
    }

    private void TryMove(int dir)
    {
        if (_hexagonControllerGrid.TryMoveDir(dir))
        {
            CheckGoal();
        }
    }

    public void OnWasAcquired()
    {
        
    }
}
