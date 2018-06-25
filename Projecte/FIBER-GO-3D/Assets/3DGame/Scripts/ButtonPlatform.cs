using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPlatform : MonoBehaviour {

    public MovingPlatform[] movingPlatforms;
    public Plataforma platform;

	// Use this for initialization
	void Start () {
        Vector3 offset = new Vector3(-1.5f, 0.0f, -1.5f);
        transform.position = platform.transform.position + offset;
	}

    void OnMouseDown()
    {
        if (platform.entity != null)
            if (platform.entity.tag == "Player")
            {
                platform.entity.GetComponent<PlayerController>().pressButton(transform.position);
                
            }
    }

    public void pressed()
    {
        foreach (MovingPlatform movingPlatform in movingPlatforms)
            movingPlatform.Interacciona();
    }
}
