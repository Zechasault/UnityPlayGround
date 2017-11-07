using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{ //sent le cul de ouf ------------------------------//sent le cul de ouf ------------------------------//sent le cul de ouf ------------------------------//sent le cul de ouf ------------------------------
    //sent le cul de ouf ------------------------------//sent le cul de ouf ------------------------------//sent le cul de ouf ------------------------------//sent le cul de ouf ------------------------------
    Unit unit;
    public LayerMask unwalkableMask;
    // Use this for initialization
    void Start () {
        unit = GetComponent<Unit>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit;*/
        if (Input.GetButtonDown("RClick"))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePos - transform.position;
            /*hit = Physics2D.Raycast(transform.position, dir, Vector2.Distance(transform.position, mousePos),  unwalkableMask);
            if(hit.collider == null)
            {
                unit.followingPath = false;
                StartCoroutine(GoToMouse(mousePos, dir));
            }
            else
            {*/
                unit.GoTo(mousePos);
            /*}
            Debug.DrawRay(transform.position, dir, Color.green);*/
        }
    }

    IEnumerator GoToMouse(Vector3 mousePos, Vector2 dir)
    {
        while (transform.position.x != mousePos.x && transform.position.y != mousePos.y)
        {
            transform.Translate(dir.normalized * Time.deltaTime * unit.speed, Space.Self);
            yield return null;
        }
    }
}
