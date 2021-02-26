using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue[] dialogues;
	DialogueManager dmanager;
	PlayerController player;
	public Sprite warningSprite;
	bool foundDialogue;
	bool duplicateItem;
	public bool oneTimeTrigger;
	public bool isEndingTrigger;

	private void Start()
	{
		dmanager = FindObjectOfType<DialogueManager>();
		player = GameObject.Find("Player").GetComponent<PlayerController>();
	}

	public void TriggerDialogue(string itemShown)
	{
		foundDialogue = false;
		duplicateItem = false;
		// dmanager.StartDialogue(dialogue[0]);
		// check player's held item and start conversation based on name
		foreach (Dialogue d in dialogues)
		{
			if (d.triggeredBy == itemShown)
			{
				// check if player already has item from dialogue
				if (d.getsItem)
				{
					foreach (Item p in player.items)
					{
						if (p.itemName == d.itemGiven.itemName || d.hasBeenSaid) 
						{
							duplicateItem = true;
							foundDialogue = true;
							string[] warningSentance = {"You already have the item from this... Go use it somewhere else!"};
							string[] warningName = { "Reminder:" };
							Sprite[] warningPictures = { warningSprite };
							dmanager.StartDialogue(new Dialogue(null, 0f, warningSentance, warningName, warningPictures, false, false, null, null));
							break;
						}
					}
				}
				if (!duplicateItem)
				{
					foundDialogue = true;
					d.hasBeenSaid = true;
					d.waitSeconds = 0.015f;
					dmanager.StartDialogue(d);
					// when the method is done, check onetimetrigger for self desctruction
					if (oneTimeTrigger)
					{
						player.dialoguePoints.Remove(gameObject);
						Destroy(gameObject);
					}
					if (isEndingTrigger && d.triggeredBy != "default")
					{
						StartCoroutine("DelayedPlayerEnd");
					}
					break;
				}
			}
		}
		if (!foundDialogue)
		{
			string[] warningSentance = { "Your hands are full! Try cycling items with Left Shift or clearing your hands with Left Control!" };
			string[] warningName = { "Oops!" };
			Sprite[] warningPictures = { warningSprite };
			dmanager.StartDialogue(new Dialogue(null, 0f, warningSentance, warningName, warningPictures, false, false, null, null));
		}
	}
	IEnumerator DelayedPlayerEnd()
	{
		yield return new WaitForSeconds(0.5f);
		player.ending = true;
		yield return null;
	}
}
