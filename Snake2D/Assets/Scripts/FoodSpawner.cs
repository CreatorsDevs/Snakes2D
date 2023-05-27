using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject massGainerFoodPrefab;
    [SerializeField] private GameObject massBurnerFoodPrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float foodLifetime = 5f;
    [SerializeField] private BoxCollider2D spawnArea;

    private void Start()
    {
        StartCoroutine(SpawnFood());
    }

    private IEnumerator SpawnFood()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            SpawnRandomFood();
            yield return new WaitForSeconds(foodLifetime);

            ClearFood();
        }
    }

    private void SpawnRandomFood()
    {
        GameObject foodPrefab;
        FoodType foodType;

        if (Random.value < 0.5f)
        {
            foodPrefab = massGainerFoodPrefab;
            foodType = FoodType.MassGainer;
        }
        else
        {
            foodPrefab = massBurnerFoodPrefab;
            foodType = FoodType.MassBurner;
        }

        Vector2 spawnPosition = GetRandomSpawnPosition();
        GameObject food = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
        FoodController foodController = food.GetComponent<FoodController>();
        foodController.SetFoodType(foodType);
    }

    private Vector2 GetRandomSpawnPosition()
    {
        float minX = spawnArea.bounds.min.x;
        float maxX = spawnArea.bounds.max.x;
        float minY = spawnArea.bounds.min.y;
        float maxY = spawnArea.bounds.max.y;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }

    private void ClearFood()
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject food in foods)
        {
            Destroy(food);
        }
    }
}

