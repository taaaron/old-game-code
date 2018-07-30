using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	//__Source this object
	public GameObject thisObj;

	public bool isHit = false;

	public AudioSource itemHit;

	public float dmg;
	public float dmgModifier;

	public bool thrown = false;
	public bool sticky = false;
	public bool exploded = false;

	public bool rock = false;
	public bool brick = false;
	public bool discus = false;
	public bool javelin = false;
	public bool shotput = false; 

	public Transform parent = null;

	public SphereCollider shotputCollider;

	public ColChanger colChange;

	public GameObject shockwave;
	private ScoreAppear apScore;
	private float destroyTime = 2;


	// Use this for initialization
	void Start () {
		thisObj = gameObject;
		apScore = GameObject.Find ("master_parent").GetComponent<ScoreAppear> ();
	}

	
	// Update is called once per frame
	void Update () {
		if(thrown)
		{
			StartCoroutine("EventuallyDie");

			if(javelin)
			{
				gameObject.transform.forward =
					Vector3.Slerp(gameObject.transform.forward, gameObject.rigidbody.velocity.normalized, Time.deltaTime);
			}
			if(parent != null)
			{
				gameObject.transform.position = parent.position;
			}
		}
	
	}


	void OnTriggerEnter( Collider other )
	{

		if (other.gameObject.tag == "bPart")
        {
			SetForce ();
		}
		if(!shotput && !discus && isHit == false)
		{
			dmg = gameObject.rigidbody.velocity.magnitude * dmgModifier;
			isHit = true;
			apScore.SpawnArea = other.gameObject.transform.position;
			apScore.damager = dmg;
			apScore.SetDamager();
			if(other.gameObject.tag == "Knee"){
				apScore.damager = apScore.damager * 3;
			}
		}

		itemHit.Play ();

		if(sticky && other.gameObject.tag != "Bullet" && other.gameObject.tag != "Holder" && other.gameObject.tag != "Untagged")
		{
			gameObject.rigidbody.velocity = Vector3.zero;
			gameObject.rigidbody.angularVelocity = Vector3.zero;
			gameObject.rigidbody.useGravity = false;
		}
		else if(other.gameObject.tag != "Bullet")
		{
			StartCoroutine ("WaitToDie");
		}

		javelin = false;

		if(shotput && other.gameObject.tag != "Bullet" && other.gameObject.tag != "Holder" 
		   && other.gameObject.tag != "Untagged" && other.gameObject.tag != "runner"
		   && other.gameObject.tag != "enemy")
		{
			dmg = 1.5f;
			shotputCollider.enabled = true;
			gameObject.rigidbody.velocity = Vector3.zero;
			gameObject.rigidbody.angularVelocity = Vector3.zero;

			if(!exploded)
			{
				Shockwave ();
			}
		}

		if(discus)
		{
			gameObject.collider.isTrigger = false;
		}
	}


	void OnCollisionEnter(Collision other)
	{

		if(shotput && other.gameObject.tag != "Bullet")
		{
			dmg = 1.5f;
			shotputCollider.enabled = true;
			gameObject.rigidbody.velocity = Vector3.zero;
			gameObject.rigidbody.angularVelocity = Vector3.zero;

			if(!exploded)
			{
				Shockwave ();
			}
		}
	}


	public void SetForce(){

		if (rock == true)
        {
			thisObj.gameObject.rigidbody.velocity = thisObj.gameObject.rigidbody.velocity * -0.5f;
		}

		if (brick == true)
        {
			thisObj.gameObject.rigidbody.velocity = thisObj.gameObject.rigidbody.velocity * -0.9f;
		}
	}


	public void SetParent (Transform transform)
	{
		parent = transform;
	}


    public void Shockwave()
    {
        exploded = true;
        shotputCollider.enabled = true;
        shotputCollider.radius = 8;

        Instantiate(shockwave, gameObject.transform.position, gameObject.transform.rotation);
        apScore.SpawnArea = gameObject.transform.position;
        apScore.SpawnArea.y = apScore.SpawnArea.y + 2;
        apScore.ShotWord = "A lot";
        apScore.SetAlot();

        Invoke("ColEnd", 2f);
    }


    public void ColEnd()
    {
        shotputCollider.enabled = false;
    }


    IEnumerator WaitToDie()
	{
		yield return new WaitForSeconds(destroyTime);

		Destroy (gameObject);
	}


	IEnumerator EventuallyDie()
	{
		yield return new WaitForSeconds(5);
		
		Destroy (gameObject);
	}
}
