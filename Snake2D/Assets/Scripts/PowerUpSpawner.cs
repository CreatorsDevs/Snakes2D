using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private GameObject scoreBoostPrefab;
    [SerializeField] private GameObject speedUpPrefab;
    [SerializeField] private BoxCollider2D spawnArea;
    [SerializeField] private float spawnInterval = 10f;

    private void Start()
    {
        StartCoroutine(SpawnPowerUps());
    }

    private IEnumerator SpawnPowerUps()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandomPowerUp();
        }
    }

    private void SpawnRandomPowerUp()
    {
        float randomX = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
        float randomY = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        Vector3 spawnPosition = new Vector3(Mathf.Round(randomX), Mathf.Round(randomY), 0f);

        int randomIndex = Random.Range(0, 3);
        GameObject powerUpPrefab = GetPowerUpPrefab(randomIndex);

        GameObject powerUp = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
        Destroy(powerUp, 10f); // Destroy the power-up after 10 seconds (adjust the duration as needed)
    }

    private GameObject GetPowerUpPrefab(int index)
    {
        switch (index)
        {
            case 0:
                return shieldPrefab;
            case 1:
                return scoreBoostPrefab;
            case 2:
                return speedUpPrefab;
            default:
                return null;
        }
    }
}

