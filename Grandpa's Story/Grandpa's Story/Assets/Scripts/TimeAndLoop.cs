using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAndLoop : MonoBehaviour
{
    public float MaxTime;
    public float curTimeLeft;
    private bool firstDialogue = true;
    private bool teleReady = true;
    private bool secondDialogue = true;
    public DialogueManager dManager;
    public Sprite grandpaSprite;
    public Sprite mCMadSprite;
    public TeleportTrigger tele;


    // Start is called before the first frame update
    void Start()
    {
        curTimeLeft = MaxTime;
        firstDialogue = true;
        teleReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dManager.animator.GetBool("IsOpen"))
        {
            curTimeLeft -= Time.deltaTime;
        }

        if (curTimeLeft <= 0.7)
        {
            if (firstDialogue)
            {
                string[] Sentence = { "I appreciate the effort, but I really don't like these cupcakes...", 
                    "It doesn't hold a candle to my wife's beautiful three layer chocolate cake!",
                    "It was so wonderful, but I just can't seem to remember the last time I had it.",
                    "Now if I could only find the cookbook. Remind me to ask her when I see her again."};
                string[] Name = { "Grandpa", "Grandpa", "Grandpa", "Grandpa"};
                Sprite[] DisPicture = { grandpaSprite, grandpaSprite, grandpaSprite, grandpaSprite };
                dManager.StartDialogue(new Dialogue(null, 0.05f, Sentence, Name, DisPicture, false, false, null, null));
                // change do dialogue to false so it doesnt call this every frame
                firstDialogue = false;
            }
            else if (!dManager.animator.GetBool("IsOpen") && teleReady)
            {
                tele.prepTele();
                teleReady = false;
            }
        }
        if (curTimeLeft <= 0 && secondDialogue)
        {
            string[] Sentence = { "God damn it!!!",
                    "I've thrown him a birthday party every day for eight days straight now and he never remembers!",
                    "This time, I'll bake that three layer cake he talked about last night. Hopefully he'll remember that.",
                    "I should also check around for stuff to decorate the party with before I bake the cake.",
                    "I think FIVE (5) special items will definitely be enough!"};
            string[] Name = { "You", "You", "You", "You", "You" };
            Sprite[] DisPicture = { mCMadSprite, mCMadSprite, mCMadSprite, mCMadSprite, mCMadSprite};
            secondDialogue = false;
            dManager.StartDialogue(new Dialogue(null, 0.02f, Sentence, Name, DisPicture, false, false, null, null));
            Destroy(gameObject);
        }
            

    }
}
