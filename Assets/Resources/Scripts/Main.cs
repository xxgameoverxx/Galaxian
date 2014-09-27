using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

	public void NewGame()
    {
        Application.LoadLevel("TestScene"); ;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
