using UnityEngine;
using System.Collections;

public class UICat : MonoBehaviour {

    public Material[] Materials; //0 is win and 1 is lose. Defaults is regular

    public GameObject Expressions;

    public bool move = false;

    public bool stopping = false;

    public int jump = 0;

    public Vector3 downPos;

    public Vector3 upPos;



    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (move)
        {
            if(!stopping)
                StartCoroutine("StopWalking");

            gameObject.GetComponent<Animator>().SetBool("Move", true);

            gameObject.transform.Translate(Vector3.right * 4 * Time.deltaTime);
        }

        //go up if 1
        if(jump == 1)
        {
            gameObject.transform.Translate(Vector3.up * Time.deltaTime);

            if(Vector3.Distance(gameObject.transform.position, upPos) < 0.1f)
            {
                jump = 2;
            }
        }

        //go down if 2
        if(jump == 2)
        {
            gameObject.transform.Translate(Vector3.down * Time.deltaTime);

            if (Vector3.Distance(gameObject.transform.position, downPos) < 0.1f)
            {
                jump = 1;
            }
        }

    }

    public void SetMove(bool other)
    {
        move = other;
    }

    public void SetLose()
    {
        Expressions.GetComponent<Renderer>().material = Materials[1];

        gameObject.GetComponent<Animator>().speed = 0.5f;
    }
    public void SetWin()
    {
        Expressions.GetComponent<Renderer>().material = Materials[0];

        gameObject.GetComponent<Animator>().speed = 5;

        jump = 1;
    }

    IEnumerator StopWalking()
    {
        stopping = true;

        yield return new WaitForSeconds(1.3f);

        move = false;

        gameObject.GetComponent<Animator>().SetBool("Move", false);

        downPos = gameObject.transform.position;
        upPos = gameObject.transform.position + new Vector3(0,0.5f,0);
    }
}
