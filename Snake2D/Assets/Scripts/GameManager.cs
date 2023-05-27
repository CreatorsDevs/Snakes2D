using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance of the GameManager

    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject playerASnake;
    public GameObject playerBSnake;

    private bool isPaused;
    private bool isGameOver; // Game over state
    public bool IsGameOver
    {
        get { return isGameOver; }
        set { isGameOver = value; }
    }
    private bool isInputEnabled; // New variable to track input state
    public bool IsInputEnabled
    {
        get { return isInputEnabled; }
        set { isInputEnabled = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        isGameOver = false; // Reset game over state
        isInputEnabled = true; // Enable input at the start
        ResumeGame();
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }

        // Check for pause/resume input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        // Check for restart input
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pause the game
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume the game
        pauseMenu.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SnakeADies()
    {
        GameOver("Player B Wins!");
        Debug.Log("Player B Wins!");
    }

    public void SnakeBDies()
    {
        GameOver("Player A Wins!");
        Debug.Log("Player A Wins!");
    }

    private void GameOver(string message)
    {
        //isGameOver = true;
        Time.timeScale = 0f; // Pause the game
        gameOverMenu.SetActive(true);
        gameOverMenu.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = message;
    }
}

