using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttachmentPoint
{
    public HexagonController attachedHexagon;
    public Vector2 position;
}

public class HexagonController : MonoBehaviour
{
    [SerializeField]
    private float radius = 1f;
    public float outlineWidth = 0f;
    public float rotation = 0f;

    public float movementSpeed = 1f;

    public float attachPointRadius = 0.1f;

    // Player = 100
    public int priority;

    public float BoundingRadius { get; private set; }

    public float Radius => radius;

    private static HashSet<HexagonController> _hexagons = new HashSet<HexagonController>();

    private HexagonController[] _attachedHexagons = new HexagonController[6];

    private int? ownerSide;

    private void Start()
    {
        _hexagons.Add(this);
        GenerateHexagon();
    }

    private void OnDestroy()
    {
        _hexagons.Remove(this);
    }

    public void MoveAttached(Vector2 ownerOrigin, Vector2 ownerAttachmentPos, float ownerRadius)
    {
        Vector2 pos = ownerOrigin + (ownerAttachmentPos - ownerOrigin).normalized * (ownerRadius + radius);
        Debug.Log(pos);
        transform.position = pos;
    }

    public void Move(Vector2 dir)
    {
        //float angle = Map(0f, 6f, 0f, 2f * Mathf.PI, dir);
        //Vector2 moveDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        transform.Translate(dir * (Time.deltaTime * movementSpeed));
        CheckAttachment();
    }

    private void CheckAttachment()
    {
        foreach (HexagonController hexagon in _hexagons)
        {
            if (hexagon == this) continue;
            
            if (priority < hexagon.priority ||
                Vector2.Distance(hexagon.transform.position, transform.position) > BoundingRadius + hexagon.BoundingRadius)
                continue;

            var points1 = GetAttachmentPoints();
            var points2 = hexagon.GetAttachmentPoints();

            for (int i = 0; i < _attachedHexagons.Length; i++)
            {
                if (_attachedHexagons[i] != null)
                    continue;
                Vector2 p1 = points1[i];
                
                int j = i + 3;
                if (j >= 6)
                {
                    j = j - 6;
                }
                Vector2 p2 = points2[j];

                if (Vector2.Distance(p1, p2) < attachPointRadius + hexagon.attachPointRadius)
                {
                    AttachTo(i, hexagon, j);
                    Debug.Log("Attached");
                }
            }
        }
    }

    public void AttachTo(int mySide, HexagonController hexagon, int theirSide)
    {
        _attachedHexagons[mySide] = hexagon;
        ownerSide = mySide;
        hexagon.Attach(theirSide, hexagon);
    }

    public void Attach(int side, HexagonController hexagon)
    {
        _attachedHexagons[side] = hexagon;
        var points = GetAttachmentPoints();
        hexagon.MoveAttached(transform.position, points[side], radius);
    }

    public Vector2[] GetAttachmentPoints()
    {
        Vector2[] points = new Vector2[_attachedHexagons.Length];
        
        float angle = 0;
        for (int i = 0; i < _attachedHexagons.Length; i++)
        {
            angle = i * (Mathf.PI / 3);
            points[i] = (Vector2)transform.position + AngleToDirSnap(angle) * Radius;
        }

        return points;
    }

    public void SetRadius(float radius)
    {
        if (radius <= 0)
            return;

        this.radius = radius;
        GenerateHexagon();
    }
    
    /// <summary>
    /// Converts a direction relative to the center of the hexagon to the direction that is the closest center of a side
    /// </summary>
    /// <param name="dir">Direction relative to the center of the hexagon</param>
    /// <returns>Direction locked to a hexagon side</returns>
    public Vector2 DirToDirSnap(Vector2 dir)
    {
        float angle = DirToAngleSideSnap(dir);
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    /// <summary>
    /// Converts a direction relative to the vertex of the hexagon
    /// </summary>
    /// <param name="dir">Direction relative to the center of the hexagon</param>
    /// <returns>The angle in radians</returns>
    public float DirToAngleSideSnap(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x);
        if (angle < 0)
        {
            angle += 2 * Mathf.PI;
        }

        angle = AngleToAngleSideSnap(angle);
        
        return angle;
    }

    public Vector2 AngleToDirSnap(float angle)
    {
        float a = AngleToAngleSideSnap(angle);
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }

    public float AngleToAngleSideSnap(float angle)
    {
        angle = Mathf.Floor(Map(0f, 2f * Mathf.PI, 0f, 6f, angle));
        return Map(0f, 6f, 0f, 2f * Mathf.PI, angle) + Mathf.PI / 6f;
    }

    public float Map(float from1, float from2, float to1, float to2, float value)
    {
        return Mathf.Lerp(to1, to2, Mathf.InverseLerp(from1, from2, value));
    }
    
    [ContextMenu("Generate Hexagon")]
    private void GenerateHexagon()
    {
        BoundingRadius = radius + attachPointRadius;
        ownerSide = null;
        _attachedHexagons = new HexagonController[6];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
        Gizmos.DrawWireSphere(transform.position, Radius / 0.866f);
        var points = GetAttachmentPoints();
        foreach (Vector2 point in points)
        {
            Gizmos.DrawWireSphere(point, attachPointRadius);
        }
        Gizmos.DrawWireSphere(transform.position, Radius + attachPointRadius);
    }
}
