using System;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon : MonoBehaviour
{
    public Vector3Int gridPosition;
    
    public static Dictionary<Vector3Int, Hexagon> AllHexagons { get; private set; }
    public static Grid Grid { get; private set; }

    public bool isBlocking = false;

    private void Awake()
    {
        if (Grid == null)
            Grid = FindObjectOfType<Grid>();
        
        if (AllHexagons == null)
            AllHexagons = new Dictionary<Vector3Int, Hexagon>();
    }

    private void OnEnable()
    {
        gridPosition = Grid.WorldToCell(transform.position);
        AllHexagons[gridPosition] = this;
        transform.position = Grid.CellToWorld(gridPosition); // Ensures that hexagons are aligned to grid when starting
    }

    private void OnDisable()
    {
        AllHexagons.Remove(gridPosition);
    }
}