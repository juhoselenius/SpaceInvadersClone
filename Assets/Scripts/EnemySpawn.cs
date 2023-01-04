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

    public Projectile[] enemyProjectileList;
    public float enemyProjectileRate;
    public float projectileCounter;
    
    public int remainingEnemies;
    public float percentAlive => (float)remainingEnemies / ((float)enemyRows * (float)enemyColumns);

    private void Awake()
    {
        levelText = GameObject.FindGameObjectWithTag("LevelText").GetComponent<Text>();

        projectileCounter = 0;

        spawnGridPosition = transform.position;
        SpawnEnemies();
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
            transform.position += movementDirection * speed.Evaluate(percentAlive) * GameManager.manager.currentLevel * Time.deltaTime;

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
                projectileCounter += Time.deltaTime;
                if (projectileCounter > (enemyProjectileRate * percentAlive + 0.5f) / GameManager.manager.currentLevel)
                {
                    EnemyAttack();
                    projectileCounter = 0;
                }
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

        if(!GameManager.manager.gameLoaded)
        {
            GameManager.manager.currentLevel++;

            // Saving the score and lives when level changes
            GameManager.manager.savedScore = GameManager.manager.currentScore;
            GameManager.manager.savedLives = GameManager.manager.currentLives;
        } else
        {
            GameManager.manager.gameLoaded = false;
        }

        if (levelText != null)
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

        switch (projectileSpawnArray[random].transform.parent.tag)
        {
            case "BottomEnemy":
                Instantiate(enemyProjectileList[0], projectileSpawnArray[random].transform.position, Quaternion.identity);
                break;
            case "MidEnemy":
                Instantiate(enemyProjectileList[1], projectileSpawnArray[random].transform.position, Quaternion.identity);
                break;
            case "TopEnemy":
                Instantiate(enemyProjectileList[2], projectileSpawnArray[random].transform.position, Quaternion.identity);
                break;
        }

        AudioManager.aManager.Play("EnemyShoot");
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
