using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {
	
	public float maxHealth = 50;
	public float currHealth;
	public Animator playerAnim;

	// Use this for initialization
	void Start () {
		currHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GotHit(float dmg)
	{
		currHealth -= dmg;
		playerAnim.SetTrigger ("Staggered");
	}
}
