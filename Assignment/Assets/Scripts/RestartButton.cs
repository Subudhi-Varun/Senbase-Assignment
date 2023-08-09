using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartButton : MonoBehaviour
{
    public CircleSpawner circleSpawner;
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void RestartWithNewCircles()
    {
        SceneManager.LoadScene("SampleScene");
        circleSpawner.SpawnCirclesWithDelay();
    }
}
