using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HexagonController))]
public class PlayerBrain : MonoBehaviour, IHexagonBrain
{
    public enum InputType { GridKeyboard }

    public InputType inputType;
    
    private HexagonController _hexagonController;
    private HexagonController _hexagonControllerGrid;

    private void Awake()
    {
        _hexagonController = GetComponent<HexagonController>();
        _hexagonController.priority = 100;

        _hexagonControllerGrid = GetComponent<HexagonController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    /*private void CheckGoal()
    {
        var goals = GameObject.FindGameObjectsWithTag("Goal");
        var attachedHexagons = _hexagonController.GetConnectedHexagonsBFS();

        int overlap = 0;

        if (goals.Length <= 0) return;

        foreach (GameObject goalGO in goals)
        {
            HexagonController goal = goalGO.GetComponent<HexagonController>();
            foreach (HexagonController attachedHexagon in attachedHexagons)
            {
                if (Vector2.Distance(goal.transform.position, attachedHexagon.transform.position) <
                    goal.centerOverlapRadius + attachedHexagon.centerOverlapRadius)
                {
                    overlap++;
                }
            }
        }

        if (overlap == goals.Length && overlap == attachedHexagons.Count)
        {
            Debug.Log("Win");
        }
    }*/

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
            _hexagonControllerGrid.TryMoveDir(0);
        if (Input.GetKeyDown(KeyCode.W))
            _hexagonControllerGrid.TryMoveDir(1);
        if (Input.GetKeyDown(KeyCode.Q))
            _hexagonControllerGrid.TryMoveDir(2);
        if (Input.GetKeyDown(KeyCode.A))
            _hexagonControllerGrid.TryMoveDir(3);
        if (Input.GetKeyDown(KeyCode.S))
            _hexagonControllerGrid.TryMoveDir(4);
        if (Input.GetKeyDown(KeyCode.D))
            _hexagonControllerGrid.TryMoveDir(5);
    }

    public void OnWasAcquired()
    {
        
    }
}
