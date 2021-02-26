using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    BoxCollider2D box;
    public string lockedBehind;
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
    }

    public void Unlock()
    {
        box.isTrigger = true;
    }
    public void Lock()
    {
        box.isTrigger = false;
    }
}
