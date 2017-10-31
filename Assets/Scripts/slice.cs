using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slice : MonoBehaviour {
    [SerializeField] private float cd;
    private float timeStamp = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (timeStamp <= Time.time && Input.GetButtonDown("Fire1"))
        {
            GetComponent<Animator>().SetTrigger("Slice");
            timeStamp = Time.time + cd;
        }
    }
}
