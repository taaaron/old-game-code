/*
This is the Controller that I wrote for the Cat character in the game Picky Birds. It is a mobile game that uses touch controls. The Cat itself takes no input and reacts to touches on the screen
around made on objects around it. Those objects then tell the Cat what state it should be in to react to the touch. This allowed me to easily control the animations and sounds played by the cat 
and create a clear seperation between the cat's actions. The cat also has differnt expressions depending on the situation. The cat has the ability to move towards an object, return to its original 
position, idle, twitch when it hears a sound, and reach to give something to another character. This script is meant to be used in Unity Game Engine.
*/

using UnityEngine;
using System.Collections;

public enum State
{
    Move,
    Return,
    Idle,
    Twitch,
    Reach,
}

public class CatController : MonoBehaviour
{

    public GameObject Selected; //Object for cat to move to
    public GameObject Held; //Food Cat is currently holding
    public GameObject Hand; //Object for cat's hand
    public GameObject Talk; //TalkBox
    public GameObject Expressions; //Object giving cat his texture
    public GameObject WalkLines; //Lines to appear to show movement

    public SoundEffects Sounds;

    public Material[] CatMaterials; //0 is default, 1 is happy, 2 is mistake

    public TextMesh TalkText; //Text in TalkBox

    public int holding = 0; //how many objects is cat holding

    public float speed = 10;
    public float timeAllowed = 1; //time added to talkTime every click

    public bool reach = false;
    public bool wait = false;

    public State state;

    //Private Variables
    private Vector3 originalPos;
    private Vector3 originalTalkPos;
    private float talkTime = 0; //If higher than 0, talkBox is allowed to be active


    // Use this for initialization
    void Start()
    {
        Sounds = GameObject.FindGameObjectWithTag("SoundEffects").GetComponent<SoundEffects>();
        originalPos = gameObject.transform.position;
        originalTalkPos = Talk.transform.localPosition;
        state = State.Idle;
    }


    // Update is called once per frame
    void Update()
    {

        if (talkTime > 0)
        {
            talkTime -= Time.deltaTime;
        }

        if (Talk.activeSelf && talkTime <= 0)
        {
            talkTime = 0;
            Talk.SetActive(false);
        }

        //set to correct look direction when moving towards selected
        if (Selected != null && state == State.Move)
        {
            float scale = gameObject.transform.localScale.x;

            if (scale < 0 && Selected.transform.position.x < gameObject.transform.position.x)
            {
                gameObject.transform.localScale = new Vector3(scale * -1, 60, 1);
            }
            else if (scale > 0 && Selected.transform.position.x >= gameObject.transform.position.x)
            {
                gameObject.transform.localScale = new Vector3(scale * -1, 60, 1);
            }
        }

        //set text bubble to correct direction
        if (gameObject.transform.localScale.x > 0 && Talk.transform.localScale.x < 0)
        {
            Talk.transform.localScale = new Vector3(-Talk.transform.localScale.x, Talk.transform.localScale.y, Talk.transform.localScale.z);

            if (Talk.transform.localPosition == originalTalkPos)
            {
                Talk.transform.localPosition = new Vector3(-originalTalkPos.x, Talk.transform.localPosition.y, Talk.transform.localPosition.z);
            }
        }
        else if (gameObject.transform.localScale.x < 0 && Talk.transform.localScale.x > 0)
        {
            Talk.transform.localScale = new Vector3(-Talk.transform.localScale.x, Talk.transform.localScale.y, Talk.transform.localScale.z);

            if (Talk.transform.localPosition != originalTalkPos)
            {
                Talk.transform.localPosition = originalTalkPos;
            }
        }

        switch (state)
        {
            case State.Idle:
                Sounds.CatStop();
                OriginalTexture();
                WalkLines.SetActive(false);
                gameObject.transform.localScale = new Vector3(-60, 60, 1);
                break;

            case State.Move:
                if (!Sounds.CatSource.isPlaying)
                {
                    Sounds.CatWalking();
                }

                WalkLines.SetActive(true);

                gameObject.GetComponent<Animator>().SetBool("Move", true);

                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, Selected.transform.position, speed * Time.deltaTime);

                if (gameObject.GetComponent<Collider>().bounds.Contains(Selected.transform.position) && Selected.tag == "Food")
                {
                    Selected.GetComponent<Food>().held = true;
                    GrabFood();
                }

                break;

            case State.Return:
                float magnitude = (originalPos - gameObject.transform.position).magnitude;

                if (!Sounds.CatSource.isPlaying)
                {
                    Sounds.CatWalking();
                }

                //set correct look direction when returning to original position
                if (originalPos.x < gameObject.transform.position.x)
                    gameObject.transform.localScale = new Vector3(60, 60, 1);
                else
                    gameObject.transform.localScale = new Vector3(-60, 60, 1);

                WalkLines.SetActive(true);

                if (magnitude > 0.5)
                {
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, originalPos, speed * Time.deltaTime);
                }
                else
                {
                    gameObject.transform.position = originalPos;
                    gameObject.GetComponent<Animator>().SetBool("Move", false);
                    Selected = null;
                    state = State.Idle;
                }
                break;

            case State.Twitch:
                Sounds.CatStop();
                OriginalTexture();
                WalkLines.SetActive(false);
                gameObject.GetComponent<Animator>().SetTrigger("Twitch");
                gameObject.GetComponent<Animator>().SetBool("Move", false);
                if (gameObject.transform.position != originalPos)
                {
                    if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Twitch"))
                        wait = true;
                    else
                    {
                        if (wait && gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                        {
                            wait = false;
                            state = State.Return;
                        }
                    }
                }
                else
                    state = State.Idle;
                break;

            case State.Reach:
                Sounds.CatStop();
                WalkLines.SetActive(false);
                gameObject.GetComponent<Animator>().SetTrigger("Reach");
                if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Reach"))
                {
                    reach = true;
                }
                else if (reach && gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    Destroy(Held);
                    reach = false;
                    state = State.Return;
                }
                break;
        }

    }


    public void GrabFood()
    {
        Sounds.FoodPickUp();

        holding++;
        Selected.transform.position = Hand.transform.position - new Vector3(0, 0, 0.1f);
        Selected.transform.parent = gameObject.transform;
        Held = Selected;
        Held.GetComponent<Food>().oneClick = false;
        state = State.Return;

        Tutorial.SendTutorialAction(Tutorial.TutorialInputs.PickUpFood);
    }


    public void AddTime(string text)
    {
        Talk.SetActive(true);
        TalkText.text = text;
        talkTime = timeAllowed;
    }


    //Set texture to default
    public void OriginalTexture()
    {
        Expressions.GetComponent<Renderer>().material = CatMaterials[0];
    }


    //Set texture for when got a question wrong
    public void MistakeTexture()
    {
        Expressions.GetComponent<Renderer>().material = CatMaterials[2];
    }


    //Set texture for when got a question right
    public void CorrectTexture()
    {
        Expressions.GetComponent<Renderer>().material = CatMaterials[1];
    }

}
