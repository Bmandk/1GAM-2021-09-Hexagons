using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hexagon))]
public class HexagonSplitter : MonoBehaviour, IInteractable
{
    public void Interact(int mySide, HexagonController hexagon, int theirSide)
    {
        Debug.Log("Split!");
    }
}
