using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

    public string zoomAxis = "Mouse ScrollWheel";
    public float minSize = 10f, maxSize = 90f, sensitivity = 10f;

    private void Update()
    {
        float size = Camera.main.orthographicSize;
        size += -Input.GetAxis(zoomAxis) * sensitivity;
        size = Mathf.Clamp(size, minSize, maxSize);
        Camera.main.orthographicSize = size;
    }
}
