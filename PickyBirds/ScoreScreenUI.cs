using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreScreenUI : MonoBehaviour {

	public Text YourScore;
	public Text YourNumber;
	public Text BestNumber;
	public Text NewHighScoreMessage;

	public GameObject[] Stars;

	public GameObject WinScreen;
	public GameObject LoseScreen;

    public GameObject UIKitty;

	public SoundEffects Sounds;
	// Use this for initialization
	void Start () {
		Sounds = GameObject.FindGameObjectWithTag ("SoundEffects").GetComponent<SoundEffects> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CountUpToScore(float score, float highScore)
	{
		StartCoroutine(CountUpToScoreCoroutine(score, highScore));
	}

	float countUpTimer = 0f;
	const float countUpTime = 1.5f;
	const float countUpTime2 = 0.5f; //if the high score is higher
	float displayScore;
	int starsEarned = 0;
	IEnumerator CountUpToScoreCoroutine(float score, float highScore)
	{
		countUpTimer = 0f;

        UIKitty.GetComponent<UICat>().SetMove(true);

        //only play calculating sound if score higher than 0
        if (score > 0)
        {
            Sounds.UIYourScore();

            //count up loop
            while (countUpTimer < countUpTime)
            {
                countUpTimer += Time.deltaTime;
                displayScore = Mathf.Round(score * countUpTimer / countUpTime);
                YourNumber.text = displayScore.ToString();

                BestNumber.text = highScore.ToString();


                if (displayScore >= ScoreController.GetNextStarScore(starsEarned) && starsEarned <= Stars.Length)
                {
                    Stars[starsEarned].SetActive(true);
                    starsEarned++;
                    Sounds.FoodSucess();
                }

                //Once score is done calculating, stop calculating sound
                if (displayScore == score)
                    Sounds.UIStop();

                yield return new WaitForEndOfFrame();
            }
        }

		if (ScoreController.Instance.AllBirdsFed())
		{
            UIKitty.GetComponent<UICat>().SetWin();
			LoadWin ();
		}
		else
		{
            UIKitty.GetComponent<UICat>().SetLose();
            LoadLose();
		}


		YourNumber.text = score.ToString();
		YourNumber.color = new Color(0.18f,0.223f,0.255f);
		YourScore.color = new Color(0.18f,0.223f,0.255f);

		countUpTimer = 0f;

		BestNumber.text = highScore.ToString();

		//new high score!
		if (score > highScore && ScoreController.Instance.AllBirdsFed())
		{
			NewHighScoreMessage.gameObject.SetActive(true);
			yield return new WaitForSeconds(2.0f);
			countUpTimer = 0f;
			Sounds.UIHighScore();

			while (countUpTimer < countUpTime2)
			{
				countUpTimer += Time.deltaTime;
				displayScore = Mathf.Round( highScore + (score - highScore) * countUpTimer / countUpTime2 );
				BestNumber.text = displayScore.ToString();
				yield return new WaitForEndOfFrame();
				
			}

			PlayerPrefs.SetFloat("highscore" + FoodSpawner.Instance.levelNumber, score);
			countUpTimer = 0f;

			BestNumber.text = score.ToString();
            Sounds.UIStop();
		}



	}

	void LoadWin()
	{
		WinScreen.SetActive (true);
		Sounds.BGMWinPurr ();
	}
	
	void LoadLose()
	{
		LoseScreen.SetActive (true);
		Sounds.BGMLosePurr ();
	}




}
