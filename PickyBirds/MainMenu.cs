using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public float fadeSpeed = 1.5f;          // Speed that the screen fades to and from black.
	public Image black;

	public Toggle TapToggle;
    public Toggle DragToggle;

	public CanvasGroup Main; 			
	public CanvasGroup Levels;
	public CanvasGroup RulesScreen;
	public CanvasGroup CreditsScreen;

	public GameObject SettingsArea;
	public GameObject TitleBird;
	public GameObject TitleBirdMatHolder;

    public GameObject LoadingBird;
    public GameObject LoadingText;

    public GameObject PlzPlayTutorialMessage;

    public bool settingsUp = false;		//Whether settings screen is up or not.
    public bool singleTapMode = false;
    public bool draggingMode = false;

	public Text ToneText;

	public AudioSource VoiceSource;
    public AudioSource MusicSource;

	public AudioClip YellowTone;
	public AudioClip GreenTone;
	public AudioClip BlueTone;
	public AudioClip RedTone;
	
	private bool sceneStarting = true;      // Whether or not the scene is still fading in.

	private int fadeMain = 0; 				//if 0, do nothing. if 1, fade it in. if -1, fade it out.
	private int fadeLevels = 0; 			//if 0, do nothing. if 1, fade it in. if -1, fade it out.
	private int fadeRules = 0; 				//if 0, do nothing. if 1, fade it in. if -1, fade it out.
	private int fadeCredits = 0; 			//if 0, do nothing. if 1, fade it in. if -1, fade it out.
    private int fadeLoading = 0;            //if 0, do nothing. if 1, fade it in.

    public static bool GoToLevelSelect = false;

    public SoundEffects Sounds;

	private Vector3 settingsOrigin;

	// Use this for initialization
	void Start () {
		black.transform.gameObject.SetActive (true);

		settingsOrigin = SettingsArea.transform.position;

		//Check for TapMode. If does not exist, set to defaut which is true
		if (!PlayerPrefs.HasKey ("TapMode"))
			PlayerPrefs.SetInt ("TapMode", 1);

        //Check for DragMode. If does not exist, set to defaut which is false
        if (!PlayerPrefs.HasKey("DragMode"))
            PlayerPrefs.SetInt("DragMode", 0);

        //Use TapMode to set singleTapMode
        if (PlayerPrefs.GetInt ("TapMode") == 0)
			TapToggle.isOn = false;
		else
			TapToggle.isOn = true;

        //Use DragMode to set DragToggle
        if (PlayerPrefs.GetInt("DragMode") == 0)
            DragToggle.isOn = false;
        else
            DragToggle.isOn = true;

        singleTapMode = TapToggle.isOn;
        draggingMode = DragToggle.isOn;

        if (GoToLevelSelect)
		{
			Play ();
			GoToLevelSelect = false;
		}

	}

	
	// Update is called once per frame
	void Update () {
        // If the scene is starting...
        if (sceneStarting)
        {
            // ... call the StartScene function.
            StartScene();
        }

		if (fadeMain == 1)
			FadeInMain ();

		if (fadeMain == -1)
			FadeAwayMain ();

		if (fadeLevels == 1)
			FadeInLevels ();

		if (fadeLevels == -1)
			FadeAwayLevels ();

		if (fadeRules == 1)
			FadeInRules ();

		if (fadeRules == -1)
			FadeAwayRules ();

		if (fadeCredits == 1)
			FadeInCredits ();
		
		if (fadeCredits == -1)
			FadeAwayCredits ();

        if (fadeLoading == 1)
            FadeInLoad();

		if(settingsUp && SettingsArea.transform.position.y < settingsOrigin.y + Screen.height/720f * 80f)
		{
			SettingsArea.GetComponent<RectTransform>().Translate (new Vector3(0,200,0) * Time.deltaTime);
			//SettingsArea.transform.position = new Vector3(0,Screen.height/720f * 60f,0) + settingsOrigin;
		}
		else if(!settingsUp && SettingsArea.transform.position.y > settingsOrigin.y)
		{
			SettingsArea.GetComponent<RectTransform>().Translate (new Vector3(0,-200,0) * Time.deltaTime);
			//SettingsArea.transform.position = settingsOrigin;
		}

		//Clear bird tone text if no voice playing
		if(!VoiceSource.isPlaying)
		{
			ToneText.text = "";
		}
		
	}

	public void TempLevel1() //Filler load level thing for now before dan puts in real one
	{
		Application.LoadLevel ("Test");
	}

	public void Settings()
	{
		settingsUp = !settingsUp;
        Sounds.UIBush();
    }

	public void TapMode()
	{
        singleTapMode = !singleTapMode;

        if (singleTapMode)
            PlayerPrefs.SetInt("TapMode", 1);
        else
            PlayerPrefs.SetInt("TapMode", 0);
	}

    public void DragMode()
    {
        draggingMode = !draggingMode;

        if (draggingMode)
            PlayerPrefs.SetInt("DragMode", 1);
        else
            PlayerPrefs.SetInt("DragMode", 0);
    }

    public void Play()
	{
        Sounds.UIClick();
		fadeMain = -1;
		Invoke ("SetFadeLevels", 0.75f);
	}


	public void Rules()
	{
        Sounds.UIClick();
        fadeMain = -1;
		Invoke ("SetFadeRules", 0.75f);
	}


	public void Credits()
	{
        Sounds.UIClick();
        fadeMain = -1;
		Invoke ("SetFadeCredits", 0.75f);
	}


	public void Exit()
	{
        Sounds.UIClick();
        Application.Quit();
	}

	public void LevelsToRules()
	{
		fadeLevels = -1;
        Sounds.UIBush();
		Invoke ("SetFadeRules", 0.75f);
	}

	public void RulesToLevels ()
	{
		fadeRules = -1;
        Sounds.UIBush();
        Invoke ("SetFadeLevels", 0.75f);
	}
	

	public void BackLevels()
	{
		fadeLevels = -1;
        Sounds.UIBush();
        Invoke ("SetFadeMain", 0.75f);
	}


	public void BackRules()
	{
		fadeRules = -1;
        Sounds.UIBush();
        Invoke ("SetFadeMain", 0.75f);
	}


	public void BackCredits()
	{
		fadeCredits = -1;
        Sounds.UIBush();
        Invoke ("SetFadeMain", 0.75f);
	}


    public void Load()
    {
        fadeLevels = -1;
    }


	void FadeToClear ()
	{
		// Lerp the colour of the texture between itself and transparent.
		black.color = Color.Lerp(black.color, Color.clear, fadeSpeed * Time.deltaTime);
	}


	void FadeToBlack ()
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

            //put up play tutorial message if first time playing
            if (PlayerPrefs.GetInt("HasPlayedTutorial") != 1)
            {
                PlzPlayTutorialMessage.SetActive(true);
                PlzPlayTutorialMessage.GetComponent<PlzPlayTutorialMessage>().OverlayOn();
            }
        }

		//If the texture is halfway clear speed it up
		else if (black.color.a < 0.5f)
			fadeSpeed = 3f;

		//If the texture is 1/4 clear speed it up
		else if (black.color.a < 0.75f)
			fadeSpeed = 1f;


	}

	void SetFadeMain()
	{
		fadeMain = 1;
	}


	void SetFadeLevels()
	{
		fadeLevels = 1;
	}


	void SetFadeRules()
	{
		fadeRules = 1;
	}


	void SetFadeCredits()
	{
		fadeCredits = 1;
	}


    void SetFadeLoading()
    {
        fadeLoading = 1;
    }


	void FadeAwayMain()
	{
		if (Main.alpha >= 0.05f)
		{
			Main.alpha -= Mathf.Lerp (0, 1, 3f * Time.deltaTime);
			//TitleBirdMatHolder.GetComponent<Renderer>().material.color = new Color(1,1,1, Main.alpha);
			TitleBird.SetActive(false);
		}
		else
		{
			Main.alpha = 0;
			//TitleBirdMatHolder.GetComponent<Renderer>().material.color = new Color(1,1,1, Main.alpha);
			fadeMain = 0;
			Main.transform.gameObject.SetActive(false);
			TitleBird.SetActive(false);
		}
	}


	void FadeInMain()
	{
		if (!Main.transform.gameObject.activeSelf)
		{
			Main.transform.gameObject.SetActive (true);
			TitleBird.SetActive(true);
		}
		
		if (Main.alpha <= 0.95f)
			Main.alpha += Mathf.Lerp (0, 1, 3f * Time.deltaTime);
		else
		{
			Main.alpha = 1;
			fadeMain = 0;
		}

		//TitleBirdMatHolder.GetComponent<Renderer>().material.color = new Color(1,1,1, Main.alpha);
	}


	void FadeAwayLevels()
	{
		if (Levels.alpha >= 0.05f)
			Levels.alpha -= Mathf.Lerp (0, 1, 3f * Time.deltaTime);
		else
		{
			Levels.alpha = 0;
			fadeLevels = 0;
			Levels.transform.gameObject.SetActive(false);
		}
	}
	

	void FadeInLevels()
	{
		if (!Levels.transform.gameObject.activeSelf)
			Levels.transform.gameObject.SetActive (true);

		if (Levels.alpha <= 0.95f)
			Levels.alpha += Mathf.Lerp (0, 1, 3f * Time.deltaTime);
		else
		{
			Levels.alpha = 1;
			fadeLevels = 0;
		}
	}

	void FadeAwayRules()
	{
		if (RulesScreen.alpha >= 0.05f)
			RulesScreen.alpha -= Mathf.Lerp (0, 1, 3f * Time.deltaTime);
		else
		{
			RulesScreen.alpha = 0;
			fadeRules = 0;
			RulesScreen.transform.gameObject.SetActive(false);
		}
	}
	
	
	void FadeInRules()
	{
		if (!RulesScreen.transform.gameObject.activeSelf)
			RulesScreen.transform.gameObject.SetActive (true);
		
		if (RulesScreen.alpha <= 0.95f)
			RulesScreen.alpha += Mathf.Lerp (0, 1, 3f * Time.deltaTime);
		else
		{
			RulesScreen.alpha = 1;
			fadeRules = 0;
		}
	}
	

	void FadeAwayCredits()
	{
		if (CreditsScreen.alpha >= 0.05f)
			CreditsScreen.alpha -= Mathf.Lerp (0, 1, 3f * Time.deltaTime);
		else
		{
			CreditsScreen.alpha = 0;
			fadeCredits = 0;
			CreditsScreen.transform.gameObject.SetActive(false);
		}
	}


	void FadeInCredits()
	{
		if (!CreditsScreen.transform.gameObject.activeSelf)
			CreditsScreen.transform.gameObject.SetActive (true);
		
		if (CreditsScreen.alpha <= 0.95f)
			CreditsScreen.alpha += Mathf.Lerp (0, 1, 3f * Time.deltaTime);
		else
		{
			CreditsScreen.alpha = 1;
			fadeCredits = 0;
		}
	}

    void FadeInLoad()
    {
        LoadingBird.SetActive(true);
        LoadingText.SetActive(true);
    }


    //Bird faces buttons
    public void YellowBirdButton()
	{
		ToneText.text = "High-Even Tone";
		VoiceSource.clip = YellowTone;
		VoiceSource.Play ();
        StartCoroutine("LowerMusic");
	}


	public void GreenBirdButton()
	{
		ToneText.text = "Rising Tone";
		VoiceSource.clip = GreenTone;
		VoiceSource.Play ();
        StartCoroutine("LowerMusic");
    }


	public void BlueBirdButton()
	{
		ToneText.text = "Dipping Tone";
		VoiceSource.clip = BlueTone;
		VoiceSource.Play ();
        StartCoroutine("LowerMusic");
    }


	public void RedBirdButton()
	{
		ToneText.text = "Falling Tone";
		VoiceSource.clip = RedTone;
		VoiceSource.Play ();
        StartCoroutine("LowerMusic");
    }

    IEnumerator LowerMusic()
    {
        MusicSource.volume = 0.15f;

        while(VoiceSource.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }

        MusicSource.volume = 1;
    }


	


}