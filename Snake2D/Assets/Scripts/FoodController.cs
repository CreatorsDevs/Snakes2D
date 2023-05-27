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
                    AudioManager.instance.Play(SoundNames.PickUpSound);
                    snake.IncreaseScoreAndSize();
                    break;
                case FoodType.MassBurner:
                    AudioManager.instance.Play(SoundNames.PickUpSound);
                    snake.DecreaseScoreAndSize();
                    break;
            }

            Destroy(gameObject);
        }
    }
}
