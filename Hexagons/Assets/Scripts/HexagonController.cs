using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Hexagon))]
public class HexagonController : MonoBehaviour
{
    public Color color;
    public Color outlineColor;
    public float outlineWidth = 0f;
    public float outlineBlendOuter = 0f;

    // Player = 100
    public int priority;
    

    private HexagonController[] _attachedHexagons = new HexagonController[6];

    private SpriteRenderer _visuals;
    private Hexagon _hexagon;

    public Vector3Int GridPosition => _hexagon.gridPosition;
    
    private void Awake()
    {
        _hexagon = GetComponent<Hexagon>();
        _visuals = GetComponentInChildren<SpriteRenderer>();
        color = _visuals.material.GetColor("_Color");
        outlineColor = _visuals.material.GetColor("_OutlineColor");
    }

    private void OnEnable()
    {
        _attachedHexagons = new HexagonController[6];
    }

    public Vector3Int IntDirToCellDir(int direction)
    {
        Vector3Int directionVector = Vector3Int.zero; 
        switch (direction)
        {
            case 0:
                directionVector = new Vector3Int(GridPosition.y % 2 == 1 ? 1 : 0, 1, 0);
                break;
            case 1:
                directionVector = new Vector3Int(1, 0, 0);
                break;
            case 2:
                directionVector = new Vector3Int(GridPosition.y % 2 == 1 ? 1 : 0, -1, 0);
                break;
            case 3:
                directionVector = new Vector3Int(GridPosition.y % 2 == 0 ? -1 : 0, -1, 0);
                break;
            case 4:
                directionVector = new Vector3Int(-1, 0, 0);
                break;
            case 5:
                directionVector = new Vector3Int(GridPosition.y % 2 == 0 ? -1 : 0, 1, 0);
                break;
            default:
                Debug.LogError("Unknown Direction given");
                break;
        }

        return directionVector;
    }

    public bool TryMoveDir(int direction)
    {
        if (CanMove(direction))
        {
            List<HexagonController> hexagons = GetConnectedHexagonsBFS();
            foreach (HexagonController hexagon in hexagons)
            {
                hexagon.SetPosition(hexagon.GridPosition + hexagon.IntDirToCellDir(direction));
                hexagon.CheckAttachment();
            }

            return true;
        }
        return false;
    }

    public bool CanMove(int direction)
    {
        List<HexagonController> hexagons = GetConnectedHexagonsBFS();
        foreach (HexagonController hexagon in hexagons)
        {
            Vector3Int cell = hexagon.GridPosition + hexagon.IntDirToCellDir(direction);
            if (Hexagon.AllHexagons.ContainsKey(cell) && Hexagon.AllHexagons[cell].isBlocking && !hexagons.Select(x => x._hexagon).Contains(Hexagon.AllHexagons[cell]))
                return false;
        }
        
        return true;
    }

    public void SetPosition(Vector3Int cell)
    {
        Vector3 worldPos = Hexagon.Grid.CellToWorld(cell);
        transform.position = worldPos;

        Hexagon.AllHexagons.Remove(GridPosition);
        Hexagon.AllHexagons[cell] = _hexagon;
        _hexagon.gridPosition = cell;
    }

    private void CheckAttachment()
    {
        for (int i = 0; i < 6; i++)
        {
            // Side already has a hexagon
            if (_attachedHexagons[i] != null)
                continue;

            Vector3Int cell = GridPosition + IntDirToCellDir(i);
            
            // Cell is unoccupied
            if (!Hexagon.AllHexagons.ContainsKey(cell))
                continue;

            HexagonController hexagon = Hexagon.AllHexagons[cell].GetComponent<HexagonController>();
            if (hexagon == null)
                continue;
            
            Attach(i, hexagon, i + 3 > 5 ? i - 3 : i + 3); // Opposite side
        }
    }

    public void Attach(int mySide, HexagonController hexagon, int theirSide)
    {
        _attachedHexagons[mySide] = hexagon;
        hexagon.AttachHexagon(theirSide, this);
    }

    public void AttachHexagon(int side, HexagonController hexagon)
    {
        _attachedHexagons[side] = hexagon;
    }

    // Only call at runtime, otherwise it will leak materials into the scene
    private void UpdateVisuals()
    {
        _visuals.material.SetFloat("_OutlineWidth", outlineWidth);
        _visuals.material.SetFloat("_OutlineBlendOuter", outlineBlendOuter);
        _visuals.material.SetColor("_Color", color);
        _visuals.material.SetColor("_OutlineColor", outlineColor);
    }

    public List<Vector3Int> GetNeighbourCells()
    {
        List<Vector3Int> cells = new List<Vector3Int>();
        
        for (int i = 0; i < 6; i++)
        {
            cells.Add(IntDirToCellDir(i));
        }

        return cells;
    }

    public List<HexagonController> GetConnectedHexagonsBFS()
    {
        List<HexagonController> hexagons = new List<HexagonController>();
        UniqueQueue<HexagonController> queue = new UniqueQueue<HexagonController>();
        queue.Enqueue(this);
        
        while (queue.Count > 0)
        {
            var hexagon = queue.Dequeue();
            for (var i = 0; i < hexagon._attachedHexagons.Length; i++)
            {
                var attachedHexagon = hexagon._attachedHexagons[i];
                
                if (attachedHexagon == null ||
                    hexagons.Contains(attachedHexagon)) continue;
                
                queue.Enqueue(attachedHexagon);
            }
            hexagons.Add(hexagon);
        }

        return hexagons;
    }
}
