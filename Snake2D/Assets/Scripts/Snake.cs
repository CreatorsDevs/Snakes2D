using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Snake : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    [SerializeField] private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private List<Transform> _segments;
    [SerializeField] private Transform segmentPrefab;
    [SerializeField] private int startingSize = 3; // Default starting size of the snake
    [SerializeField] private BoxCollider2D snakeGridArea;
    private int playerScore;
    private const int scoreValue = 7;

    private GameManager gameManager;
    private bool isShieldActive; // Flag to track if the shield power-up is active
    public bool IsShieldActive
    {
        get { return isShieldActive; }
    }
    private bool isScoreBoostActive; // Flag to track if the score boost power-up is active
    private bool isSpeedUpActive; // Flag to track if the speed up power-up is active
    private bool isSnakeDead;
    [SerializeField] public bool isSnakeA;
    private float originalGridMoveTimerMax; // Stores the original grid move timer max value
    [SerializeField] private float speedUpDuration = 5f; // Duration for which the speed up power-up is active
    [SerializeField] private float speedUpMultiplier = 2f; // Multiplier to increase the speed

    [SerializeField] private GameObject shieldPowerUp;
    [SerializeField] private GameObject ScoreBoostPowerUp;
    [SerializeField] private GameObject speedBoostPowerUp;
    [SerializeField] private GameObject scoreBoard;

    private void Awake() {
        gridMoveTimerMax = .2f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1,0);
    }

    private void Start(){
        gameManager = GameManager.instance;
        _segments = new List<Transform>();
        
        for (int i = 0; i < startingSize; i++)
        {
            AddSegment();
        }

        playerScore = 0;
        
        isShieldActive = false; // Initialize shield state
        isScoreBoostActive = false; // Initialize score boost state
        isSpeedUpActive = false; // Initialize speed up state
        originalGridMoveTimerMax = gridMoveTimerMax; // Store the original grid move timer max value
    }
    private void FixedUpdate() {
        if (!gameManager.IsGameOver && !gameManager.IsPaused) // Only update if game is not over and unpaused
        {
            HandleGridMovement();
        }
    }

    private void Update()
    {
        if (!gameManager.IsGameOver && gameManager.IsInputEnabled) // Only handle input if game is not over and input state enabled
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if (isSnakeA)
        {
            if(Input.GetKeyDown(KeyCode.W)){
            
            if (gridMoveDirection.y != -1){
                gridMoveDirection.x = 0;
                gridMoveDirection.y = +1;
            }
            }
            if(Input.GetKeyDown(KeyCode.S)){
            
                if (gridMoveDirection.y != +1){
                    gridMoveDirection.x = 0;
                    gridMoveDirection.y = -1;
                }
            }
            if(Input.GetKeyDown(KeyCode.A)){
            
                if (gridMoveDirection.x != +1){
                    gridMoveDirection.x = -1;
                    gridMoveDirection.y = 0;
                }
            }
            if(Input.GetKeyDown(KeyCode.D)){
            
                if (gridMoveDirection.x != -1){
                    gridMoveDirection.x = +1;
                    gridMoveDirection.y = 0;
                }
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.UpArrow)){
            
            if (gridMoveDirection.y != -1){
                gridMoveDirection.x = 0;
                gridMoveDirection.y = +1;
            }
            }
            if(Input.GetKeyDown(KeyCode.DownArrow)){
            
                if (gridMoveDirection.y != +1){
                    gridMoveDirection.x = 0;
                    gridMoveDirection.y = -1;
                }
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
            
                if (gridMoveDirection.x != +1){
                    gridMoveDirection.x = -1;
                    gridMoveDirection.y = 0;
                }
            }
            if(Input.GetKeyDown(KeyCode.RightArrow)){
            
                if (gridMoveDirection.x != -1){
                    gridMoveDirection.x = +1;
                    gridMoveDirection.y = 0;
                }
            }
        }        
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;

        // Check if the speed up power-up is active
        if (isSpeedUpActive)
        {
            gridMoveTimerMax = originalGridMoveTimerMax / speedUpMultiplier;
            if (gridMoveTimerMax < 0.05f)
            {
                gridMoveTimerMax = 0.05f; // Set a minimum value for the grid move timer max
            }
        }
        else
        {
            gridMoveTimerMax = originalGridMoveTimerMax;
        }

        if (gridMoveTimer >= gridMoveTimerMax)
        {
            Vector2Int nextGridPosition = gridPosition + gridMoveDirection;

            // Wrap around the screen if the snake goes out of bounds
            WrapAroundScreen(ref nextGridPosition);

            // Check for self-collision
            if(!isShieldActive) //If shield is not active
            {
                if (_segments.Count > 1)
                {
                    foreach (Transform segment in _segments)
                    {
                        if (segment.position == new Vector3(nextGridPosition.x, nextGridPosition.y))
                        {
                            // Self-collision detected, end the game or perform game over logic here
                            Debug.Log("GameOver!");
                            GameOver();
                            return;
                        }
                    }
                }
            }
            
            gridPosition = nextGridPosition;
            gridMoveTimer -= gridMoveTimerMax;

            transform.position = new Vector3(gridPosition.x, gridPosition.y, 0f);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90);

            UpdateSegments();
        }
    }


    private float GetAngleFromVector(Vector2Int dir){
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(n<0) n += 360;
        return n;
    }

    private void WrapAroundScreen(ref Vector2Int position)
    {
        if (position.x < Mathf.FloorToInt(snakeGridArea.bounds.min.x))
        {
            position.x = Mathf.FloorToInt(snakeGridArea.bounds.max.x);
        }
        else if (position.x >= Mathf.FloorToInt(snakeGridArea.bounds.max.x + 1))
        {
            position.x = Mathf.FloorToInt(snakeGridArea.bounds.min.x);
        }

        if (position.y < Mathf.FloorToInt(snakeGridArea.bounds.min.y))
        {
            position.y = Mathf.FloorToInt(snakeGridArea.bounds.max.y);
        }
        else if (position.y >= Mathf.FloorToInt(snakeGridArea.bounds.max.y + 1))
        {
            position.y = Mathf.FloorToInt(snakeGridArea.bounds.min.y);
        }
    }

    private void UpdateSegments()
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }
        _segments[0].position = transform.position;
    }

    private void AddSegment()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.position = _segments.Count > 0 ? _segments[_segments.Count - 1].position : transform.position;
        _segments.Add(segment);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(isSnakeDead)
            return;

        if(other.GetComponent<Shield>())
        {
            // Activate the shield power-up
            AudioManager.instance.Play(SoundNames.PickUpSound);
            Destroy(other.gameObject);
            shieldPowerUp.SetActive(true);
            Debug.Log("Shield PowerUp is Active!");
            isShieldActive = true;
            StartCoroutine(DeactivateShield());
        }
        else if (other.GetComponent<ScoreBooster>())
        {
            // Activate the score boost power-up
            AudioManager.instance.Play(SoundNames.PickUpSound);
            Destroy(other.gameObject);
            ScoreBoostPowerUp.SetActive(true);
            Debug.Log("ScoreBoost PowerUp is Active!");
            isScoreBoostActive = true;
            StartCoroutine(DeactivateScoreBoost());
        }
        else if (other.GetComponent<SpeedBooster>())
        {
            // Activate the speed up power-up
            AudioManager.instance.Play(SoundNames.PickUpSound);
            Destroy(other.gameObject);
            speedBoostPowerUp.SetActive(true);
            Debug.Log("Speed PowerUp is Active!");
            isSpeedUpActive = true;
            StartCoroutine(DeactivateSpeedUp());
        }
        else if(isSnakeA && other.GetComponent<PlayerB>())
        {
            if (other.GetComponent<PlayerB>().snake != null && !other.GetComponent<PlayerB>().snake.IsShieldActive)
            {
                // Snake A bites Snake B
                Debug.Log("Snake B Dead! Is Shield Active: " + other.GetComponent<PlayerB>().snake.IsShieldActive);
                gameManager.SnakeBDies();
                other.GetComponent<PlayerB>().snake.isSnakeDead = true;
            }
        }
        else if(!isSnakeA && other.GetComponent<PlayerA>())
        {
            if (other.GetComponent<PlayerA>().snake != null && !other.GetComponent<PlayerA>().snake.IsShieldActive)
            {
                // Snake B bites Snake A
                Debug.Log("Snake A Dead! Is Shield Active: " + other.GetComponent<PlayerA>().snake.IsShieldActive);
                gameManager.SnakeADies();
                other.GetComponent<PlayerA>().snake.isSnakeDead = true;
            }
        }
    }

    private IEnumerator DeactivateShield()
    {
        yield return new WaitForSeconds(10f); // Shield duration
        isShieldActive = false;
        shieldPowerUp.SetActive(false);
    }

    private IEnumerator DeactivateScoreBoost()
    {
        yield return new WaitForSeconds(10f); // Score boost duration
        ScoreBoostPowerUp.SetActive(false);
        isScoreBoostActive = false;
    }

    private IEnumerator DeactivateSpeedUp()
    {
        yield return new WaitForSeconds(speedUpDuration); // Speed up duration
        speedBoostPowerUp.SetActive(false);
        isSpeedUpActive = false;
    }

    public void IncreaseScoreAndSize()
    {
        if (isScoreBoostActive)
        {
            playerScore += 2*scoreValue;
            Debug.Log("ScoreBoost is Active!");
            scoreBoard.GetComponent<TextMeshProUGUI>().text = playerScore.ToString();
        }
        else
        {
            playerScore += scoreValue;
            scoreBoard.GetComponent<TextMeshProUGUI>().text = playerScore.ToString();
            Debug.Log("ScoreBoost is inactive!");
        }

        AddSegment();
    }

    public void DecreaseScoreAndSize()
    {
        if (_segments.Count > startingSize)
        {
            // Remove the last segment from the snake's body
            Transform lastSegment = _segments[_segments.Count - 1];
            _segments.Remove(lastSegment);
            Destroy(lastSegment.gameObject);

            playerScore -= scoreValue;
            scoreBoard.GetComponent<TextMeshProUGUI>().text = playerScore.ToString();
        }
    }

    private void GameOver()
    {
        gameManager.IsGameOver = true; // Set game over state
        gameManager.IsInputEnabled = false; // Disable input when game is over

        if (gameObject.GetComponent<PlayerA>())
        {
            gameManager.SnakeADies();
        }
        else if (gameObject.GetComponent<PlayerB>())
        {
            gameManager.SnakeBDies();
        }
    }

}
