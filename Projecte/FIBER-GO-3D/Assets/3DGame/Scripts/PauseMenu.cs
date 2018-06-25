using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject minimap;

    public Image black;
    public Animator anim;

    private int numberOfScenes;

    // Use this for initialization
    void Start () {
        numberOfScenes = SceneManager.sceneCountInBuildSettings;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
	}

    public void Resume()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().playSound(11);
        pauseMenuUI.SetActive(false);
        minimap.SetActive(true);
        //Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Restart()
    {
        StartCoroutine(Fading(SceneManager.GetActiveScene().buildIndex));
    }

    public void MainMenu()
    {
        StartCoroutine(Fading(0));
    }

    public void NextLevel()
    {
        StartCoroutine(Fading((SceneManager.GetActiveScene().buildIndex + 1)%numberOfScenes));
    }

    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    IEnumerator Fading(int index)
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => black.color.a == 1);
        if (index != 0)
            SceneManager.LoadScene(index);
        else
            SceneManager.LoadScene(0);
    }

    void Pause()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().playSound(11);
        pauseMenuUI.SetActive(true);
        minimap.SetActive(false);
        //Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public bool GamePaused()
    {
        return GameIsPaused;
    }
}
