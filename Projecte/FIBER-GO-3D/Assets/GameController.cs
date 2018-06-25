using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    Camera cam;
    public PlayerController player;
    public PauseMenu pauseState;
    public Transform newObject;
    public int numLevel;

    private AudioSource audioSource;

    public AudioClip[] audioClips;

    // Use this for initialization
    void Start () {
        cam = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetMouseButtonDown(0) && !pauseState.GamePaused())
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Plataforma ground = hit.collider.GetComponent<Plataforma>();
                if (ground != null)
                {
                    interaccioPlataforma(ground);
                }
                ArtefactController artefact = hit.collider.GetComponent<ArtefactController>();
                if (artefact != null)
                {
                    artefact.setLvlArt(numLevel);
                    playSound(0);
                }
                GemController gem = hit.collider.GetComponent<GemController>();
                if (gem != null)
                {
                    Debug.Log("ieee");
                    gem.getGem(numLevel);
                    playSound(0);
                }
                /*ButtonPlatform button = hit.collider.GetComponent<ButtonPlatform>();
                if(button != null)
                    if (button.platform.entity != null)
                        if (button.platform.entity.tag == "Player")
                            foreach (MovingPlatform movingPlatform in button.movingPlatforms)
                                movingPlatform.Interacciona();*/
            }
        }
    }

    public void interaccioPlataforma(Plataforma ground)
    {
        if (player.MoveToPoint(ground))
        {
            playSound(5);
            LizardController[] lizards = (LizardController[])GameObject.FindObjectsOfType(typeof(LizardController));
            foreach (LizardController lizard in lizards)
                if (lizard != null)
                    lizard.Move(ground);

            SpiderController[] spiders = (SpiderController[])GameObject.FindObjectsOfType(typeof(SpiderController));
            foreach (SpiderController spider in spiders)
                if (spider != null)
                    spider.Move();

            if (player.hasGun)
            {
                calculaShooteables();
            }
        }
        else if (player.hasGun)
        {
            if (ground.entity != null)
            {
                if (ground.entity.tag == "Snake")
                {
                    SnakeController snake = ground.entity.GetComponent<SnakeController>();
                    if (snake.shooteable)
                    {
                        player.shoot(ground);
                        //Instantiate(newObject, transform.position, transform.rotation);
                    }
                }
                else if (ground.entity.tag == "Lizard")
                {
                    LizardController lizard = ground.entity.GetComponent<LizardController>();
                    if (lizard.shooteable)
                    {
                        player.shoot(ground);
                        //Instantiate(newObject, GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0,2f,0), GameObject.FindGameObjectWithTag("Player").transform.rotation * Quaternion.Euler(euler: new Vector3(0, -90, 0)));
                    }
                }
                else if (ground.entity.tag == "Spider")
                {
                    SpiderController spider = ground.entity.GetComponent<SpiderController>();
                    if (spider.shooteable)
                    {
                        player.shoot(ground);
                        //Instantiate(newObject, GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0,2f,0), GameObject.FindGameObjectWithTag("Player").transform.rotation * Quaternion.Euler(euler: new Vector3(0, -90, 0)));
                    }
                }
                //ground.entity.die()
            }

        }
    }

    public void calculaShooteables()
    {
        Plataforma[] allPlats = (Plataforma[])GameObject.FindObjectsOfType(typeof(Plataforma));
        for (int i = 0; i < allPlats.Length; ++i)
        {
            if (allPlats[i] != player.currentPlatform)
            {
                if ((allPlats[i].transform.position.x == player.currentPlatform.transform.position.x || allPlats[i].transform.position.z == player.currentPlatform.transform.position.z)
                    && allPlats[i].transform.position.y == player.currentPlatform.transform.position.y
                    && allPlats[i].entity != null)
                {
                    if (allPlats[i].entity.tag == "Snake")
                        allPlats[i].entity.GetComponent<SnakeController>().startBrilla();
                    if (allPlats[i].entity.tag == "Lizard")
                        allPlats[i].entity.GetComponent<LizardController>().startBrilla();
                    if (allPlats[i].entity.tag == "Spider")
                        allPlats[i].entity.GetComponent<SpiderController>().startBrilla();

                    //allPlats[i].entity.startBrilla()
                }
                else if (allPlats[i].entity != null)
                {
                    //allPlats[i].entity.stopBrilla()
                    if (allPlats[i].entity.tag == "Snake")
                        allPlats[i].entity.GetComponent<SnakeController>().stopBrilla();
                    if (allPlats[i].entity.tag == "Lizard")
                        allPlats[i].entity.GetComponent<LizardController>().stopBrilla();
                    if (allPlats[i].entity.tag == "Spider")
                        allPlats[i].entity.GetComponent<SpiderController>().stopBrilla();
                }

            }
        }
    }

    public void playSound(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    public AudioClip getSound(int index)
    {
        return audioClips[index];
    }
}



