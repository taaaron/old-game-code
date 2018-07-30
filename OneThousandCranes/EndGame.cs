using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

    public GameObject panda;

    public GameObject lastCrane;

    public GameObject endButton;

    public ScreenFading ScreenFade;

    public bool move = true;

    public bool finish = false;

	// Use this for initialization
	void Start () {
        panda.GetComponent<Animator>().SetBool("walkBool", true);
	
	}
	
	// Update is called once per frame
	void Update () {

        //panda walks until hits trigger.
        if(move)
        {
            panda.transform.Translate(Vector3.left * 2 * Time.deltaTime);
        }

        //Stops moving and drops last crane into pile
        if(!move && !finish)
        {
            lastCrane.transform.Translate(Vector3.down * Time.deltaTime);
        }

        if(finish)
        {
            if(ScreenFade.GetAlpha() > 0.95f)
            {
                ScreenFade.black.color = Color.black;

                Application.LoadLevel("Main Menu");
            }
        }
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Panda")
        {
            move = false;

            panda.GetComponent<Animator>().SetBool("walkBool", false);

            StartCoroutine("WaitThenButton");
        }
    }

    public void End()
    {
        ScreenFade.SetSceneEnd();

        endButton.SetActive(false);
    }

    IEnumerator WaitThenButton()
    {
        yield return new WaitForSeconds(0.6f);

        finish = true;

        endButton.SetActive(true);
    }


}
