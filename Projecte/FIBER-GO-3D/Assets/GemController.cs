using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GemController : MonoBehaviour {

    public int artnum;
    static bool gem1 = false;
    static bool gem2 = false;
    static bool gem3 = false;

    static bool gem4 = false;
    static bool gem5 = false;
    static bool gem6 = false;

    static bool gem7 = false;
    static bool gem8 = false;
    static bool gem9 = false;

    static int numGems = 0;

    public bool show;
    public TextMeshProUGUI mText;

    

    // Use this for initialization
    void Start () {
        if(show)
            mText.text = "Gems: " + numGems.ToString() + "/9";
    }

    public void getGem(int n)
    {
        n = artnum * n;
        if (n == 1 && !gem1) { 
                ++numGems;
                gem1 = true;
        }
        if (n == 2 && !gem2)
        {
            ++numGems;
            gem2 = true;
        }
        if (n == 3 && !gem3)
        {
            ++numGems;
            gem3 = true;
        }
        if (n == 4 && !gem4)
        {
            ++numGems;
            gem4 = true;
        }
        if (n == 5 && !gem5)
        {
            ++numGems;
            gem5 = true;
        }
        if (n == 6 && !gem6)
        {
            ++numGems;
            gem1 = true;
        }
        if (n == 7 && !gem7)
        {
            ++numGems;
            gem1 = true;
        }
        if (n == 8 && !gem8)
        {
            ++numGems;
            gem8 = true;
        }
        else if (n == 9 && !gem9)
        {
            ++numGems;
            gem9 = true;
        }

        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
