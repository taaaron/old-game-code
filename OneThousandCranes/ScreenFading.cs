using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFading : MonoBehaviour {

    public float fadeSpeed = 5f;          // Speed that the screen fades to and from black.
    public Image black;

    //private bool sceneStarting = true;      // Whether or not the scene is still fading in.
    private bool sceneEnding = false;
    private bool sceneStarting = true;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(sceneStarting)
        {
            StartScene();
        }

        if(sceneEnding)
        {
            EndScene();
        }
	
	}

    void FadeToClear()
    {
        // Lerp the colour of the texture between itself and transparent.
        black.color = Color.Lerp(black.color, Color.clear, fadeSpeed * Time.deltaTime);
    }


    void FadeToBlack()
    {
        // Lerp the colour of the texture between itself and black.
        black.color = Color.Lerp(black.color, Color.black, fadeSpeed * Time.deltaTime);
    }

	void StartScene ()
	{
		// Fade the texture to clear.
		FadeToClear();
		
		// If the texture is almost clear...
		if(black.color.a <= 0.05f)
		{
			// ... set the colour to clear and disable the GUITexture.
			black.color = Color.clear;
			black.enabled = false;
			
			// The scene is no longer starting.
			sceneStarting = false;
		}
	}

    public void EndScene()
    {
        // Make sure the texture is enabled.
        black.enabled = true;

        // Start fading towards black.
        FadeToBlack();
    }

    public float GetAlpha()
    {
        return black.color.a;
    }

    public void SetSceneEnd()
    {
        sceneEnding = true;
        sceneStarting = false;
        black.color = Color.clear;
    }
}
