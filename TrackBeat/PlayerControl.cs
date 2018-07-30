using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

	public GameObject invisCube;

	//enum for throw State
	enum ThrowState
	{
		STATE_UP,
		STATE_HOLD,
		STATE_DOWN
	};

	//__Audio
	public AudioSource throwingSound;

	//Camera movement
	public float sensitivityX = 15F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	float rotationX = 0F;
	float rotationY = 0F;

	//Range variables
	public float maxRange = 20;
	public float range = 0;
	public float rangeRate = 0.2f;

	//FOV variables
	public float minFOV = 40;
	public float maxFOV = 60;

	//Current Weapon
	public Weapons.Weapon currWeapon;

	//PowerBar
	public Slider powerBar;
	public Image PowerImage;

	//Private stuff
	private GameObject projectile;
	private GameObject toLoad;
	private Weapons inventory;
	private Weapons.Weapon switchWeapon; //weapons to switch to
	private bool loaded = false; //weapon is loaded and ready to shoot when this is true
	private bool wait = false; //player cant shoot when this is true
	private bool switching = false; //if true, SwitchWeapons Coroutine is running
	private bool shooting = false; //player is currently charging a shot 
	private ThrowState state = ThrowState.STATE_UP;


	// Use this for initialization
	void Start () {
		inventory = gameObject.GetComponent<Weapons> ();
		currWeapon = inventory.items[0];
		toLoad = inventory.Rock;
		LoadWeapon ();
		loaded = true;

		//put powerImage on mouse position
		PowerImage.transform.position = Input.mousePosition + new Vector3(16,-16,0);
	
	}


	// Update is called once per frame
	void Update () {

		//Switching Weapons
		if(Input.GetKeyDown(KeyCode.Alpha1) && currWeapon != inventory.items[0] 
		   && CheckQuantity(inventory.items[0]) && !inventory.items[0].locked)
		{
			if(switchWeapon != inventory.items[0])
			{
				switchWeapon = inventory.items[0];
				toLoad = inventory.Rock;
				StartCoroutine("SwitchWeapons");
			}
		}
		if(Input.GetKeyDown (KeyCode.Alpha2) && currWeapon != inventory.items[1] 
		   && CheckQuantity(inventory.items[1]) && !inventory.items[1].locked)
		{
			if(switchWeapon != inventory.items[1])
			{
				switchWeapon = inventory.items[1];
				toLoad = inventory.Brick;
				StartCoroutine("SwitchWeapons");
			}
		}
		if(Input.GetKeyDown (KeyCode.Alpha3) && currWeapon != inventory.items[2] 
		   && CheckQuantity(inventory.items[2]) && !inventory.items[2].locked)
		{
			if(switchWeapon != inventory.items[2])
			{
				switchWeapon = inventory.items[2];
				toLoad = inventory.Javelin;
				StartCoroutine("SwitchWeapons");
			}
		}
		if(Input.GetKeyDown (KeyCode.Alpha4) && currWeapon != inventory.items[3] 
		   && CheckQuantity(inventory.items[3]) && !inventory.items[3].locked)
		{
			if(switchWeapon != inventory.items[3])
			{
				switchWeapon = inventory.items[3];
				toLoad = inventory.Discus;
				StartCoroutine("SwitchWeapons");
			}
		}
		if(Input.GetKeyDown (KeyCode.Alpha5) && currWeapon != inventory.items[4] 
		   && CheckQuantity(inventory.items[4]) && !inventory.items[4].locked)
		{
			if(switchWeapon != inventory.items[4])
			{
				switchWeapon = inventory.items[4];
				toLoad = inventory.Shotput;
				StartCoroutine("SwitchWeapons");
			}
		}

		//Input for rotating camera
		rotationX += Input.GetAxis("KeyboardRotate") * sensitivityX;
		rotationX = Mathf.Clamp (rotationX, minimumX, maximumX);

		transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		
		//Take input for canceling shot
		if(Input.GetButtonDown("Fire2") && !wait && shooting == true)
		{
			StartCoroutine("CancelShot");
		}

		//Take input for how far to shoot projectile
		if(Input.GetButton ("Fire1") && !wait  && loaded == true)
		{
			shooting = true;

			switch(state)
			{
				case ThrowState.STATE_UP:
					range += rangeRate * Time.deltaTime;
					if(range >= maxRange)
					{
						range = maxRange;
						StartCoroutine("HoldCharge");
						state = ThrowState.STATE_HOLD;
					}
					break;

				case ThrowState.STATE_HOLD:
					break;

				case ThrowState.STATE_DOWN:
					range -= (rangeRate / 2) * Time.deltaTime;
					if(range <= 0)
					{
						range = 0;
						state = ThrowState.STATE_UP;
					}
					break;
			}
		}

		//If shot has ended, smoothly reset power bar
		if(!shooting && range > 0)
		{
			range -= 400 * Time.deltaTime;
		}

		if(range < 0)
		{
			range = 0;
		}


		//Shoot projectile when player releases mouse button
		if(Input.GetButtonUp ("Fire1") && !wait && loaded == true)
		{
			StopCoroutine("HoldCharge");
			state = ThrowState.STATE_HOLD;
			StartCoroutine("Shoot");
		}

		//change powerbar value
		powerBar.value = range/maxRange;
		PowerImage.fillAmount = range / maxRange;

		//put powerImage on mouse position
		PowerImage.transform.position = Input.mousePosition + new Vector3(13,-13,0);// + new Vector3(16,-16,0);

		Camera.main.fieldOfView = Mathf.Lerp (maxFOV, minFOV, powerBar.value);
	

		////Have projectile facing mouse position while not thrown
		if(loaded && projectile != null)
		{
			//Find mouse position and set lookPoint
			Ray ray  = Camera.main.ScreenPointToRay (Input.mousePosition);

			Vector3 lookPoint;

			if(range > 5)
				lookPoint = ray.origin + (ray.direction * range);
			else
				lookPoint = ray.origin + (ray.direction * 5);

			Debug.DrawRay(ray.origin, ray.direction, Color.green, 1);

			projectile.transform.LookAt(lookPoint);
		}
	
	}


	//Function for loading weapon on screen
	//Checks name of currWeapon to know which object to load
	public void LoadWeapon()
	{
		//spawn position for weapons
		Vector3 spawnPOS = Camera.main.transform.position + Camera.main.transform.forward + new Vector3(0.5f, -0.2f, 0);

		//Check is weapon quantity is above 0. If quantity is below 0, it is infinite
		if(!CheckQuantity(currWeapon))
		{
            //load rock since currWeapon does not have a valid quantity
            switchWeapon = inventory.items[0];
            currWeapon = inventory.items[0];
            toLoad = inventory.Rock;
        }

		//load projectile
		projectile = (GameObject)Instantiate (toLoad, spawnPOS, Camera.main.transform.rotation);
		projectile.GetComponent<Rigidbody> ().isKinematic = true;
		projectile.transform.parent = Camera.main.transform;

		rangeRate = currWeapon.chargeRate;
	}


	//Function to check if valid quantity for weapon
	public bool CheckQuantity( Weapons.Weapon item)
	{
		if(item.quantity > 0 || item.quantity < 0)
			return true;
		else
			return false;
	}


	//Coroutine to shoot projectile
	IEnumerator Shoot()
	{

		//Stuff for bouncing weapons
		if (currWeapon.name == "Rock") {
			projectile.GetComponent<Projectile>().rock = true;
		}
		if (currWeapon.name == "Brick") {
			projectile.GetComponent<Projectile>().brick = true;
		}

		//Throwing sounds
		throwingSound.Play ();

		wait = true;
		
		//Find mouse position
		Ray ray  = Camera.main.ScreenPointToRay (Input.mousePosition);

		//calculate direction vector for weapon to aim point
		Vector3 direction = (ray.direction * range + ray.origin) - projectile.transform.position;

		//Turn rigidbody back on and throw the projectile
		projectile.GetComponent<Rigidbody> ().isKinematic = false;
		projectile.rigidbody.AddForce(direction.normalized * range, ForceMode.Impulse);

		projectile.GetComponent<Projectile> ().thrown = true;

		if(currWeapon.stick)
		{
			projectile.GetComponent<Projectile> ().sticky = true;
		}

		projectile.transform.parent = null;
		projectile.GetComponent<Projectile>().dmgModifier = currWeapon.dmg;
	
		loaded = false;
		shooting = false;

		//wait for cooldown
		yield return new WaitForSeconds(currWeapon.coolDown);

		//subtract from weapon quantity and load next weapon 
		currWeapon.SubtractQuantity ();

		//Debug.Log ("switching: " + switching);

		if(!switching)
			LoadWeapon ();

		wait = false;
		loaded = true;

		state = ThrowState.STATE_UP;
	}


	IEnumerator SwitchWeapons()
	{
		switching = true;

		while(range != 0 || loaded == false) //if currently shooting or charging, wait for that to end before switching
		{
			yield return new WaitForSeconds(0.1f);
		}

		loaded = false;
		wait = true;

		//animation of hand coming down???

		yield return new WaitForSeconds(0.2f);
	

		//Debug.Log ("wtf");
		if(currWeapon != switchWeapon)
		{
			if(projectile != null)
			{
				Destroy (projectile);
			}
			currWeapon = switchWeapon;
			LoadWeapon();
		}

		wait = false;
		loaded = true;
		switching = false;
	}


	IEnumerator HoldCharge()
	{
		yield return new WaitForSeconds (2f);

		state = ThrowState.STATE_DOWN;
	}


	IEnumerator CancelShot()
	{
		shooting = false;
		wait = true;

		state = ThrowState.STATE_HOLD;

		range = 0;

		yield return new WaitForSeconds (currWeapon.coolDown);
		
		state = ThrowState.STATE_UP;

		wait = false;
	}
}
