using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenController : MonoBehaviour {

	public Button startButton;
	public Button helpButton;
	public Button quitGame;
	public Button endless;
	public Button levelselect;
	public Button level1;
	public Button level2;
	public Button level3;
	public GameObject help;
	public GameObject levelpanel;

	public void StartGame(){
		SceneManager.LoadScene ("Level 1");
	}

	public void Help(){
		help.SetActive (true);
		startButton.gameObject.SetActive (false);
		helpButton.gameObject.SetActive (false);
		quitGame.gameObject.SetActive (false);
		endless.gameObject.SetActive (false);
		levelselect.gameObject.SetActive (false);
	}

	public void Back(){
		startButton.gameObject.SetActive (true);
		helpButton.gameObject.SetActive (true);
		quitGame.gameObject.SetActive (true);
		endless.gameObject.SetActive (true);
		levelselect.gameObject.SetActive (true);
		help.SetActive (false);
	}

	public void Quit(){
		Application.Quit ();
	}

	public void EndlessMode(){
		SceneManager.LoadScene ("Level 4");
	}

	public void LevelSelect(){
		levelpanel.SetActive (true);
		startButton.gameObject.SetActive (false);
		helpButton.gameObject.SetActive (false);
		quitGame.gameObject.SetActive (false);
		endless.gameObject.SetActive (false);
		levelselect.gameObject.SetActive (false);
	}

	public void Back2(){
		startButton.gameObject.SetActive (true);
		helpButton.gameObject.SetActive (true);
		quitGame.gameObject.SetActive (true);
		endless.gameObject.SetActive (true);
		levelselect.gameObject.SetActive (true);
		levelpanel.SetActive (false);
	}

	public void Level1(){
		SceneManager.LoadScene ("Level 1");
	}

	public void Level2(){
		SceneManager.LoadScene ("Level 2");
	}

	public void Level3(){
		SceneManager.LoadScene ("Level 3");
	}
}
