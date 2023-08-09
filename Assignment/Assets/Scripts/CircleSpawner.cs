using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawner : MonoBehaviour
{
    public GameObject circlePrefab;
    public int minCircleCount = 0;
    public int maxCircleCount = 10;
    public float spawnDelay = 0.5f;

    private void Start()
    {
        StartCoroutine(SpawnCirclesWithDelay());
    }

    public System.Collections.IEnumerator SpawnCirclesWithDelay()
    {
        int circleCount = Random.Range(minCircleCount, maxCircleCount + 1);
        for (int i = 0; i < circleCount; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-6f, 6f), 0);
            Instantiate(circlePrefab, randomPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay); // Wait for the specified delay
        }
    }

    private void ClearExistingCircles()
    {
        GameObject[] circles = GameObject.FindGameObjectsWithTag("Circle");
        foreach (GameObject circle in circles)
        {
            Destroy(circle);
        }
    }
    public bool AreCirclesEmpty()
    {
        GameObject[] circles = GameObject.FindGameObjectsWithTag("Circle");
        return circles.Length == 0;
    }
}
