using UnityEngine;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;

public class OpenPortal : MonoBehaviour {

    public GameObject[] counters;
    public GameObject portal;

    public BoxCollider trigger;

    public Material lit;
    public Material unlit;

    public SFXManager SFXMan;

    public int countersLit = 0;
    public int countersNeeded = 5;

    private GameObject TeleportButton;

    private GameObject Player;

    // Use this for initialization
    void Awake () {
        portal.SetActive(false);
        trigger.enabled = false;
        SFXMan = GameObject.Find("SFXManager").GetComponent<SFXManager>();

        TeleportButton = GameObject.FindGameObjectWithTag("Next Level");
        Player = GameObject.FindGameObjectWithTag("Player");

        TeleportButton.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (countersLit == countersNeeded)
        {
            portal.SetActive(true);
            trigger.enabled = true;
        }
    }

    public void AddCounter()
    {
        if (countersLit < countersNeeded)
        { 
            counters[countersLit].GetComponent<Renderer>().material = lit;
            countersLit++;
        }

        if (countersLit == countersNeeded)
        {
            portal.SetActive(true);
            trigger.enabled = true;

            countersLit++;

            StartCoroutine("PortalCinematic");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TeleportButton.SetActive(true);
            SFXMan.PlayLevelCompleteSound();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
            TeleportButton.SetActive(false);
    }

    IEnumerator PortalCinematic()
    {
        Camera.main.GetComponent<ProCamera2D>().AddCameraTarget(gameObject.transform, 1, 1, 1);

        CameraTarget playerTarget = Camera.main.GetComponent<ProCamera2D>().GetCameraTarget(Player.transform);

        Camera.main.GetComponent<ProCamera2D>().AdjustCameraTargetInfluence(playerTarget, 0, 0, 1);

        Camera.main.GetComponent<ProCamera2DForwardFocus>().enabled = false;

        Player.GetComponent<PlayerPrototype>().enabled = false;

        SFXMan.PlayLevelCompleteSound();

        yield return new WaitForSeconds(3);

        Camera.main.GetComponent<ProCamera2DForwardFocus>().enabled = true;

        Camera.main.GetComponent<ProCamera2D>().AdjustCameraTargetInfluence(playerTarget, 1, 1, 1);

        Camera.main.GetComponent<ProCamera2D>().RemoveCameraTarget(gameObject.transform, 1);

        Player.GetComponent<PlayerPrototype>().enabled = true;
    }


}
