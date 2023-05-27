using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private List<Transform> _segments;
    [SerializeField] private Transform segmentPrefab;
    [SerializeField] private int startingSize = 3; // Default starting size of the snake
    private bool isGameOver; // Game over state
    private bool isInputEnabled; // New variable to track input state
    [SerializeField] private BoxCollider2D snakeGridArea;

    private void Awake() {
        gridPosition = new Vector2Int(4,5);
        gridMoveTimerMax = .2f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1,0);
    }

    private void Start(){
        _segments = new List<Transform>();
        //_segments.Add(this.transform);
        for (int i = 0; i < startingSize; i++)
        {
            AddSegment();
        }
        isGameOver = false; // Reset game over state
        isInputEnabled = true; // Enable input at the start
    }
    private void FixedUpdate() {
        if (!isGameOver) // Only update if game is not over
        {
            HandleGridMovement();
        }
    }

    private void Update()
    {
        if (!isGameOver && isInputEnabled) // Only handle input if game is not over and input state enabled
        {
            HandleInput();
        }
    }

    private void HandleInput(){
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

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            Vector2Int nextGridPosition = gridPosition + gridMoveDirection;

            // Wrap around the screen if the snake goes out of bounds
            WrapAroundScreen(ref nextGridPosition);

            // Check for self-collision
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

        if(other.CompareTag("Food")){
            AddSegment();
        }
    }

    private void GameOver()
    {
        isGameOver = true; // Set game over state
        isInputEnabled = false; // Disable input when game is over
    }

}
