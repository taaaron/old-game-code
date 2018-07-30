using UnityEngine;
using System.Collections;

public class FloatingNumber : MonoBehaviour {

	public float fadeSpeed = 5f;  
	public float moveSpeed = 1f;

	private TextMesh text;

	// Use this for initialization
	void Start () {
		text = gameObject.GetComponent<TextMesh> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		FadeToClear ();

		gameObject.transform.Translate (new Vector3 (0, moveSpeed * Time.deltaTime, 0));
	
	}


	public void FadeToClear ()
	{	
		// Lerp the colour of the texture between itself and clear
		text.color = Color.Lerp(text.color, Color.clear, fadeSpeed * Time.deltaTime);
		
		// If the text is almost clear
		if(text.color.a <= 0.05f)
		{
			text.color = Color.clear;
			Destroy (gameObject);
		}
	}
}
