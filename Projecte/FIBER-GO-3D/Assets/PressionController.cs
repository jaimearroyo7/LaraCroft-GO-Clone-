using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PressionController : MonoBehaviour {

    // Use this for initialization
    public MovingPlatform[] movingPlatforms;
    Plataforma plat;
    bool ocupada;
    bool ocupadaPreviament;
    public GameObject previous;
    
	void Start () {
		plat = GetComponent<Plataforma>();
        ocupada = ocupadaPreviament = false;
        Renderer rend = GetComponent<Renderer>();
        rend.material.SetColor("_Color", Color.blue);
    }
	
	// Update is called once per frame
	void Update () {
		if(plat.entity == null && (ocupada || ocupadaPreviament))
        {
            if (ocupada && ocupadaPreviament)
            {
                ocupada = false;
            }
            else if (previous.GetComponent<NavMeshAgent>().remainingDistance <= previous.GetComponent<NavMeshAgent>().stoppingDistance)
            {
                ocupadaPreviament = false;
                foreach (MovingPlatform movingPlatform in movingPlatforms)
                    movingPlatform.Interacciona();
            }
        }
        else if (plat.entity != null && !ocupada)
        {
            previous = plat.entity.gameObject;
            ocupada = true;ocupadaPreviament = true;
            foreach (MovingPlatform movingPlatform in movingPlatforms)
                movingPlatform.Interacciona();
        }
    }
}
