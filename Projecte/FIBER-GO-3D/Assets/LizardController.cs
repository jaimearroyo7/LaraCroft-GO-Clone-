using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class LizardController : MonoBehaviour {

    // Use this for initialization
    public Plataforma startPlatform;
    public Plataforma attackPlatform;
    private Animator mAnimator;
    private AudioSource audioSource;

    NavMeshAgent agent;

    Queue<Plataforma> path;
    public bool shooteable = false;
    private GameObject aura;

    public Transform shootingPosition;

    Vector3 offset = new Vector3(0, 0.5f, 0);

    private bool atacking = false;
    bool waiting = true;
    int dir = -1;
    bool started = false;
    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (started)
        {
            if (attackPlatform == startPlatform.topPlat)
                dir = 0;
            else if (attackPlatform == startPlatform.rightPlat)
                dir = 1;
            else if (attackPlatform == startPlatform.bottomPlat)
                dir = 2;
            else if (attackPlatform == startPlatform.leftPlat)
                dir = 3;
            
        }
        else {
            path = new Queue<Plataforma>();
            startPlatform.entity = this.gameObject;
            agent = GetComponent<NavMeshAgent>();
            mAnimator = GetComponent<Animator>();
            transform.position = startPlatform.transform.position + offset;
            aura = transform.Find("auraEnemy").gameObject;
            aura.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!started) {
            started = !started;
            Start();       
        }

        if (!atacking)
        {
            bool walking = (agent.remainingDistance > agent.stoppingDistance);

            if (attackPlatform.entity != null)
            {
                //Debug.Log("atackPlatform entity != null");
                if (attackPlatform.entity.tag == "Player")
                {
                    //Debug.Log("atackPlatform entity.tag = player");
                    agent.SetDestination(attackPlatform.transform.position + offset);
                    //Debug.Log("Kill");
                    if (walking && agent.remainingDistance < 2.0f)
                    {
                        agent.isStopped = true;
                        mAnimator.SetTrigger("attack");
                        atacking = true;
                        Debug.Log("kill player");
                    }
                    else
                    {
                        mAnimator.SetBool("walk", walking);
                    }
                }
                else if (!walking)
                {
                    mAnimator.SetBool("walk", walking);
                }
            }
            else
            {
                mAnimator.SetBool("walk", walking);
            }

            if (startPlatform.entity != null)
            {
                if (startPlatform.entity.tag == "Player")
                    Debug.Log("Kill");
            }
            if (waiting && attackPlatform.getPlataformaAdj(dir).entity != null)
            {
                if (attackPlatform.getPlataformaAdj(dir).entity.tag == "Player")
                {
                    waiting = false;
                    Debug.Log("Al ataque");
                    path.Enqueue(attackPlatform);
                    path.Enqueue(attackPlatform.getPlataformaAdj(dir));
                }
            }
        }
    }


    public void Move(Plataforma newDest)
    {
        if (!waiting) {
            path.Enqueue(newDest);
            Plataforma destino = path.Dequeue();
            if (startPlatform.isReachable(destino))
            {
                if (destino.entity == null)
                {
                    attackPlatform = path.Peek();
                    mAnimator.SetBool("walk", true);
                    agent.SetDestination(destino.transform.position+offset);
                    startPlatform.entity = null;
                    startPlatform = destino;
                    startPlatform.entity = this.gameObject;
                    destino.moved();
                }
            }
            else
            {
                Debug.Log("no reachable");
                path = new Queue<Plataforma>();
                waiting = true;
            }
        }
    }

    public void kill()
    {
        Debug.Log("Snake killed");
    }

    public void die()
    {
        mAnimator.SetTrigger("dieBackwards");
    }

    public void lookatPlayer()
    {
        transform.LookAt(FindObjectOfType<PlayerController>().transform);
    }

    public void Disapear()
    {
        Debug.Log("Mutant defeated");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().finishAtack();
        Destroy(gameObject);

    }

    public void AttackPlayer()
    {
        attackPlatform.entity.GetComponent<PlayerController>().kill(gameObject);
        attackPlatform.entity = null;
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
