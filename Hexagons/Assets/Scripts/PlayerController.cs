using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HexagonController))]
public class PlayerController : MonoBehaviour, IHexagonController
{
    private HexagonController _hexagonController;

    private void Awake()
    {
        _hexagonController = GetComponent<HexagonController>();
        _hexagonController.priority = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //Vector2 mousePos = (Vector2)Input.mousePosition - new Vector2(Screen.width, Screen.height) / 2;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _hexagonController.Move(_hexagonController.DirToDirSnap(mousePos - (Vector2)transform.position));
        }
    }

    public void OnWasAcquired()
    {
        
    }
}
