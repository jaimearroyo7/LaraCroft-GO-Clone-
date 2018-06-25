using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    // Use this for initialization
    public Transform target;
    public Vector3 offset;
    public float pitch = 2f;

    private float currentZoom = 16f;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 20f;
    void Start () {
		
	}

    private void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    // Update is called once per frame
    void LateUpdate () {
        transform.position = target.position - offset * currentZoom;
        transform.LookAt(target.position + Vector3.up*pitch);
	}
}
