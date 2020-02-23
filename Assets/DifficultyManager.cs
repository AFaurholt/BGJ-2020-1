using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] PositionProvider spawnPosition;
    [SerializeField] bool hasBegun = false;

    public float maxDifficulty = 100;
    [SerializeField] float difficulty;

    public float minObstacles = 2;
    public float maxObstacles = 30;
    [SerializeField] float oCount;

    public List<GameObject> obstacleList;
    public List<GameObject> obstacles;

    public Transform playerTransform;

    [SerializeField] Vector3 oCheckV3;
    public float obstacleCheckDistance = 10;

    void Start()
    {
        ResetDifficulty();
        //BeginDifficulty(); //comment later on
    }

    public void BeginDifficulty()
    {
        hasBegun = true;
    }

    void Update()
    {
        if (!hasBegun)
            return;
        if (difficulty < maxDifficulty)
            difficulty = Mathf.Min(difficulty + Time.deltaTime, maxDifficulty);
        CheckObstacles();
    }

    private void CheckObstacles()
    {
        List<GameObject> toDelete = new List<GameObject>();

        foreach (GameObject g in obstacles)
        {
            if (Vector3.Distance(
                Vector3.Scale(g.transform.position, oCheckV3),
                Vector3.Scale(playerTransform.position, oCheckV3)) > obstacleCheckDistance)
            {
                toDelete.Add(g);
            }

        }

        foreach (GameObject g in toDelete)
        {
            obstacles.Remove(g);
            Destroy(g);
        }
        toDelete.Clear();
    }

    private void FixedUpdate()
    {
        if (!hasBegun)
            return;
        float r = Random.value;
        float chanceToSpawn = 0;
        if (oCount < minObstacles)
            chanceToSpawn = 1;
        else
        {
            chanceToSpawn = 1f - (oCount / ((maxObstacles - minObstacles) * difficulty / maxDifficulty + minObstacles));
        }

        if (chanceToSpawn > r)
        {
            SpawnObstacle();
        }

    }

    public void SpawnObstacle()
    {
        int r = Random.Range(0, obstacleList.Count);
        GameObject go = Instantiate(obstacleList[r], spawnPosition.Get(), Quaternion.identity);
        obstacles.Add(go);
        // extra stuff like giving them speed
    }

    public void ResetDifficulty()
    {
        foreach (GameObject g in obstacles)
        {
            Destroy(g);
        }
        obstacles.Clear();

        oCount = 0;
        difficulty = 0;

        hasBegun = false;
    }
}
