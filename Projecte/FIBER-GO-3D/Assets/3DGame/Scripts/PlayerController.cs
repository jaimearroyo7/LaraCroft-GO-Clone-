using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour {

    public AudioClip[] soundEfects;
    private AudioSource audioSource;
    
    public PauseMenu pauseState;

    public Plataforma startPlatform;
    public Transform auraPowerup;
    public Transform projectil;
    private Transform _projectil;

    public Plataforma currentPlatform;
    NavMeshAgent agent;
    private Animator playerAnimator;

    private bool walking = false;
    private bool alive = false;
    public bool atacking = false;
    private bool poweredUp = false;
    private bool pressingButton = false;
    private Plataforma target;

    public bool hasGun = false;

    Vector3 offset = new Vector3(0, 0.6f, 0);

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        playerAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentPlatform = startPlatform;
        transform.position = startPlatform.transform.position+offset;
        currentPlatform.entity = gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if (alive & !atacking)
        {
            walking = agent.remainingDistance > agent.stoppingDistance;
            //Debug.Log(walking);
            if (!atacking)
                playerAnimator.SetBool("walk", walking);

            if (agent.remainingDistance < 3.0f && agent.remainingDistance > 0)
            {
                //Debug.Log(agent.remainingDistance.ToString());
                if (currentPlatform.entity == null)
                {
                    Debug.Log("no entity");
                    currentPlatform.entity = this.gameObject;
                    agent.isStopped = false;
                    atacking = false;
                }
                else if (currentPlatform.entity.tag == "Snake")
                {
                    Debug.Log("ATACK TO SNAKE");
                    if (!atacking)
                    {
                        playerAnimator.SetTrigger("atack");
                        playerAnimator.SetBool("atacking", true);
                    }
                    atacking = true;
                    currentPlatform.entity.GetComponent<SnakeController>().lookatPlayer();
                    agent.isStopped = true;
                    playerAnimator.SetBool("walk", false);
                }
                else if (currentPlatform.entity.tag == "Lizard")
                {
                    Debug.Log("ATACK TO LIZARD");
                    atacking = true;
                    playerAnimator.SetTrigger("atack");
                    currentPlatform.entity.GetComponent<LizardController>().lookatPlayer();
                    agent.isStopped = true;
                    playerAnimator.SetBool("walk", false);
                }
                else if (currentPlatform.entity.tag == "Spider")
                {
                    Debug.Log("ATACK TO LIZARD");
                    atacking = true;
                    playerAnimator.SetTrigger("atack");
                    currentPlatform.entity.GetComponent<SpiderController>().lookatPlayer();
                    agent.isStopped = true;
                    playerAnimator.SetBool("walk", false);
                } else if (agent.remainingDistance < 1.5f && agent.remainingDistance > 0 && pressingButton)
                {
                    agent.isStopped = true;
                    playerAnimator.SetTrigger("pressButton");
                    playerAnimator.SetBool("walk", false);
                    Debug.Log("set trigger i bool");
                    atacking = true;
                    pressingButton = false;
                }
            }
        }
        if (currentPlatform.tag != "Finish" && !pauseState.GameIsPaused)
        {
            playerAnimator.SetBool("atacking", atacking);
            Plataforma aux = null;
            if (agent.remainingDistance < 0.1f && agent.remainingDistance > 0)
                playerAnimator.SetBool("poweringUp", poweredUp);
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("Up arrow");
                aux = currentPlatform.getPlataformaAdj(0);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("Right arrow");
                aux = currentPlatform.getPlataformaAdj(1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("Left arrow");
                aux = currentPlatform.getPlataformaAdj(3);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("Down arrow");
                aux = currentPlatform.getPlataformaAdj(2);
            }
            if (aux != null && !poweredUp)
                FindObjectOfType<GameController>().interaccioPlataforma(aux);
            
        }
        else if (agent.remainingDistance < 0.1f && agent.remainingDistance > 0)
        {
            playerAnimator.SetTrigger("victory");
            alive = false;
        }
    }

    public bool MoveToPoint(Plataforma dest)
    {
        if (!walking & alive & !atacking)
        {
            if (currentPlatform.GetComponent<Plataforma>().isReachable(dest))
            {
                currentPlatform.entity = null;
                currentPlatform = dest;
                if (dest.entity == null)
                    dest.entity = this.gameObject;
                dest.moved();
                Debug.Log("destination setted");
                agent.SetDestination(dest.transform.position + offset);
                return true;
            }
        }
        Debug.Log("return false");
        return false;
    }

    public void atack()
    {
        if (currentPlatform.entity.tag == "Snake")
        {
            currentPlatform.entity.GetComponent<SnakeController>().die();
        }
        else if (currentPlatform.entity.tag == "Lizard")
        {
            currentPlatform.entity.GetComponent<LizardController>().die();
        }
        else if (currentPlatform.entity.tag == "Spider")
        {
            currentPlatform.entity.GetComponent<SpiderController>().die();
        }

    }

    public void shoot(Plataforma plataforma) {
        if (!atacking)
        {
            target = plataforma;
            playerAnimator.SetTrigger("Shoot");
            atacking = true;
            hasGun = false;
            transform.LookAt(plataforma.transform);
        }
    }

    public void shootNow()
    {
        _projectil = Instantiate(projectil, GameObject.FindGameObjectWithTag("Hand").transform.position, Quaternion.identity);
    }

    public Plataforma getTarget()
    {
        return target;
    }

    public void kill(GameObject atacker)
    {
        transform.LookAt(atacker.transform);
        Debug.Log("Player killed");
        playerAnimator.SetTrigger("die");
        agent.isStopped = true;
        alive = false;
        //Destroy(gameObject);
    }

    public void Die()
    {
        Destroy(gameObject);
        FindObjectOfType<PauseMenu>().Restart();
    }

    public void Win()
    {
        FindObjectOfType<PauseMenu>().NextLevel();
    }

    public void Alive()
    {
        alive = true;
    }

    public void finishAtack()
    {
        atacking = false;
    }

    public void powerUp()
    {
        poweredUp = true;
        //playerAnimator.SetBool("poweringUp", true);
    }

    public void PoweredUp()
    {
        playerAnimator.SetBool("poweringUp", false);
        poweredUp = false;
        hasGun = true;
        atacking = false;
        FindObjectOfType<GameController>().calculaShooteables();
    }

    public void setPowerUpAura()
    {
        atacking = true;
        Instantiate(auraPowerup, transform.position, Quaternion.identity);
        playSound(1);
    }

    public void pressButton(Vector3 pos)
    {
        agent.SetDestination(pos+offset);
        transform.LookAt(pos);
        Debug.Log(agent.remainingDistance);
        pressingButton = true;
    }

    public void buttonPress()
    {
        //TODO: start pressing button
        currentPlatform.pressButton();
    }

    public void buttonPressed()
    {
        agent.SetDestination(currentPlatform.transform.position+offset);
        agent.isStopped = false;
        atacking = false;
        pressingButton = false;
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
