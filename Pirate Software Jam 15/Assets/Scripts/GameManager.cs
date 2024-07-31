using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool inMenu;

    [SerializeField] private GameObject gameOverUI;

    void Awake()
    {
        instance = this;

        gameOverUI.SetActive(false);
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
        inMenu = true;
        
        Time.timeScale = 0;
    }
}
