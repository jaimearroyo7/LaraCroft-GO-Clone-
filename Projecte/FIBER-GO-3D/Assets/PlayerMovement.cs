using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    // Use this for initialization
    public Rigidbody rb;
	void Start () {
        rb.AddForce(200*Time.deltaTime, 0, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rb.AddForce(200 * Time.deltaTime, 0, 0);
    }
}
