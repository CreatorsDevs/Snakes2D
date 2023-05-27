using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance of the GameManager

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject playerASnake;
    [SerializeField] private GameObject playerBSnake;
    [SerializeField] private GameObject playPanel;
    [SerializeField] private Button playButton;

    private bool isFirstGame; // Flag to track if it's the first game
    private bool isPaused;
    public bool IsPaused
    {
        get {return isPaused;}
    }
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
        Time.timeScale = 0f;
        isPaused = true; // Start the game in paused state
        isGameOver = false; // Reset game over state
        isInputEnabled = true; // Enable input at the start
        isFirstGame = true; // Set the flag to true initially
        playButton.onClick.AddListener(PlayGame); // Add listener to play button
        //ResumeGame();
        if(isFirstGame)
        {
            playPanel.SetActive(true);
        }
        else
        {
            ResumeGame();
        }
        
    }

    private void Update()
    {
        if (isGameOver || !isInputEnabled)
        {
            return;
        }

        // Check for pause/resume input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.instance.Play(SoundNames.buttonSound);
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1f; // Resume the game
        isPaused = false; // Resume the game
        isInputEnabled = true; // Enable input
        isFirstGame = false; // Set the flag to false after the first game
        playPanel.SetActive(false); // Hide the play panel
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
        isGameOver = true;
        AudioManager.instance.Play(SoundNames.gameOverSound);
        Time.timeScale = 0f; // Pause the game
        gameOverMenu.SetActive(true);
        gameOverMenu.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = message;
    }

    public void PlayButtonSound()
    {
        AudioManager.instance.Play(SoundNames.buttonSound);
    }
}

