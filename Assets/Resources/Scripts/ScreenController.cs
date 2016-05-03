using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenController : MonoBehaviour {

	public Button startButton;
	public Button helpButton;
	public Button quitGame;
	public GameObject help;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void StartGame(){
		SceneManager.LoadScene ("Level 1");
	}

	public void Help(){
		help.SetActive (true);
		startButton.gameObject.SetActive (false);
		helpButton.gameObject.SetActive (false);
		quitGame.gameObject.SetActive (false);
	}

	public void Back(){
		startButton.gameObject.SetActive (true);
		helpButton.gameObject.SetActive (true);
		quitGame.gameObject.SetActive (true);
		help.SetActive (false);
	}

	public void Quit(){
		Application.Quit ();
	}
}
