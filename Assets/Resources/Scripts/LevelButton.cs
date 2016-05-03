using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour {

	void OnMouseButtonDown() {
		switch (name) {
			case "Square1":
				SceneManager.LoadScene(1);
				break;
			case "Square2":
				SceneManager.LoadScene(2);
				break;
			case "Square3":
				SceneManager.LoadScene(3);
				break;
		}
	}
}
