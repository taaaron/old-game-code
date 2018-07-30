using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameButtons : MonoBehaviour {

	public Timer GameTimer;

	public Text countdownText;

	public GameObject PauseScreen;
	public Button PauseButton;

    public SoundEffects Sounds;

    public float fadeSpeed = 0.75f;

    public bool start = false;

    private GameObject Moosic;

    // Use this for initialization
    void Start () {
        Moosic = GameObject.Find("Moosic2");
        if(Moosic != null)
            Moosic.GetComponent<AudioSource>().volume = 1;

    }
	
	// Update is called once per frame
	void Update () {

        if (Moosic != null && start)
        {
            Moosic.GetComponent<AudioSource>().volume -= fadeSpeed * Time.deltaTime;

            if (Moosic.GetComponent<AudioSource>().volume <= 0)
                Destroy(Moosic);
        }

        if (Time.timeScale != 0)
		{
			Time.timeScale = Input.GetKey(KeyCode.DownArrow) ? 10f : 1f;
			PauseButton.interactable = true;
		}
			

		else
			PauseButton.interactable = false;
	}

	public void Replay()
	{
        Sounds.UIClick();
		Time.timeScale = 1;
		if (Tutorial.isTutorial)
			Application.LoadLevel ("Tutorial");
		else
			Application.LoadLevel ("Test");
	}

	public void Pause()
	{
        if (Time.timeScale == 0)
		{
            Sounds.UIBush();
            GameTimer.ResumeTimer();
			Time.timeScale = 1;
			//countdownText.text = "";
			PauseScreen.SetActive(false);
		}
		else
		{
            Sounds.UIBush();
            GameTimer.PauseTimer();
			Time.timeScale = 0;
			PauseScreen.SetActive(true);
		}
	}

	public void NextLevel()
	{
        Sounds.UIClick();
        Time.timeScale = 1;

        if (LevelSelectUI.SelectedLevel != 11)
        {
            LevelSelectUI.SelectedLevel++;
            Application.LoadLevel("Test");
        }
        else if (LevelSelectUI.SelectedLevel == 11)
            Application.LoadLevel("Title");
	}

	public void MainMenu()
	{
        if(Moosic != null)
        {
            Destroy(Moosic);
        }
        Sounds.UIClick();
        Time.timeScale = 1;
		Application.LoadLevel ("Title");
	}

    public void GameStart()
    {
        start = true;
    }
}
