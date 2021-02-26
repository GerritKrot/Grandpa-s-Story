using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text dialogueText;
	public Image image;
	public Animator animator;

	private PlayerController player;

	private Queue<string> sentences;

	private int sentanceCount;

	public float waitSeconds;

	private float currentWaitSeconds;

	private bool ableToStartNextSentance;

	private Dialogue savedDlog;

	// Use this for initialization
	void Start () {
		sentences = new Queue<string>();
		ableToStartNextSentance = true;
		player = GameObject.Find("Player").GetComponent<PlayerController>();
	}

	public void StartDialogue (Dialogue dialogue)
   	{
		savedDlog = dialogue;

		animator.SetBool("IsOpen", true);

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		// set wait seconds
		waitSeconds = dialogue.waitSeconds;

		currentWaitSeconds = waitSeconds;

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
  		if (ableToStartNextSentance)
		{

			ableToStartNextSentance = false;
			if (sentences.Count == 0)
			{
				EndDialogue();
				return;
			}
			
			string sentence = sentences.Dequeue();
			
			StopAllCoroutines();
			// change header and image
			nameText.text = savedDlog.names[sentanceCount];
			image.sprite = savedDlog.images[sentanceCount];
			sentanceCount++;

			StartCoroutine(TypeSentence(sentence));
		}
		else
		{
			currentWaitSeconds = 0f;
		}
	}

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return new WaitForSeconds(currentWaitSeconds);
			yield return null;
		}
		currentWaitSeconds = waitSeconds;
		ableToStartNextSentance = true;
	}

	void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
		ableToStartNextSentance = true;
		sentanceCount = 0;
		// add and remove items
		if (savedDlog.getsItem)
		{
			player.AddItem(savedDlog.itemGiven);
			// open doors
			player.ActivateDoors(savedDlog.itemGiven.itemName);
		}
		if (savedDlog.removesItem)
		{
			player.RemoveItem(savedDlog.removedItem.itemName);
		}

		savedDlog = null;
	}

}
