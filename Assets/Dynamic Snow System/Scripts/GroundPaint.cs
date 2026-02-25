using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPaint : MonoBehaviour
{
    Texture2D whiteTexture;
    public RenderTexture rendText;


    Vector2 UVBuffer;
    int SnowResolution = 512;
    public Texture2D splash;
    public float splashSize = 1;

    // Use this for initialization
    void Start()
    {
        SnowResolution = rendText.width;
    }


    void OnCollisionExit(Collision col)
    {
        CollisionPaint(col);
    }

    void OnCollisionEnter(Collision col)
    {
        CollisionPaint(col);
    }

    void OnCollisionStay(Collision col)
    {
        CollisionPaint(col);
    }


    public void getRTPixels(RenderTexture rt)
    {
        rendText = new RenderTexture(SnowResolution, SnowResolution, 32);
        Graphics.Blit(whiteTexture, rendText);
    }




    void CollisionPaint(Collision collisionInfo)
    {
        RaycastHit ray;
        foreach (ContactPoint contact in collisionInfo.contacts)
        {

            if (Physics.Raycast(contact.point, contact.normal, out ray))

                if (UVBuffer != ray.textureCoord)
                {
                    UVBuffer = ray.textureCoord;
                    Vector2 pixelUV = ray.textureCoord;
                    pixelUV.y *= SnowResolution;
                    pixelUV.x *= SnowResolution;
                    DrawSnowMap(pixelUV.x, pixelUV.y);
                    Debug.DrawRay(contact.point, contact.normal * 10, Color.white);
                }


           
        }
    }


    void DrawSnowMap(float posX, float posY)
    {
        RenderTexture.active = rendText;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, SnowResolution, SnowResolution, 0);

        Graphics.DrawTexture(new Rect(posX - splash.width / splashSize, (rendText.height - posY) - splash.height / splashSize, splash.width / (splashSize * 0.5f), splash.height / (splashSize * 0.5f)), splash);
        GL.PopMatrix();
        RenderTexture.active = null;

    }


    void ClearTexture()
    {
        whiteTexture = new Texture2D(1, 1);
        whiteTexture.SetPixel(0, 0, Color.white);
        whiteTexture.Apply();
    }
}
