using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    BoxCollider2D box;
    PlayerController player;
    GameObject viewport;
    Image fadeScreen;
    public Vector2 teleportPosition;
    public Vector2 teleportCamera;
    private bool readyToTeleport;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        viewport = GameObject.Find("Main Camera");
        fadeScreen = GameObject.Find("Fadeout").GetComponent<Image>();
        box = GetComponent<BoxCollider2D>();
        readyToTeleport = true;
        fadeScreen.CrossFadeAlpha(0f, 0f, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (box.IsTouching(player.box) && readyToTeleport)
        {
            prepTele();
        }

    }

    public void prepTele() {
        player.frozen = true;
        fadeScreen.CrossFadeAlpha(1f, 0.5f, false);
        StartCoroutine("Teleport");
        readyToTeleport = false;
    }
    IEnumerator Teleport()
    {
        yield return new WaitForSeconds(0.5f);
        player.transform.position = new Vector2(teleportPosition.x, teleportPosition.y);
        viewport.transform.position = new Vector3(teleportCamera.x, teleportCamera.y, -10f);
        StartCoroutine("FadeBack");
        yield return null;
    }
    IEnumerator FadeBack()
    {
        fadeScreen.CrossFadeAlpha(0f, 0.5f, false);
        readyToTeleport = true;
        player.frozen = false;
        yield return null;
    }
}
