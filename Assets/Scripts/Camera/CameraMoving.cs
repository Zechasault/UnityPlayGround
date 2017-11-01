using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour {
    [SerializeField] private float speed = 0.1f;
    public string horizontalAxis = "Horizontal", VerticalAxis = "Vertical";
    public GameObject target;
    private float _hAxisValue, _vAxisValue;

    private void Start()
    {
    }


    private void Update()
    {
        _hAxisValue = Input.GetAxis(horizontalAxis);
        _vAxisValue = Input.GetAxis(VerticalAxis);

        if (Input.GetMouseButtonDown(0))
        {
            Click0();
        }
            

        if (Camera.current != null)
        {
            
            if (target != null)
            {
                Camera.current.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, Camera.current.transform.position.z);
                if (_hAxisValue != 0f || _vAxisValue != 0f)
                {
                    target = null;
                }
            }
            else
            {
                Camera.current.transform.Translate(new Vector3(_hAxisValue*speed, _vAxisValue*speed, 0f));
            }
        }

       
    }

    private void Click0()
    {
        Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

        if (hit)
        {
            Debug.Log(hit.transform.name);
            target = hit.transform.gameObject;
        }
    }
}
