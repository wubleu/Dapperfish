using UnityEngine;
using System.Collections;

public class TestManager : MonoBehaviour {

	void Start () {
		new GameObject().AddComponent<Player>().init();
	}
}
