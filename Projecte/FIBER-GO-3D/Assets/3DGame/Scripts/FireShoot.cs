using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShoot : MonoBehaviour {

    private float speed = 20;
    private Plataforma target;
    public Transform hit;
    private Vector3 dest;

    public float impactDist;

    // Use this for initialization
    void Start () {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().getTarget();
        if (target.entity.tag == "Snake")
        {
            impactDist = 0.25f;
            dest = target.entity.GetComponent<SnakeController>().shootingPosition.position;
        }
        else if (target.entity.tag == "Lizard")
        {
            impactDist = 0.25f;
            dest = target.entity.GetComponent<LizardController>().shootingPosition.position;
        } 
        else if (target.entity.tag == "Spider")
        {
            impactDist = 0.25f;
            dest = target.entity.transform.position + Vector3.up;
        }
        transform.position = GameObject.FindGameObjectWithTag("Hand").transform.position;
    }
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(transform.position, dest) < impactDist)
        {
            Transform hitAux = Instantiate(hit, transform.position, Quaternion.identity);
            hitAux.transform.rotation = Quaternion.LookRotation(hitAux.transform.position - GameObject.FindGameObjectWithTag("Hand").transform.position);
            Destroy(gameObject);
            switch (target.entity.tag)
            {
                case "Snake":
                    target.entity.GetComponent<SnakeController>().die();
                    break;
                case "Lizard":
                    target.entity.GetComponent<LizardController>().die();
                    break;
                case "Spider":
                    target.entity.GetComponent<SpiderController>().die();

                    break;
            }
        }
        transform.LookAt(dest);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
	}
}
