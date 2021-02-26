using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleInputHandler : MonoBehaviour
{
    private bool showingAssets;
    public GameObject viewport;
    // Start is called before the first frame update
    void Start()
    {
        showingAssets = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!showingAssets)
        {
            if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("Grandpa's House");
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                showingAssets = true;
                viewport.transform.position = new Vector3(30, 0, -10f);
            }
        }
        else
        {
            // switch back to main screen
            if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Return))
            {
                showingAssets = false;
                viewport.transform.position = new Vector3(0, 0, -10f);
            }
        }
    }
}
