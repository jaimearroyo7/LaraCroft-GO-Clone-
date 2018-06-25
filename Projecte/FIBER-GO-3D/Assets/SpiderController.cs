using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderController : MonoBehaviour {

    // Use this for initialization
    public bool horizontal;
    public Plataforma currentPlat;
    private Vector3 offset = new Vector3(0, 0.5f, 0);

    private NavMeshAgent agent;
    private Animator mAnimator;
    private int dir;
    private bool atacking = false;
    public bool shooteable = false;
    private AudioSource audioSource;

    public Transform explosion;

    private GameObject aura;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentPlat.entity = gameObject;
        transform.position = currentPlat.transform.position+offset;
        mAnimator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        aura = transform.Find("auraEnemy").gameObject;
        dir = 0;
    }

    public void Move(){
        Debug.Log("Move spider");
        bool moved = false;
        Plataforma now = currentPlat;
        if (horizontal)
        {
            if (dir == 0)
            {
                if (currentPlat.rightPlat != null)
                {
                    moved = true;
                    currentPlat.entity = null;
                    agent.SetDestination(currentPlat.rightPlat.transform.position);
                    currentPlat = currentPlat.rightPlat;
                }
                else if (currentPlat.leftPlat != null)
                {
                    moved = true;
                    dir = 1;
                    currentPlat.entity = null;
                    agent.SetDestination(currentPlat.leftPlat.transform.position);
                    currentPlat = currentPlat.leftPlat;
                }
            }
            else {
                if (currentPlat.leftPlat != null)
                {
                    moved = true;
                    currentPlat.entity = null;
                    agent.SetDestination(currentPlat.leftPlat.transform.position);
                    currentPlat = currentPlat.leftPlat;
                }
                else if (currentPlat.rightPlat != null)
                {
                    moved = true;
                    dir = 0;
                    currentPlat.entity = null;
                    agent.SetDestination(currentPlat.rightPlat.transform.position);
                    currentPlat = currentPlat.rightPlat;
                }
            }
        }
        else {
            if (dir == 0)
            {
                if (currentPlat.topPlat != null)
                {
                    moved = true;
                    currentPlat.entity = null;
                    agent.SetDestination(currentPlat.topPlat.transform.position + offset);
                    currentPlat = currentPlat.topPlat;
                }
                else if (currentPlat.bottomPlat != null)
                {
                    dir = 1;
                    currentPlat.entity = null;
                    moved = true;
                    agent.SetDestination(currentPlat.bottomPlat.transform.position + offset);
                    currentPlat = currentPlat.bottomPlat;
                }
            }
            else
            {
                if (currentPlat.bottomPlat != null)
                {
                    moved = true;
                    currentPlat.entity = null;
                    agent.SetDestination(currentPlat.bottomPlat.transform.position + offset);
                    currentPlat = currentPlat.bottomPlat;
                }
                else if (currentPlat.topPlat != null)
                {
                    moved = true;
                    dir = 0;
                    currentPlat.entity = null;
                    agent.SetDestination(currentPlat.topPlat.transform.position + offset);
                    currentPlat = currentPlat.topPlat;
                }
            }
        }
        if (moved)
        {
            if (currentPlat.entity != null)
            {
                Debug.Log("Set Destination now");
                //agent.SetDestination(now.transform.position + offset);
                now.entity = this.gameObject;
                if (currentPlat.entity.tag == "Player")
                {
                    Debug.Log("Spider kills Player");
                }
            }
            else
            {
                
                currentPlat.entity = this.gameObject;
                currentPlat.moved();
            }
        } else
        {
            dir = (dir + 1) % 2;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.name);
    }

    // Update is called once per frame
    void Update () {
        //La sierra rota constantemente
        //transform.eulerAngles += new Vector3(10, 0, 0);
        if (!atacking)
        {
            bool walking = agent.remainingDistance > agent.stoppingDistance;
            mAnimator.SetBool("walking", walking);
            if (walking)
            {
                if (currentPlat.entity.tag == "Player" && agent.remainingDistance < 2.5f)
                {
                    agent.isStopped = true;
                    mAnimator.SetTrigger("atack");
                    mAnimator.SetBool("walking", false);
                    atacking = true;
                    Debug.Log("kill player");
                }
            }
        }


    }

    public void die()
    {
        mAnimator.SetTrigger("defeat");
        Instantiate(explosion, transform.position, Quaternion.identity);
        Disapear();
    }

    public void lookatPlayer()
    {
        transform.LookAt(FindObjectOfType<PlayerController>().transform);
    }

    public void Disapear()
    {
        Debug.Log("Spider defeated");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().finishAtack();
        Destroy(gameObject);

    }

    public void AttackPlayer()
    {
        Debug.Log("Atack finished, kill player!");
        currentPlat.entity.GetComponent<PlayerController>().kill(gameObject);
        currentPlat.entity = null;
        mAnimator.SetBool("walk", false);
    }

    public void startBrilla()
    {
        Debug.Log("Start brillar");
        aura.SetActive(true);
        shooteable = true;
    }

    public void stopBrilla()
    {
        aura.SetActive(false);
        shooteable = false;
    }

    public void playSound(int index)
    {
        if (index == 14)
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().playSound(index);
        else
        {
            audioSource.clip = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().getSound(index);
            audioSource.Play();
        }
    }
}
