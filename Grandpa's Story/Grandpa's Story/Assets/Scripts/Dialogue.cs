using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

	public string triggeredBy = "default";

	public float waitSeconds = 0.05f;

	[TextArea(3, 5)]
	public string[] sentences;

	public string[] names;

	public Sprite[] images;

	public bool getsItem;

	public bool removesItem;

	public Item itemGiven;

	public Item removedItem;

	public bool hasBeenSaid;

	public Dialogue(string triggeredBy, float waitSeconds, string[] sentences, string[] names, Sprite[] images, bool getsItem, bool removesItem, Item itemGiven, Item removedItem)
	{
		this.triggeredBy = triggeredBy;
		this.waitSeconds = waitSeconds;
		this.sentences = sentences;
		this.names = names;
		this.images = images;
		this.getsItem = getsItem;
		this.removesItem = removesItem;
		this.itemGiven = itemGiven;
		this.removedItem = removedItem;
		hasBeenSaid = false;
	}
}
