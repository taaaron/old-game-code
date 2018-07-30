using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    public GameObject[] Points;
    public int movingTowards = 0;

    public float speed = 1;

    public float delayTime = 0;

    public bool wait = false;

    private Vector3[] Points2;

    private float initialDistance;

	// Use this for initialization
	void Start () {
        Points2 = new Vector3[Points.Length];
        for(int i = 0; i < Points.Length; i++)
        {
            Points2[i] = Points[i].transform.position;
            Destroy(Points[i]);
        }

        initialDistance = Vector3.Distance(transform.position, Points2[movingTowards]);

    }
	
	// Update is called once per frame
	void Update () {
        if(Vector3.Distance(transform.position,Points2[movingTowards]) < 0.5f)
        {
            if(movingTowards + 1 == Points2.Length)
            {
                movingTowards = 0;
                initialDistance = Vector3.Distance(transform.position, Points2[movingTowards]);
            }
            else
            {
                movingTowards++;
                initialDistance = Vector3.Distance(transform.position, Points2[movingTowards]);
            }

            StartCoroutine("DelayMovement");
        }

        Vector3 direction;

        direction = Points2[movingTowards] - transform.position;
        direction.Normalize();

        float speedVariation = (Vector3.Distance(transform.position, Points2[movingTowards])/initialDistance);
        if (speedVariation < 0.2f)
            speedVariation = 0.2f;
        //Debug.Log(gameObject.name + ": " + speedVariation + " initial: " + initialDistance + " moving towards: " + movingTowards);

        if (!wait)
            transform.Translate(direction * speed * Time.deltaTime * speedVariation);
	
	}

    IEnumerator DelayMovement()
    {
        wait = true;

        yield return new WaitForSeconds(delayTime);

        wait = false;
    }
}
