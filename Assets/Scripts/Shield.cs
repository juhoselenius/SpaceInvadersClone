using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Texture2D originalTexture;
    public SpriteRenderer spriteRenderer;
    public BoxCollider2D bCollider;
    public int radius;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalTexture = spriteRenderer.sprite.texture;
        bCollider = GetComponent<BoxCollider2D>();

        ResetShield();
    }

    private void Update()
    {

    }

    public void ResetShield()
    {
        // Each shield needs a unique instance of the sprite texture since it will be modified at the source
        CopyTexture(originalTexture);

        gameObject.SetActive(true);
    }

    private void CopyTexture(Texture2D original)
    {
        Texture2D copy = new Texture2D(original.width, original.height, original.format, false);
        copy.filterMode = FilterMode.Point;
        copy.anisoLevel = original.anisoLevel;
        copy.wrapMode = original.wrapMode;
        copy.SetPixels(original.GetPixels());
        copy.Apply();

        Sprite sprite = Sprite.Create(copy, spriteRenderer.sprite.rect, new Vector2(0.5f, 0.5f), spriteRenderer.sprite.pixelsPerUnit);
        spriteRenderer.sprite = sprite;
    }

    public bool CheckPoint(Vector3 hitPoint, GameObject collidingObject, out int px, out int py)
    {
        // Transform the point from world space to local space
        Vector3 localPoint = transform.InverseTransformPoint(hitPoint);

        // Offset the point to the corner of the object instead of the center so we can transform to uv coordinates
        localPoint.x += bCollider.size.x / 2;
        localPoint.y += bCollider.size.y / 2;

        Texture2D texture = spriteRenderer.sprite.texture;

        // Transform the point from local space to uv coordinates
        px = (int)((localPoint.x / bCollider.size.x) * texture.width);
        py = (int)((localPoint.y / bCollider.size.y) * texture.height);

        // Return true if the pixel is not empty (not transparent)
        if (texture.GetPixel(px, py).a == 1.0)
        {
            // Correcting the hit to the edge of the shield, if update frequency has set it inside the shield
            if (collidingObject.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
            {
                while (texture.GetPixel(px, py).a == 1.0)
                {
                    py++;
                }
            }

            // Correcting the hit to the edge of the shield, if update frequency has set it inside the shield
            if (collidingObject.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
            {
                while (texture.GetPixel(px, py).a == 1.0)
                {
                    py--;
                }
            }

            // Pixel is not empty.
            return true;
        }
        else
        {
            // Pixel is empty (transparent).
            return false;
        }
    }

    public bool CheckDamage(Vector3 hitPoint, GameObject collidingObject)
    {
        int px;
        int py;

        // Only proceed if the point maps to a non-empty pixel
        if (!CheckPoint(hitPoint, collidingObject, out px, out py))
        {
            return false;
        }

        Texture2D texture = spriteRenderer.sprite.texture;

        if (collidingObject.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            radius = collidingObject.GetComponent<Enemy>().destructionRadius;
        }
        if (collidingObject.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile") || collidingObject.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            radius = collidingObject.GetComponent<Projectile>().destructionRadius;
        }

        // Non-empty pixel has been hit. Take the surrounding pixels and change them as transparent.
        Circle(texture, px, py, radius, Color.clear);

        texture.Apply();

        return true;
    }

    public void Circle(Texture2D tex, int cx, int cy, int r, Color col)
    {
        int x, y, px, nx, py, ny, d;

        for (x = 0; x <= r; x++)
        {
            d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));

            for (y = 0; y <= d; y++)
            {

                px = cx + x;
                nx = cx - x;
                py = cy + y;
                ny = cy - y;

                tex.SetPixel(px, py, col);
                tex.SetPixel(nx, py, col);

                tex.SetPixel(px, ny, col);
                tex.SetPixel(nx, ny, col);
            }
        }
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            radius = 30;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile") || other.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            radius = 5;
        }
    }*/
}
