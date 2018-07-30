using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

    public bool loading = false;

    public bool fading = false;

    public Slider LoadBar;

    public ScreenFading FadeScript;

    private int levelToLoad;

    private AsyncOperation async = null;

    // Use this for initialization
    void Start()
    {
        levelToLoad = PlayerPrefs.GetInt("Level", 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!loading)
            StartCoroutine("LoadInBg");
        if (loading)
        {
            if (async.progress < 0.9f)
            {
                int rounded;
                rounded = (int)(100 * async.progress);
                LoadBar.value = async.progress;
            }
        }
        if (async.progress == 0.9f)
        {
            LoadBar.value = 1;

            async.allowSceneActivation = true;
        }

    }

    IEnumerator LoadInBg()
    {
        loading = true;

        switch(levelToLoad)
        {
            case 0:
                async = Application.LoadLevelAsync("Tutorial");
                break;
            case 1:
                async = Application.LoadLevelAsync("Forest");
                break;
            case 2:
                async = Application.LoadLevelAsync("Mountain");
                break;
            case 3:
                async = Application.LoadLevelAsync("Air");
                break;
        }

        async.allowSceneActivation = false;

        yield return new WaitForEndOfFrame();

        yield return async;
    }

    IEnumerator WaitThenLoad()
    {
        fading = true;

        yield return new WaitForSeconds(10);

        FadeScript.SetSceneEnd();

        async.allowSceneActivation = true;
    }
}
