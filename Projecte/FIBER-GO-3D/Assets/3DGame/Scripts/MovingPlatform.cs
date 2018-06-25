using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    
    private int currentState;
    public float smooth;
    public float resetTime;
    public NavigationBaker navigationBaker;
    private float journeyLength;
    private float transformStartTime;


    public Plataforma plat;
    public Vector3 displacement;

    Vector3 newPosition;
    Vector3 iniPosition;
    Vector3 destPosition;
    Vector3 resize;
    // Use this for initialization
    void Start () {
        newPosition = iniPosition = plat.transform.position;
        destPosition = transform.position + displacement;
        currentState = 0;
        plat = GetComponent<Plataforma>();
    }
	
    public void Interacciona()
    {
        plat.topPlat = null;
        plat.bottomPlat = null;
        plat.leftPlat = null;
        plat.rightPlat = null;
        if (currentState == 0)
        {
            journeyLength = Vector3.Distance(transform.position, destPosition);
            transformStartTime = Time.time;

            newPosition = destPosition;
            currentState = 1;
        }
        else if (currentState == 2)
        {
            journeyLength = Vector3.Distance(transform.position, iniPosition);
            transformStartTime = Time.time;

            newPosition = iniPosition;
            currentState = 1;
        }
    }

	// Update is called once per frame
	void Update () {
        if (transform.position != newPosition)
        {
            float distanceCovered = (Time.time - transformStartTime) * 2.0F;
            float amountOfJourneyCompleted = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, newPosition, amountOfJourneyCompleted);
            if (plat.entity != null) {
                plat.entity.transform.position = Vector3.Lerp(plat.entity.transform.position, newPosition, amountOfJourneyCompleted);
            }
            if (plat.topPlat != null) plat.topPlat.calculaReachables();
            if (plat.bottomPlat != null) plat.bottomPlat.calculaReachables();
            if (plat.leftPlat != null) plat.leftPlat.calculaReachables();
            if (plat.rightPlat != null) plat.rightPlat.calculaReachables();
            plat.calculaReachables();
            if (plat.topPlat != null) plat.topPlat.calculaReachables();
            if (plat.bottomPlat != null) plat.bottomPlat.calculaReachables();
            if (plat.leftPlat != null) plat.leftPlat.calculaReachables();
            if (plat.rightPlat != null) plat.rightPlat.calculaReachables();
        }           

    }

    void LateUpdate()
    {
        if (transform.position == destPosition)
        {

            currentState = 2;
            navigationBaker.UpdateNavmesh();
            transform.position = destPosition;
            if (plat.topPlat != null) plat.topPlat.calculaReachables();
            if (plat.bottomPlat != null) plat.bottomPlat.calculaReachables();
            if (plat.leftPlat != null) plat.leftPlat.calculaReachables();
            if (plat.rightPlat != null) plat.rightPlat.calculaReachables();
            plat.calculaReachables();
            if (plat.topPlat != null) plat.topPlat.calculaReachables();
            if (plat.bottomPlat != null) plat.bottomPlat.calculaReachables();
            if (plat.leftPlat != null) plat.leftPlat.calculaReachables();
            if (plat.rightPlat != null) plat.rightPlat.calculaReachables();
        }
        else if (transform.position == iniPosition)
        {
            currentState = 0;
            navigationBaker.UpdateNavmesh();
            transform.position = iniPosition;
            if (plat.topPlat != null) plat.topPlat.calculaReachables();
            if (plat.bottomPlat != null) plat.bottomPlat.calculaReachables();
            if (plat.leftPlat != null) plat.leftPlat.calculaReachables();
            if (plat.rightPlat != null) plat.rightPlat.calculaReachables();
            plat.calculaReachables();
            if (plat.topPlat != null) plat.topPlat.calculaReachables();
            if (plat.bottomPlat != null) plat.bottomPlat.calculaReachables();
            if (plat.leftPlat != null) plat.leftPlat.calculaReachables();
            if (plat.rightPlat != null) plat.rightPlat.calculaReachables();
        }
        else
        {
            navigationBaker.UpdateNavmesh();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("COLISON ENTER MOVING PLATFORM");
        resize = col.transform.localScale;
        col.transform.parent = gameObject.transform;
    }

    void OnTriggerExit(Collider col)
    {
        Debug.Log("COLISON EXIT MOVING PLATFORM");
        //Vector3 local = transform.lossyScale;
        col.transform.parent = null;
        col.transform.localScale = resize;
    }
}
