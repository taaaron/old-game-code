using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapons : MonoBehaviour {
	
	public class Weapon
	{
		public string name;
		public float dmg;
		public int quantity;
		public float coolDown;
		public int price;
		public bool stick;
		public float chargeRate;
		public bool locked;
		
		//Constructor
		public Weapon()
		{
			name = "Rock";
			dmg = 0.5f;
			quantity = -1;
			coolDown = 0.5f;
			price = 0;
			chargeRate = 100;
			stick = false;
		}

		public Weapon(string newName, float newDmg, int newQuantity, float newCoolDown, int newPrice, float newCharge, bool newStick)
		{
			name = newName;
			dmg = newDmg;
			quantity = newQuantity;
			coolDown = newCoolDown;
			price = newPrice;
			chargeRate = newCharge;
			stick = newStick;
		}

		public void SubtractQuantity()
		{
			if(quantity > 0)
			{
				quantity--;
			}
		}
	}

	//starting quantities
	public int InitialBrick = 5;
	public int InitialJavelin = 5;
	public int InitialDiscus = 5;
	public int InitialShotput = 5;

	//weapon locked
	public bool LockBrick = false;
	public bool LockJavelin = false;
	public bool LockDiscus = false;
	public bool LockShotput = false;

	//Weapon objects
	public GameObject Rock;
	public GameObject Brick;
	public GameObject Javelin;
	public GameObject Discus;
	public GameObject Shotput;

	//List of current weapons
	public List<Weapon> items;

	//weapon prices
	private int BrickPrice = 2;
	private int JavelinPrice = 7;
	private int DiscusPrice = 3;
	private int ShotputPrice = 7;

	//weapon dmg modifiers
	private float RockMod = 1f;
	private float BrickMod = 0.5f;
	private float JavelinMod = 5;
	private float DiscusMod = 0.5f;
	private float ShotputMod = 3;

	//weapon power speed
	private float RockCharge = 100;
	private float BrickCharge = 75;
	private float JavelinCharge = 40;
	private float DiscusCharge = 75;
	private float ShotputCharge = 40;

	// Use this for initialization before Start
	void Awake () {

		//Instantiate list and default items
		items = new List<Weapon>();
		items.Add (new Weapon ());
		items[0].dmg = RockMod;
		items [0].chargeRate = RockCharge;
		items.Add (new Weapon("Brick", BrickMod, InitialBrick, 1, BrickPrice, BrickCharge, false));
		items.Add (new Weapon("Javelin", JavelinMod, InitialJavelin, 1, JavelinPrice, JavelinCharge, true));
		items.Add (new Weapon("Discus", DiscusMod, InitialDiscus, 1, DiscusPrice, DiscusCharge, false));
		items.Add (new Weapon("Shotput", ShotputMod, InitialShotput, 1, ShotputPrice, ShotputCharge, false));

		if(LockBrick)
			items[1].locked = true;
		if(LockJavelin)
			items[2].locked = true;
		if(LockDiscus)
			items[3].locked = true;
		if(LockShotput)
			items[4].locked = true;
	
	}
}
