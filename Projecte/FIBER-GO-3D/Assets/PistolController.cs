using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolController : MonoBehaviour {

    public Plataforma platform;
    public PlayerController player;
    public GameController game;

	// Use this for initialization
	void Start () {
        //Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
        transform.position = platform.transform.position + Vector3.up * 2;
    }

    private void disable()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (platform.entity != null)
            if (platform.entity.tag == "Player")
            {
                //player.hasGun = true;
                player.powerUp();
                //game.calculaShooteables();
                Invoke("disable", 2);
            }

	}
}
