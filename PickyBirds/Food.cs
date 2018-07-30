using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

	public float speed = 50f;
	public float fadeSpeed = 1f;
	public float fadeInPosition = 6f; //y value that food starts fading in when it drops under

	public int tone; //can be 1, 2, 3, or 4. Corresponds to neutral, rising, down/up, and descending

	public string roman = "Da"; //romanization of the sound

	public bool held = false;
	public bool startFade = false;
	public bool stopMoving;
	public bool oneClick = false;

	public AudioSource audio;

	public Timer GameTimer;

	private GameObject Cat;
	private SpriteRenderer foodSprite;
	private Color trueColor;
	//private float clickTime = 2f;

	//private float lastClick = 0;

	// Use this for initialization
	void Start () {
		Cat = GameObject.FindGameObjectWithTag ("Player");
		foodSprite = gameObject.GetComponent<SpriteRenderer> ();
		trueColor = foodSprite.color;
		foodSprite.color = Color.clear;
		GameTimer = GameObject.FindWithTag ("Control").GetComponent<Timer> ();

		speed *= FoodSpawner.CurrentSettings.FallSpeed;
	}

	
	// Update is called once per frame
	void Update () {

		//Check for touch
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended && Application.isMobilePlatform && !GameTimer.paused) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			
			if (Physics.Raycast(ray, out hit)) 
			{
				if(hit.transform.gameObject == gameObject)
				{
					Tutorial.SendTutorialAction(Tutorial.TutorialInputs.ClickOnFood);
					Cat.GetComponent<CatController>().Selected = gameObject;

					//Check tap mode and allow single clicking if TapMode isnt 0
					if(PlayerPrefs.GetInt("TapMode") != 0)
					{
						oneClick = true;
						audio.Play();
						Cat.GetComponent<CatController>().AddTime(roman);
					}

					//Check for double click
					if(oneClick && Cat.GetComponent<CatController>().Selected == gameObject && !Tutorial.listening)
					{
						Tutorial.SendTutorialAction(Tutorial.TutorialInputs.PickUpFood);

						//Check if cat already holding something
						if(Cat.GetComponent<CatController>().holding > 0)
						{
							Cat.GetComponent<CatController>().Held.transform.parent = null;	
							Cat.GetComponent<CatController>().Held.GetComponent<Food>().held = false;	
							Cat.GetComponent<CatController>().holding--;
						}

						//Check if food already inside cat collider, otherwise tell cat to move to food
						if(Cat.GetComponent<Collider>().bounds.Contains(gameObject.transform.position))
						{
							held = true;
							Cat.GetComponent<CatController>().GrabFood();
						}
						else
						{
							Cat.GetComponent<CatController>().state = State.Move;
						}
							
					}
					else
					{
						oneClick = true;
						audio.Play();
						Cat.GetComponent<CatController>().AddTime(roman);
						Cat.GetComponent<CatController>().state = State.Twitch;
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
					Cat.GetComponent<CatController>().Selected = gameObject;
					Tutorial.SendTutorialAction(Tutorial.TutorialInputs.ClickOnFood);

					//Check tap mode and allow single clicking if TapMode isnt 0
					if(PlayerPrefs.GetInt("TapMode") != 0)
					{
						oneClick = true;
						audio.Play();
						Cat.GetComponent<CatController>().AddTime(roman);
					}

					//Check for double click
					if(oneClick && Cat.GetComponent<CatController>().Selected == gameObject && !Tutorial.listening)
					{
						Tutorial.SendTutorialAction(Tutorial.TutorialInputs.PickUpFood);

						//Check if cat already holding something
						if(Cat.GetComponent<CatController>().holding > 0)
						{
							Cat.GetComponent<CatController>().Held.transform.parent = null;	
							Cat.GetComponent<CatController>().Held.GetComponent<Food>().held = false;
							Cat.GetComponent<CatController>().holding--;
						}

						//Check if food already inside cat collider, otherwise tell cat to move to food
						if(Cat.GetComponent<Collider>().bounds.Contains(gameObject.transform.position))
						{
							held = true;
							Cat.GetComponent<CatController>().GrabFood();
						}
						else
						{
							Cat.GetComponent<CatController>().state = State.Move;
						}
							
					}
					else
					{
						oneClick = true;
						audio.Play();
						Cat.GetComponent<CatController>().AddTime(roman);
						Cat.GetComponent<CatController>().state = State.Twitch;
					}
				}
			}
		}

		//check if time to start fadin in yet
		if (gameObject.transform.position.y < 6) 
			startFade = true;

		//If not held by cat, move down screen
		if(!held && !stopMoving)
		{
			gameObject.transform.Translate (new Vector3(0,-speed * Time.deltaTime,0));

			if(foodSprite.color.a != 1 && startFade)
				FadeIn ();
		}
		else
		{
			//if held then make color true color
			foodSprite.color = trueColor;
		}

		if(oneClick && Cat.GetComponent<CatController>().Selected != gameObject)
			oneClick = false;

	}


	public void OnTriggerEnter(Collider other)
	{
		//Kill self if enter kill box
		if (other.gameObject.tag == "Kill")
			Destroy (gameObject);

	}

	public void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Stop" && !stopMoving && !held)
		{
			stopMoving = true;
			gameObject.transform.Translate (new Vector3(Random.Range(1,2), 0f, 0f));
			
		}
	}
	
	public void FadeIn()
	{
		// Lerp the colour of the texture between itself and clear
		foodSprite.color = Color.Lerp(foodSprite.color, trueColor, fadeSpeed * speed * Time.deltaTime);
		
		// If the text is almost clear
		if(foodSprite.color.a >= 0.95f)
		{
			foodSprite.color = trueColor;
		}
	}

}
