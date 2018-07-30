using UnityEngine;
using System.Collections;

public class CheckpointShrine : MonoBehaviour {

    public Light checkpointLight;
    public GameObject respawnVector;
    private bool used;

    private SFXManager SFXMan;

    // Use this for initialization
    void Awake () {
        used = false;
        checkpointLight.gameObject.SetActive(false);

        SFXMan = GameObject.Find("SFXManager").GetComponent<SFXManager>();

        respawnVector.transform.position = new Vector3(respawnVector.transform.position.x, respawnVector.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !used)
        {
            other.GetComponent<PlayerPrototype>().respawnLocation = respawnVector.transform.position;
            used = true;
            checkpointLight.gameObject.SetActive(true);
            SFXMan.PlayCheckpointSound();
        }
    }
}
