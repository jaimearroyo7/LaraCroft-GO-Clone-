using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtefactController : MonoBehaviour {

    static bool artLvl1 = false;
    static bool artLvl2 = false;
    static bool artLvl3 = false;

    public bool showArts;

    public GameObject int1;
    public GameObject int2;
    public GameObject int3;

    public GameObject mes1;
    public GameObject mes2;
    public GameObject mes3;

    // Use this for initialization
    void Start () {
        if (showArts) {
            if (artLvl1) {
                int1.SetActive(false);
                mes1.SetActive(true);
            }
            if (artLvl2)
            {
                int2.SetActive(false);
                mes2.SetActive(true);
            }
            if (artLvl3)
            {
                int3.SetActive(false);
                mes3.SetActive(true);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setLvlArt(int n) {
        if (n == 1)
            artLvl1 = true;
        else if (n == 2)
            artLvl2 = true;
        else if (n == 3)
            artLvl3 = true;

        this.gameObject.SetActive(false);
    }
}
