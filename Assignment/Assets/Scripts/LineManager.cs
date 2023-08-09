using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LineManager : MonoBehaviour
{
    public GameObject linePrefab;
    public LayerMask circleLayer;
    public CircleSpawner circleSpawner;

    private LineRenderer currentLine;
    private List<Collider2D> collidedCircles = new List<Collider2D>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateNewLine();
            if (Input.GetMouseButtonUp(0))
            {
                if (collidedCircles.Count != 0 && circleSpawner.AreCirclesEmpty())
                {
                    SceneManager.LoadScene("GameOverScene"); 
                    Debug.Log("Game Finished");
                }
                Destroy(currentLine.gameObject);
                currentLine = null;
                collidedCircles.Clear();
            }
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentLine.positionCount++;
            currentLine.SetPosition(currentLine.positionCount - 1, mousePosition);

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(mousePosition, 0.1f, circleLayer);
            foreach (Collider2D collider in hitColliders)
            {
                if (!collidedCircles.Contains(collider))
                {
                    collidedCircles.Add(collider);
                    Destroy(collider.gameObject);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Destroy(currentLine.gameObject);
            currentLine = null;
            collidedCircles.Clear();
        }
    }

    private void CreateNewLine()
    {
        GameObject newLine = Instantiate(linePrefab);
        currentLine = newLine.GetComponent<LineRenderer>();
        currentLine.positionCount = 0;
    }
}
