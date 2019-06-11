using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScript : Interactable
{
    private void Start()
    {
        Screen.fullScreen = true;
    }
    protected override void Click(Vector3 clickposition)
    {
        //Debug.Log("click");
        SceneManager.LoadScene("SampleScene");
    }
}
