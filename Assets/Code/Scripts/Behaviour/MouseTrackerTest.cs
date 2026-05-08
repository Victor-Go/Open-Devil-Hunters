using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTrackerTest : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        transform.position = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
