using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject OffScreen;

    public GameObject MainButtons;

    public GameObject LevelButtons;

    public GameObject CreditsText;

    public Text HighScore1;

    public Text HighScore2;

    public Text HighScore3;

    private Vector3 originalPos;

    private View currView;

    private enum View
    {
        Main,
        Levels,
        Credits,
    }

	// Use this for initialization
	void Start () {
        currView = View.Main;
        LevelButtons.transform.position = OffScreen.transform.position;
        CreditsText.transform.position = OffScreen.transform.position;
        originalPos = MainButtons.transform.position;

        HighScore1.text = PlayerPrefs.GetInt("Score1", 0).ToString() + "/6";
        HighScore2.text = PlayerPrefs.GetInt("Score2", 0).ToString() + "/11";
        HighScore3.text = PlayerPrefs.GetInt("Score3", 0).ToString() + "/12";
    }
	
	// Update is called once per frame
	void Update () {
        switch(currView)
        {
            case View.Main:
                MainButtons.transform.position = originalPos;
                LevelButtons.transform.position = OffScreen.transform.position;
                CreditsText.transform.position = OffScreen.transform.position;
                break;

            case View.Levels:
                LevelButtons.transform.position = originalPos;
                MainButtons.transform.position = OffScreen.transform.position;
                CreditsText.transform.position = OffScreen.transform.position;
                break;

            case View.Credits:
                LevelButtons.transform.position = OffScreen.transform.position;
                MainButtons.transform.position = OffScreen.transform.position;
                CreditsText.transform.position = originalPos;
                break;
        }
	
	}

    public void Credits()
    {
        currView = View.Credits;
    }

    public void Levels()
    {
        currView = View.Levels;
    }

    public void LevelsBack()
    {
        currView = View.Main;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Tutorial()
    {
        PlayerPrefs.SetInt("Level", 0);
        Application.LoadLevel("LoadingScene");
    }

    public void Level1()
    {
        PlayerPrefs.SetInt("Level", 1);
        Application.LoadLevel("LoadingScene");
    }

    public void Level2()
	{
        PlayerPrefs.SetInt("Level", 2);
        Application.LoadLevel("LoadingScene");
	}

    public void Level3()
    {
        PlayerPrefs.SetInt("Level", 3);
        Application.LoadLevel("LoadingScene");
    }
}
