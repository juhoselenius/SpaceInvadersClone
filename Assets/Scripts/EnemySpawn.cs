using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * @Author: Juho Selenius
 * With the help of "How to make Space Invaders in Unity (Complete Tutorial)"
 * by Zigurous (https://www.youtube.com/watch?v=qWDQgmdUzWI).
 */

public class EnemySpawn : MonoBehaviour
{
    public Enemy[] enemyList = new Enemy[5];
    public int enemyRows = 5;
    public int enemyColumns = 11;
    public float enemySpacing = 2f;
    private Vector3 movementDirection = Vector3.right;
    private Vector3 spawnGridPosition;
    private Text levelText;
    public AnimationCurve speed;

    private float counter = 0f;

    public Projectile enemyProjectile;
    public float enemyProjectileRate = 1f;
    public int enemyProjectilesActive = 0;
    
    public int remainingEnemies;
    public float percentAlive => (float)remainingEnemies / ((float)enemyRows * (float)enemyColumns);

    private void Awake()
    {
        levelText = GameObject.FindGameObjectWithTag("LevelText").GetComponent<Text>();

        spawnGridPosition = transform.position;
        SpawnEnemies();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        //Getting the screen edge coordinates
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        if(GameManager.manager.paused == false)
        {
            //transform.position += movementDirection * speed.Evaluate(percentAlive) * GameManager.manager.currentLevel * Time.deltaTime;
            transform.position += movementDirection * 5 * GameManager.manager.currentLevel * Time.deltaTime; // REMOVE THIS FROM FINAL!!!!

            foreach (Transform enemy in transform)
            {
                if (movementDirection == Vector3.right && enemy.position.x >= (rightEdge.x - 1f))
                {
                    GoDownRow();
                }
                else if (movementDirection == Vector3.left && enemy.position.x <= (leftEdge.x + 1f))
                {
                    GoDownRow();
                }
            }
        }
        else
        {
            transform.position += movementDirection * 0 * Time.deltaTime;
        }

        // Resetting enemy grid, when all enemies have been destroyed
        if(remainingEnemies == 0)
        {
            transform.position = spawnGridPosition;
            movementDirection = Vector3.right;
            SpawnEnemies();
            counter = 0;
        }


        // Delaying the first enemy attack until 2 second mark
        if(GameManager.manager.paused == false)
        {
            if (counter > 2f)
            {
                EnemyAttack();
            }
        }
    }

    private void GoDownRow()
    {
        //Changing direction
        movementDirection *= -1f;

        //Going down one row
        Vector3 newPosition = transform.position;
        newPosition.y -= 1f;
        transform.position = newPosition;
    }

    private void SpawnEnemies()
    {
        for (int x = 0; x < enemyRows; x++)
        {
            float gridWidth = enemySpacing * (enemyColumns - 1);
            float gridHeight = enemySpacing * (enemyRows - 1);

            Vector2 centerOffset = new Vector2(-gridWidth * 0.5f, -gridHeight * 0.5f);
            Vector3 rowPosition = new Vector3(centerOffset.x, (enemySpacing * x) + centerOffset.y, 0f);

            for (int y = 0; y < enemyColumns; y++)
            {
                Enemy enemy = Instantiate(enemyList[x], transform);

                enemy.rowValue = x + 1;
                enemy.columnValue = y + 1;

                Vector3 position = rowPosition;
                position.x += enemySpacing * y;
                enemy.transform.localPosition = position;

                if(x == 0)
                {
                    SetProjectileSpawn(enemy.transform);
                }
            }
        }

        remainingEnemies = enemyColumns * enemyRows;

        GameManager.manager.currentLevel++;

        if(levelText != null)
        {
            levelText.text = "Level " + GameManager.manager.currentLevel;
        }
    }

    private void EnemyAttack()
    {
        if(remainingEnemies == 0)
        {
            return;
        }

        GameObject[] projectileSpawnArray = GameObject.FindGameObjectsWithTag("ProjectileSpawn");

        int random = Random.Range(0, projectileSpawnArray.Length - 1);

        if(((float)remainingEnemies / ((float)enemyColumns * (float)enemyRows)) > 0.5)
        {
            if (enemyProjectilesActive == 0)
            {
                Instantiate(enemyProjectile, projectileSpawnArray[random].transform.position, Quaternion.identity);
                enemyProjectilesActive++;
            }
        }
        else
        {
            if (enemyProjectilesActive < 2)
            {
                Instantiate(enemyProjectile, projectileSpawnArray[random].transform.position, Quaternion.identity);
                enemyProjectilesActive++;
            }
        }
    }

    public void SetProjectileSpawn(Transform enemy)
    {
        GameObject projectileSpawn = new GameObject();
        projectileSpawn.name = "ProjectileSpawn";
        projectileSpawn.tag = "ProjectileSpawn";
        projectileSpawn.transform.parent = enemy.transform;

        projectileSpawn.transform.position = enemy.transform.position;
    }
}
