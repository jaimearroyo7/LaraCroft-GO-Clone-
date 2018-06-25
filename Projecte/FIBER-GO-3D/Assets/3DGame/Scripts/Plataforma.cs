using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma : MonoBehaviour {
    
    public Plataforma topPlat = null;
    public Plataforma leftPlat = null;
    public Plataforma rightPlat = null;
    public Plataforma bottomPlat = null;
    public ButtonPlatform buttonPlatform;

    //Transform transform;

    public bool breakable;
    private bool falling = false;
    int uses = 1;
    public GameObject entity;
    private Vector3 fallPosition;

    void Start()
    {
        calculaReachables();
        fallPosition = transform.position + Vector3.up * (-10);
    }

    void Update()
    {
        if (falling)
        {
            transform.position = Vector3.Lerp(transform.position, fallPosition, 5*Time.deltaTime);
        }
    }

    public void calculaReachables() {
        topPlat = null;
        leftPlat = null;
        rightPlat = null;
        bottomPlat = null;
        Plataforma[] allPlats = (Plataforma[])GameObject.FindObjectsOfType(typeof(Plataforma));
        for (int i = 0; i < allPlats.Length; ++i)
        {
            if (transform.position.x == allPlats[i].transform.position.x && transform.position.z == allPlats[i].transform.position.z + 5 && transform.position.y == allPlats[i].transform.position.y)
                leftPlat = allPlats[i];
            else if (transform.position.x == allPlats[i].transform.position.x && transform.position.z == allPlats[i].transform.position.z - 5 && transform.position.y == allPlats[i].transform.position.y)
                rightPlat = allPlats[i];
            if (transform.position.x == allPlats[i].transform.position.x + 5 && transform.position.z == allPlats[i].transform.position.z && transform.position.y == allPlats[i].transform.position.y)
                topPlat = allPlats[i];
            else if (transform.position.x == allPlats[i].transform.position.x - 5 && transform.position.z == allPlats[i].transform.position.z && transform.position.y == allPlats[i].transform.position.y)
                bottomPlat = allPlats[i];
        }
    }

    public void moved() {
        if (breakable) {
            Debug.Log("Brekeable");
            --uses;
            GetComponent<Animator>().SetTrigger("gonnabreak");
        }
        if (uses == -1)
        {
            //Destroy(entity);
            //transform.position += new Vector3(0.0f, 0.2f, 0.0f); ;

            Plataforma[] platforms = (Plataforma[])GameObject.FindObjectsOfType(typeof(Plataforma));
            for (int i = 0; i < platforms.Length; ++i)
            {
                platforms[i].calculaReachables();
            }
            GetComponent<Animator>().SetTrigger("break");
            //Destroy(this.gameObject);
        }
    }

    void OnMouseDown()
    {

    }

    public Plataforma getPlataformaAdj(int dir) {
        if (dir == 0)
            return topPlat;
        else if (dir == 1)
            return rightPlat;
        else if (dir == 2)
            return bottomPlat;
        else if (dir == 3)
            return leftPlat;
        else
            return null;
           
    }

    public void smookParticles()
    {
        //TODO: POSAR PARTICLES DE FUM MENTRE CAU
        Debug.Log("Particles de fum");
        falling = true;
        transform.position = transform.position + Vector3.up*(-100) ;
    }

    public void breack()
    {
        Destroy(entity);

        Plataforma[] platforms = (Plataforma[])GameObject.FindObjectsOfType(typeof(Plataforma));
        for (int i = 0; i < platforms.Length; ++i)
        {
            platforms[i].calculaReachables();
        }
        Destroy(this.gameObject);
    }

    public bool isReachable(Plataforma dest)
    {
        return dest == topPlat || dest == bottomPlat || dest == leftPlat || dest == rightPlat;
    }

    public void pressButton()
    {
        buttonPlatform.pressed();
    }
}
