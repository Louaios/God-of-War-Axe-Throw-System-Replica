using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBuffer : MonoBehaviour {


    public bool FadeSnow = true;
    public float FillTime = 1f;
    float timer;

    public RenderTexture SnowMap;
    Texture2D ClearTexture;


    // Use this for initialization
    void Start () {
        ClearTexture = new Texture2D(1, 1);
        ClearTexture.SetPixel(0, 0, new Color(255, 255, 255, 0.05f));
        ClearTexture.Apply();
        Graphics.Blit(ClearTexture, SnowMap);
    }

    void FillBuffer(RenderTexture RendTex)
    {
        RenderTexture.active = RendTex;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, RendTex.height, RendTex.width, 0);
        Graphics.DrawTexture(new Rect(0, 0, RendTex.height, RendTex.width), ClearTexture);
        GL.PopMatrix();
        RenderTexture.active = null;
    }
	
	// Update is called once per frame
	void Update () {

        if (FadeSnow)
        {
            if (Time.time > timer)
            {
                timer = Time.time + FillTime;

                    FillBuffer(SnowMap);

            }
        }
	}
}
