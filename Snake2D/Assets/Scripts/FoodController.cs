using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    private FoodType foodType;

    public void SetFoodType(FoodType type)
    {
        foodType = type;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerA") || other.CompareTag("PlayerB"))
        {
            Snake snake = other.GetComponent<Snake>();

            switch (foodType)
            {
                case FoodType.MassGainer:
                    snake.IncreaseScoreAndSize();
                    break;
                case FoodType.MassBurner:
                    snake.DecreaseScoreAndSize();
                    break;
            }

            Destroy(gameObject);
        }
    }
}
