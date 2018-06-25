using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SnakeController : MonoBehaviour {

    public Plataforma startPlatform;
    public Plataforma attackPlatform;
    private GameObject aura;
    private Animator mAnimator;
    private NavMeshAgent agent;
    private Vector3 offset = new Vector3(0, 0.5f, 0);
    public bool shooteable = false;
    private bool atacking = false;
    public Transform shootingPosition;
    public int attackdir;
    private AudioSource audioSource;
    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        startPlatform.calculaReachables();
        transform.position = startPlatform.transform.position + offset;
        if (attackdir == 1) {
            attackPlatform = startPlatform.topPlat;
            transform.LookAt(startPlatform.transform.position + Vector3.left);
        }
        if (attackdir == 2)
        {
            attackPlatform = startPlatform.rightPlat;
            transform.LookAt(startPlatform.transform.position + Vector3.forward);
        }
        if (attackdir == 3)
        {
            attackPlatform = startPlatform.bottomPlat;
            transform.LookAt(startPlatform.transform.position + Vector3.right);
        }
        if (attackdir == 4)
        {
            attackPlatform = startPlatform.leftPlat;
            transform.LookAt(startPlatform.transform.position + Vector3.back);
        }
        //transform.LookAt(attackPlatform.transform);
        startPlatform.entity = this.gameObject;
        mAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        aura = transform.Find("auraEnemy").gameObject;
        aura.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (!atacking)
        {
            if (agent.remainingDistance == 0.0f)
            {
                if (attackdir == 1)
                {
                    attackPlatform = startPlatform.topPlat;
                }
                if (attackdir == 2)
                {
                    attackPlatform = startPlatform.rightPlat;
                }
                if (attackdir == 3)
                {
                    attackPlatform = startPlatform.bottomPlat;
                }
                if (attackdir == 4)
                {
                    attackPlatform = startPlatform.leftPlat;
                }
                transform.position = startPlatform.transform.position + transform.position.y * Vector3.up;
            }
            //transform.position = startPlatform.transform.position + offset;
            
            if (attackPlatform != null)
            {
                if (attackPlatform.entity != null)
                {
                    if (attackPlatform.entity.tag == "Player")
                    {
                        agent.SetDestination(attackPlatform.transform.position + offset);
                        mAnimator.SetBool("walk", true);
                        //Debug.Log("Kill");
                    }
                    if (agent.remainingDistance > 0.0f && agent.remainingDistance < 1.75f)
                    {
                        agent.isStopped = true;
                        mAnimator.SetTrigger("attack");
                        atacking = true;
                        Debug.Log("kill player");
                    }
                }
                else
                    mAnimator.SetBool("walk", false);
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
        mAnimator.SetBool("walk", false);
        attackPlatform.entity = null;
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
