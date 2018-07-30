using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bird : MonoBehaviour {

	public Text scoreText;
	public GameObject scoreObject;

	public GameObject numberObject;

	public GameObject Expressions; //Object giving bird its texture
	public GameObject food; //Food to instantiate and spit out

	public SoundEffects Sounds;

	public Material[] BirdMaterials; //0 is default, 1 is happy, 2 is mistake

	public int currScore = 0;
	public int toFeed = 5;
	public int tone; //can be 1, 2, 3, or 4. Corresponds to neutral, rising, down/up, and descending

	public Timer GameTimer;

	public GameObject Cat;
	private bool badFood = false; //Whether or not the bird has spit out food or not during spit animation

	// Use this for initialization
	void Awake () {
		UpdateScore ();
	}


	void Start() {

		if (FoodSpawner.CurrentSettings.DeactivatedBirds.Find(x => x == tone) > 0)
		{
			scoreObject.SetActive(false);
			gameObject.SetActive(false);
		}


		Cat = GameObject.FindGameObjectWithTag ("Player");
		Sounds = GameObject.FindGameObjectWithTag ("SoundEffects").GetComponent<SoundEffects> ();
	}


	// Update is called once per frame
	void Update () {


		if(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			gameObject.GetComponent<Animator>().SetBool("Dissapoint", false);
			//OriginalTexture();
			badFood = false;
			OriginalTexture();
		}
		if(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Spit"))
		{
			MistakeTexture();

			if(!badFood && gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
			{
				//Shoot bad food out
				GameObject clone;

				clone = (GameObject)Instantiate (food, gameObject.transform.position + new Vector3(-0.6f,0.5f,0), new Quaternion(0,0,0,0));
				clone.GetComponent<Food>().speed = 4;
				clone.GetComponent<CapsuleCollider>().enabled = false;
				clone.tag = "Spit";

				badFood = true;
			}

		}
		if(gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Eat"))
		{
			CorrectTexture();
		}

		//if Cat is holding a tone get excited
		if(Cat.GetComponent<CatController>().Held != null)
		{
			gameObject.GetComponent<Animator>().SetBool("Excite", true);
		}

		//If Bird not selected by Cat then be dissapointed, selected one stays excited
		if(Cat.GetComponent<CatController>().Selected != null && Cat.GetComponent<CatController>().Selected.tag == "Bird")
		{
			if(Cat.GetComponent<CatController>().Selected == gameObject)
			{
				gameObject.GetComponent<Animator>().SetBool("Excite", true);
			}
			else
			{
				gameObject.GetComponent<Animator>().SetBool("Dissapoint", true);
				gameObject.GetComponent<Animator>().SetBool("Excite", false);
				DissapointTexture();
			}
		}

		//Check for touch
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended && Application.isMobilePlatform && !GameTimer.paused) 
		{

			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch (0).position);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit)) 
			{
				if(hit.transform.gameObject == gameObject)
				{

					//Don't tell cat to move if not holding any food
					if(Cat.GetComponent<CatController>().holding > 0)
					{
						if (Tutorial.isTutorial && Cat.GetComponent<CatController>().Held.GetComponent<Food>().tone != tone)
						{
							Sounds.BirdWrong();
						}
						else
						{
							Cat.GetComponent<CatController>().Selected = gameObject;
							Cat.GetComponent<CatController>().state = State.Move;
						}
					}
				}
			}
		}

		//Check for mouse click
		if (Input.GetMouseButtonDown(0) && !Application.isMobilePlatform && !GameTimer.paused)
		{

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast(ray, out hit)) 
			{
				if(hit.transform.gameObject == gameObject)
				{

					//Don't tell cat to move if not holding any food
					if(Cat.GetComponent<CatController>().holding > 0)
					{
						if (Tutorial.isTutorial && Cat.GetComponent<CatController>().Held.GetComponent<Food>().tone != tone)
						{
							Sounds.BirdWrong();
						}
						else
						{
							Cat.GetComponent<CatController>().Selected = gameObject;
							Cat.GetComponent<CatController>().state = State.Move;
						}
					}
				}
			}
		}

		//Check if held food is inside Bird Collider and this Bird is Selected by the player
		if(Cat.GetComponent<CatController>().holding > 0 && Cat.GetComponent<CatController>().Held != null)
		{
			if(gameObject.GetComponent<Collider>().bounds.Contains(Cat.GetComponent<CatController>().Held.transform.position) && Cat.GetComponent<CatController>().Selected == gameObject)
			{
				ReactToFood (Cat.GetComponent<CatController>().Held.GetComponent<Collider>());
			}
		}
	
	}


	public void OnTriggerEnter(Collider other)
	{
		ReactToFood (other);
	}


	public void ReactToFood(Collider other)
	{
		//Check if food coming into mouth
		if (other.gameObject.tag == "Food" && Cat.GetComponent<CatController>().Selected == gameObject) 
		{
			//set Cat reach animation
			//Cat.GetComponent<Animator>().SetTrigger("Reach");
			
			//eating animation
			gameObject.GetComponent<Animator>().SetTrigger("Eat");
            Sounds.InvokeBirdGulp();

            //Check if food is correct tone
            if (other.gameObject.GetComponent<Food>().tone != tone)
			{
				Sounds.BirdWrong();
				
				//spitting animation
				gameObject.GetComponent<Animator>().SetTrigger("Spit");
                Sounds.InvokeBirdSpit();
				
				Cat.GetComponent<CatController>().MistakeTexture();
				
				Destroy (other.gameObject);
				ScoreController.Instance.Incorrect(this);
			}
			else
			{
				TutorialBirdFeed();
				
				Sounds.BirdCorrect();
				
				//return to idle when done eating
				gameObject.GetComponent<Animator>().SetTrigger("Idle");
				
				Cat.GetComponent<CatController>().CorrectTexture();
				
				Destroy (other.gameObject);
				currScore++;
				UpdateScore();
				
				GameObject clone;
				
				clone = (GameObject)Instantiate (numberObject, gameObject.transform.position + new Vector3(0,1,-1), gameObject.transform.rotation);
				clone.transform.LookAt(Camera.main.transform.position); //Fixing number orientation
				clone.transform.localScale = new Vector3(-0.1f,0.1f,0.1f); //Fixing number direction
				ScoreController.Instance.Correct(this);
			}

			//Make sure bird not excited anymore
			Cat.GetComponent<CatController>().Selected = null;
			gameObject.GetComponent<Animator>().SetBool("Excite", false);
			
			Cat.GetComponent<CatController>().holding--;
			Cat.GetComponent<CatController>().Held = null;
			Cat.GetComponent<CatController>().state = State.Return;
		}
	}


	public void UpdateScore()
	{
		scoreText.text = currScore + " / " + toFeed;
		//ScoreController.Instance.UpdateScore();
	}

	//Set texture to default
	public void OriginalTexture()
	{
		Expressions.GetComponent<Renderer> ().material = BirdMaterials [0];
	}
	
	//Set texture for when got a question wrong
	public void MistakeTexture()
	{
		Expressions.GetComponent<Renderer> ().material = BirdMaterials [2];
	}
	
	//Set texture for when got a question right
	public void CorrectTexture()
	{
		Expressions.GetComponent<Renderer> ().material = BirdMaterials [1];
	}

	//Set texture for when not chosen by cat
	public void DissapointTexture()
	{
		Expressions.GetComponent<Renderer> ().material = BirdMaterials [3];
	}

	void TutorialBirdFeed()
	{
		switch (tone)
		{
		case 1:
			Tutorial.SendTutorialAction(Tutorial.TutorialInputs.Bird1);
			break;
		case 2:
			Tutorial.SendTutorialAction(Tutorial.TutorialInputs.Bird2);
			break;
		case 3:
			Tutorial.SendTutorialAction(Tutorial.TutorialInputs.Bird3);
			break;
		case 4:
			Tutorial.SendTutorialAction(Tutorial.TutorialInputs.Bird4);
			break;
		}
	}
}
