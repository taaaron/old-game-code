using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AsyncLoad : MonoBehaviour {

    public bool loading = false;
    public bool showing = false; //bool is true when time to reveal next tonelette, false when waiting

    public Text Percentage;

    public Slider LoadBar;

    public GameObject[] Tonelettes;

    public int toShow = 0;

    private AsyncOperation async = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!loading)
            StartCoroutine("LoadInBg");
        if(loading)
        {
            if (async.progress < 0.9f)
            {
                int rounded;
                rounded = (int)(100 * async.progress);
                Percentage.text = rounded.ToString() + "%";
                LoadBar.value = async.progress;
            }
        }
        if (async.progress == 0.9f)
        {
            Percentage.text = "100%";
            LoadBar.value = 1;
            //StartCoroutine("WaitThenLoad");
            async.allowSceneActivation = true;
        }

        if(toShow < 3 && !showing)
        {
            StartCoroutine("ShowTonelette");
        }

    }

    IEnumerator LoadInBg()
    {
        loading = true;

        //Check if should load tutorial or regular level
        if (PlayerPrefs.GetInt("HasPlayedTutorial") != 1)
        {
            PlayerPrefs.SetInt("HasPlayedTutorial", 1);
            async = Application.LoadLevelAsync("Tutorial");
        }
        else
        {
            async = Application.LoadLevelAsync("Test");
        }

        async.allowSceneActivation = false;

        yield return new WaitForEndOfFrame();

        yield return async;
    }

    IEnumerator WaitThenLoad()
    {
        yield return new WaitForSeconds(1);

        async.allowSceneActivation = true;
    }

    IEnumerator ShowTonelette()
    {
        showing = true;

        Tonelettes[toShow].SetActive(true);

        toShow++;

        yield return new WaitForSeconds(0.1f);

        if(toShow == 3)
        {
            toShow = 0;

            for(int i = 0; i < Tonelettes.Length; i++)
            {
                Tonelettes[i].SetActive(false);
            }
        }

        showing = false;
    }
}
