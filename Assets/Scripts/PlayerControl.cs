using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    public float speed = 5f;
    public Sprite[] playerSprites = new Sprite[3];

    public Projectile projectile;
    public bool projectileActive;

    public GameObject explosionAnimation;
    public GameObject flashAnimation;
    
    // Start is called before the first frame update
    void Start()
    {
        if (flashAnimation.activeInHierarchy)
        {
            flashAnimation.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= speed * Time.deltaTime;
            gameObject.GetComponent<SpriteRenderer>().sprite = playerSprites[1];
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            position.x += speed * Time.deltaTime;
            gameObject.GetComponent<SpriteRenderer>().sprite = playerSprites[2];
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) ||
            Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = playerSprites[0];
        }

        position.x = Mathf.Clamp(position.x, leftEdge.x + 1f, rightEdge.x - 1f);
        transform.position = position;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if(!projectileActive && gameObject.GetComponent<SpriteRenderer>().enabled)
        {
            Vector3 gunPosition = transform.position;
            gunPosition.y += 1f;
            AudioManager.aManager.Play("PlayerShoot");
            Instantiate(projectile, gunPosition, Quaternion.identity);
            ShowFlash();
            Invoke("HideFlash", 0.1f);
            projectileActive = true;
        }
    }

    public void ShowFlash()
    {
        flashAnimation.SetActive(true);
    }

    public void HideFlash()
    {
        flashAnimation.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile") || collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            PlayerDeath();
        }
    }

    private void SetVisible()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        GameManager.manager.paused = false;
    }

    private void GameOver()
    {
        GameManager.manager.paused = false;
        AudioManager.aManager.StopAll();
        AudioManager.aManager.Play("GameOver");
        SceneManager.LoadScene("GameOver");
    }

    public void PlayerDeath()
    {
        GameManager.manager.paused = true;
        GameManager.manager.currentLives--;

        Text livesNumber = GameObject.FindGameObjectWithTag("LivesNumber").GetComponent<Text>();
        livesNumber.text = GameManager.manager.currentLives.ToString();
        Vector3 deathPosition = gameObject.transform.position;

        if (GameManager.manager.currentLives > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Instantiate(explosionAnimation, deathPosition, Quaternion.identity);
            AudioManager.aManager.Play("PlayerExplosion");
            transform.position = new Vector3(0, -13, 0);
            Invoke("SetVisible", 1.5f);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            // Begin the explosion animation
            Instantiate(explosionAnimation, deathPosition, Quaternion.identity);
            AudioManager.aManager.Play("PlayerExplosion");

            // Delay for the ship explosion;
            Invoke("GameOver", 1.5f);
        }
    }
}
