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

    public float cooldown = 0.1f;
    float timer = 10f;

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
        if (timer < cooldown)
            timer += Time.deltaTime;
    }

    private void CheckObstacles()
    {
        List<GameObject> toDelete = new List<GameObject>();

        foreach (GameObject g in obstacles)
        {
            if(g.transform.position.y-playerTransform.position.y > obstacleCheckDistance)
            {
                toDelete.Add(g);
            }
            /*
            if (Vector3.Distance(
                Vector3.Scale(g.transform.position, oCheckV3),
                Vector3.Scale(playerTransform.position, oCheckV3)) > obstacleCheckDistance)
            {
                toDelete.Add(g);
            }
            */
        }

        foreach (GameObject g in toDelete)
        {
            obstacles.Remove(g);
            Destroy(g);
            oCount -= 1;
        }
        toDelete.Clear();
    }

    private void FixedUpdate()
    {
        if (!hasBegun)
            return;
        if (timer < cooldown)
            return;
        float r = Random.Range(0f,1f);
        float chanceToSpawn = 0;
        if (oCount < minObstacles)
            chanceToSpawn = 1;
        else
        {
            chanceToSpawn = 1f - (oCount / ((maxObstacles - minObstacles) * difficulty / maxDifficulty + minObstacles));
        }
        chanceToSpawn *= Time.fixedDeltaTime;
        if (chanceToSpawn > r)
        {
            SpawnObstacle();
            Debug.Log(oCount+"| "+chanceToSpawn);
        }

    }

    public void SpawnObstacle()
    {
        int r = Random.Range(0, obstacleList.Count);
        GameObject go = Instantiate(obstacleList[r], spawnPosition.Get(), Quaternion.Euler(90,Random.Range(0f,360f),0f));
        obstacles.Add(go);
        oCount += 1;
        timer = 0;
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
