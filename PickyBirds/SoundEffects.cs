using UnityEngine;
using System.Collections;

public class SoundEffects : MonoBehaviour {

	//Audio Sources
	public AudioSource BirdSource;
    public AudioSource BirdSource2;
	public AudioSource CatSource;
	public AudioSource UISource;
	public AudioSource FoodSource;
    public AudioSource BGMSource;

	//Bird Sounds
	public AudioClip Wrong;
	public AudioClip Correct;
    public AudioClip Gulp;
    public AudioClip Spit;

	//Cat Sounds
	public AudioClip Walking;

	//UI Sounds
	public AudioClip YourScore;
	public AudioClip HighScore;
	public AudioClip WinPurr;
	public AudioClip LosePurr;
    public AudioClip Click;
    public AudioClip Bush;
    public AudioClip Ready;
    public AudioClip Go;

	//FoodSounds
	public AudioClip Success;
	public AudioClip PickUp;

	// Use this for initialization
	void Start () {
	
	}

	
	// Update is called once per frame
	void Update () {
	
	}


	public void BirdStop()
	{
		BirdSource.Stop ();
	}


	public void CatStop()
	{
		CatSource.Stop ();
	}


	public void UIStop()
	{
		UISource.Stop ();
	}


	public void BirdWrong()
	{
		BirdSource.clip = Wrong;
		BirdSource.Play ();
	}


	public void BirdCorrect()
	{
		BirdSource.clip = Correct;
		BirdSource.Play ();
	}


    public void BirdGulp()
    {
        if (Gulp != null)
        {
            BirdSource2.clip = Gulp;
            BirdSource2.Play();
        }
    }


    public void BirdSpit()
    {
        if (Spit != null)
        {
            BirdSource2.clip = Spit;
            BirdSource2.Play();
        }
    }


    public void InvokeBirdGulp()
    {
        Invoke("BirdGulp", 0.3f);
    }


    public void InvokeBirdSpit()
    {
        Invoke("BirdSpit", 1.3f);
    }


	public void CatWalking()
	{
		CatSource.clip = Walking;
		CatSource.loop = true;
		CatSource.pitch = 2;
		CatSource.volume = 0.2f;
		CatSource.Play ();
	}


	public void UIYourScore()
	{
		UISource.clip = YourScore;
		UISource.loop = false;
        UISource.outputAudioMixerGroup.audioMixer.SetFloat("EffectsVolume", 0);
        UISource.Play ();
	}


	public void UIHighScore()
	{
		UISource.clip = HighScore;
		UISource.loop = false;
        UISource.outputAudioMixerGroup.audioMixer.SetFloat("EffectsVolume", 0);
        UISource.Play ();
	}


    public void UIClick()
    {
        if (Click != null)
        {
            UISource.clip = Click;
            UISource.loop = false;
            UISource.outputAudioMixerGroup.audioMixer.SetFloat("EffectsVolume", 0);
            UISource.Play();
        }
    }

    public void UIBush()
    {
        if(Bush != null)
        {
            UISource.clip = Bush;
            UISource.loop = false;
            UISource.outputAudioMixerGroup.audioMixer.SetFloat("EffectsVolume", 5);
            UISource.Play();
            //start ienumerator to stop it in like 0.5seconds
        }
    }


    public void UIReady()
    {
        if (Ready != null)
        {
            UISource.clip = Ready;
            UISource.loop = false;
            UISource.outputAudioMixerGroup.audioMixer.SetFloat("EffectsVolume", 0);
            UISource.Play();
        }
    }


    public void UIGo()
    {
        if (Go != null)
        {
            UISource.clip = Go;
            UISource.loop = false;
            UISource.outputAudioMixerGroup.audioMixer.SetFloat("EffectsVolume", 0);
            UISource.Play();
        }
    }


    public void BGMWinPurr()
    {
        BGMSource.clip = WinPurr;
        BGMSource.loop = false;
        BGMSource.Play();
    }


    public void BGMLosePurr()
    {
        BGMSource.clip = LosePurr;
        BGMSource.loop = false;
        BGMSource.Play();
    }


    public void FoodSucess()
	{
		FoodSource.clip = Success;
		FoodSource.loop = false;
		FoodSource.pitch = 2;
		FoodSource.Play ();
	}


	public void FoodPickUp()
	{
		FoodSource.clip = PickUp;
		FoodSource.loop = false;
		FoodSource.pitch = 1;
		FoodSource.Play ();
	}
}
