using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance { get; private set; }

    public Animator animation;
    public float animationTime = 1f;

    void Awake()
    {
        instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            NextLevel();
        }*/
    }

    public void NextLevel()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void Restart()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadLevel(0));
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel(int level)
    {
        // Play animation
        animation.SetTrigger("Start");

        // Wait till done
        yield return new WaitForSeconds(animationTime);

        // Load Scene/Level
        SceneManager.LoadScene(level);
    }
}
