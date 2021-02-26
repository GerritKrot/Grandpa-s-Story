using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCutsceneTrigger : MonoBehaviour
{
    public string debugGoodEnding;
    public string debugBadEnding;
    public DialogueManager dManager;
    public PlayerController player;
    public TeleportTrigger goodTele;
    public TeleportTrigger badTele;
    public Dialogue badDialogue;
    public Dialogue goodDialogue;

    private float time;
    public float timeUntilCutsceneDone;
    private bool endingDialoguePlayed;
    private bool endStart;
    private bool isGoodEnding;


    // Start is called before the first frame update
    void Start()
    {
        endStart = false;
        timeUntilCutsceneDone = 0.6f;
    }

    // Update is called once per frame
    void Update() { 
    
        if (!dManager.animator.GetBool("IsOpen") && endStart)
        {
            time += Time.deltaTime;
        }

        if (time > timeUntilCutsceneDone)
        {
            // load different unity scene based on ending
            if (isGoodEnding)
            {
                // good ending scene
                SceneManager.LoadScene("GoodEnding");

            }
            else
            {
                // bad ending scene
                SceneManager.LoadScene("BadEnding");
            }
        }


        // debug shortcut
        /*
        if (Input.GetKeyDown(debugGoodEnding) )
        {
            StartEnding(true);
        }
        if (Input.GetKeyDown(debugBadEnding))
        {
            StartEnding(false);
        }
        */
    }
    public void StartEnding(bool isGoodEnding)
    {
        this.isGoodEnding = isGoodEnding;
        player.frozen = true;
        endStart = true;
        if (isGoodEnding)
        {
            // do ui transition and teleport
            goodTele.prepTele();
        }
        else
        {
            badTele.prepTele();
        }
        StartCoroutine("EndingDialoguePlayer");


    }
    IEnumerator EndingDialoguePlayer()
    {
        yield return new WaitForSeconds(0.5f);
        if (isGoodEnding)
        {
            dManager.StartDialogue(goodDialogue);
        }
        else
        {
            dManager.StartDialogue(badDialogue);
        }

        yield return null;
    }
}
