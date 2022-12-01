using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyMainMenu : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] spriteAnimationList = new Sprite[0];
    public float animationTime = 1f;
    private int animationFrame = 0;
    public float turnTime;
    public float animationLife;
    public bool right;
    public int moveSpeed;

    public float counter;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteAnimationList[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
        counter = 0;
        right = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter += Time.fixedDeltaTime;

        if(right && counter < turnTime)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else if (right && counter > turnTime)
        {
            right = false;
            counter = 0;
        }
        else if (!right && counter < turnTime)
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        else if (!right && counter > turnTime)
        {
            right = true;
            counter = 0;
        }
    }

    private void AnimateSprite()
    {
        animationFrame++;

        if (animationFrame >= spriteAnimationList.Length)
        {
            animationFrame = 0;
        }

        spriteRenderer.sprite = spriteAnimationList[animationFrame];

    }
}
