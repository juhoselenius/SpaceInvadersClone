using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @Author: Juho Selenius
 * With the help of by quarag (https://forum.unity.com/threads/force-camera-aspect-ratio-16-9-in-viewport.385541/).
 */

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float targetAspect = 1f;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;
        Camera camera = Camera.main;

        if (scaleHeight < 1.0f) // add letterbox
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
