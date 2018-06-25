using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killSpider : MonoBehaviour {

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

	public void Disapear()
    {
        GetComponentInParent<SpiderController>().Disapear();
    }

    public void Atack()
    {
        GetComponentInParent<SpiderController>().AttackPlayer();
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
