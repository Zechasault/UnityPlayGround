using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : MonoBehaviour {
    SpriteRenderer spriteRenderer;
    float feetPos;
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
        feetPos = transform.position.y - spriteRenderer.bounds.extents.y / 2;
        spriteRenderer.sortingOrder = -Mathf.RoundToInt(feetPos * 100f);
    }

    private void OnDrawGizmos()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        feetPos = transform.position.y - spriteRenderer.bounds.extents.y;
        Gizmos.DrawCube(new Vector3(transform.position.x, feetPos, transform.position.z), new Vector3(0.1f,0.1f,0.1f));
    }
}
