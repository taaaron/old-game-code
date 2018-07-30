using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CraneCollect : MonoBehaviour {

    public Text CraneText;

    public int collected = 0;
    public int cranesInLevel = 7;

    private GameObject shrine;
    private int levelNum = 0;

    // sounds
    private SFXManager SFXMan;

	// Use this for initialization
	void Start () {
        //Get number of cranes from wherever we save it
        if(PlayerPrefs.HasKey("Level"))
        {
            levelNum = PlayerPrefs.GetInt("Level");
            
            switch(levelNum)
            {
                case 0:
                    cranesInLevel = 4;
                    break;
                case 1:
                    cranesInLevel = 4;
                    break;
                case 2:
                    cranesInLevel = 6;
                    break;
                case 3:
                    cranesInLevel = 8;
                    break;
                default:
                    cranesInLevel = 0;
                    break;
            }
        }
        else
        {
            cranesInLevel = 0;
        }


        SetCranesText();
        shrine = GameObject.FindGameObjectWithTag("Shrine");
        shrine.GetComponent<OpenPortal>().countersNeeded = cranesInLevel;
        SFXMan = GameObject.Find("SFXManager").GetComponent<SFXManager>();
    }

    // Update is called once per frame
    void Update () {
        
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Crane")
        {
            collected++;
            Destroy(other.gameObject);
            //fancy animation or particle effect

            // Sound
            SFXMan.PlayCraneCollectSound();

            SetCranesText();
            SetHighScore();
            shrine.GetComponent<OpenPortal>().AddCounter();
        }
    }

    public void SubtractCranes(int num)
    {
        collected = collected - num;

        SetCranesText();
    }

    public void SetCranesText()
    {
        CraneText.text = collected + "/" + cranesInLevel;
    }

    public void SetHighScore()
    {
        if(PlayerPrefs.HasKey("Level"))
        {
            if(levelNum == 1)
            {
                if(PlayerPrefs.GetInt("Score1", 0) < collected)
                {
                    PlayerPrefs.SetInt("Score1", collected);
                }
            }
            if (levelNum == 2)
            {
                if (PlayerPrefs.GetInt("Score2", 0) < collected)
                {
                    PlayerPrefs.SetInt("Score2", collected);
                }
            }
            if (levelNum == 3)
            {
                if (PlayerPrefs.GetInt("Score3", 0) < collected)
                {
                    PlayerPrefs.SetInt("Score3", collected);
                }
            }
        }
    }
}
