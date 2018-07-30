using UnityEngine;
using System.Collections;

public class BackpackVoice : MonoBehaviour {

	public AudioSource audioSource;

	public AudioClip[] voiceClipsPositive;
	public AudioClip[] voiceClipsNegative;

	public string corner = "";

	public bool active = false;

	public GameObject backpackBR;
	public GameObject backpackBL;
	public GameObject backpackTL;

	private Vector3 endPoint;
	private Vector3 origPoint;

	private float startTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(active)
		{
			if(!audioSource.isPlaying)
			{
				backpackTL.GetComponent<Animator>().SetBool("Out", false);
				Debug.Log ("Voice false");
				active = false;
			}
		}


	
	}

	public void Activate(bool positive)
	{


		if(!audioSource.isPlaying)
		{
			active = true;
			
			startTime = Time.time;
			
			//choose backpack
			int num = Random.Range (0, 2);
			num = 2;
			
			if (num == 0)
				corner = "BR";
			if (num == 1)
				corner = "BL";
			if (num == 2)
				corner = "TL";
			
			switch(corner)
			{
			case "BR":
				break;
			case "BL":
				break;
			case "TL":
				backpackTL.GetComponent<Animator>().SetBool("Out", true);
				Debug.Log ("Voice true");
				break;
				
			}

			if(positive)
			{
				audioSource.clip = voiceClipsPositive[ChoosePositive ()];
			}
			else
			{
				audioSource.clip = voiceClipsNegative[ChooseNegative ()];
			}
			
			audioSource.Play ();
		}

	}

	public int ChoosePositive()
	{
		return Random.Range (0, 6);
	}

	public int ChooseNegative()
	{
		return Random.Range (0, 4);
	}

}
