using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// this class will move the player, check collisions, and be hooked up to the main game loop to check for conversations
public class PlayerController : MonoBehaviour
{
    Rigidbody2D body;
    public BoxCollider2D box;
    public Sprite emptyHands;
    public Animator animator;
    SpriteRenderer sprite;
    GameObject dhost;
    DialogueManager dmanager;
    public List<GameObject> dialoguePoints;
    LockedDoor[] doors;
    public List<Item> items;
    Image heldItemBox;
    Text heldItemText;
    public string[] itemsNeeded;
    public EndingCutsceneTrigger endTrig;

    public int currentHeldItem;

    public bool frozen;
    
    public bool ending;
    private bool endingStarted;

    float horizontal;
    float vertical;
    static float moveLimiter = 0.7f;

    public float walkSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        dhost = GameObject.Find("DialogueManager");
        dmanager = dhost.GetComponent<DialogueManager>();
        dialoguePoints = new List<GameObject>();
        heldItemBox = GameObject.Find("Item Image Area").GetComponent<Image>();
        heldItemText = GameObject.Find("Item Name Area").GetComponent<Text>();

        doors = (LockedDoor[])GameObject.FindObjectsOfType(typeof(LockedDoor));

        frozen = false;

        items = new List<Item>();
        items.Add(new Item("default", emptyHands, ""));
        currentHeldItem = 0;
        CycleItem(0);


        foreach (Transform t in dhost.transform)
        {
            dialoguePoints.Add(t.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

        // do not let the character move if in text
        if (dmanager.animator.GetBool("IsOpen"))
        {
            vertical = 0f;
            horizontal = 0f;

            // set animators for dialogue
            animator.SetFloat("Speed", 0);

            // let them advance with space
            if (Input.GetButtonDown("Jump"))
            {
                dmanager.DisplayNextSentence();
            }
        }
        else if (!frozen)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            // set animators with floats
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);
            // set last horizontal and vertical if not idling for future use
            if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1
                || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
            {
                animator.SetFloat("LastHorizontal", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("LastVertical", Input.GetAxisRaw("Vertical"));
            }

            // Ik this is not speed but it will be positive when moving
            animator.SetFloat("Speed", Math.Abs(horizontal) + Math.Abs(vertical));

            // check for item cycling
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                CycleItem(1);
            }
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                currentHeldItem = 0;
                CycleItem(0);
            }

            // check for interactable
            if (Input.GetButtonDown("Jump"))
            {
                foreach (GameObject g in dialoguePoints)
                {
                    if (box.IsTouching(g.GetComponent<BoxCollider2D>()))
                    {
                        DialogueTrigger dtrig = g.GetComponent<DialogueTrigger>();
                        dtrig.TriggerDialogue(items[currentHeldItem].itemName);
                        break;
                    }
                }
            }
            // ending trigger
            if (ending && !endingStarted)
            {
                endingStarted = true;
                // check for certain items
                bool[] gottenItems = new bool[itemsNeeded.Count()];

                for (int i = 0; i < itemsNeeded.Count(); i++)
                {
                    foreach (Item g in items)
                    {
                        if (g.itemName == itemsNeeded[i])
                        {
                            gottenItems[i] = true;
                            break;
                        }
                    }
                }
                // if got them all, start good cutscene
                if (CheckIfAllBoolsTrue(gottenItems))
                {
                    endTrig.StartEnding(true);
                }
                else
                {
                    endTrig.StartEnding(false);
                }
                
            }
        }
    }

    private void FixedUpdate()
    {

        // horizontal move speed limiter
        if (horizontal != 0 && vertical != 0)
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        // update change in position
        body.velocity = new Vector2(horizontal * walkSpeed, vertical * walkSpeed);
    }
    public void CycleItem(int cycleBy)
    {
        // updates the current held item
        // built for 1, 0, and -1
        currentHeldItem += cycleBy;

        // cycling
        if (currentHeldItem == 0)
        {

        }
        else if (currentHeldItem >= items.Count)
        {
            currentHeldItem = 0;
        }
        else if (currentHeldItem < 0)
        {
            currentHeldItem = items.Count - 1;
        }
        // update sprite on top
        heldItemBox.sprite = items[currentHeldItem].sprite;
        heldItemText.text = items[currentHeldItem].displayName;
    }
    public void AddItem(Item item)
    {
        items.Add(item);
        currentHeldItem = 0;
        CycleItem(-1);
    }
    public void RemoveItem(string itemName)
    {
        Item itemToRemove = items.SingleOrDefault(i => i.itemName == itemName);
        if (itemToRemove != null)
        {
            items.Remove(itemToRemove);
        }
        currentHeldItem = 0;
        CycleItem(0);
    }
    public void ActivateDoors(string name)
    {
        foreach (LockedDoor d in doors)
        {
            if (d.lockedBehind == name)
            {
                d.Unlock();
            }
        }
    }
    public bool CheckIfAllBoolsTrue(bool[] array)
    {
        foreach (bool b in array)
        {
            if (!b)
            {
                return false;
            }
        }
        return true;
    }
}
