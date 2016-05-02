using UnityEngine;
using System.Collections;

public class Link{

	public Vector3 begin;
	public Vector3 end;
	public KeyCode into;
	public KeyCode outof;
	public string name;
	public bool unlocked;
	public float time;

	public Link(Vector3 b, Vector3 e, string k1, string n, string o, string t) {
		begin = b;
		end = e;
		if (k1 == "A") {
			into = KeyCode.A;
			outof = KeyCode.D;
		} else if (k1 == "S") {
			into = KeyCode.S;
			outof = KeyCode.W;
		} else if (k1 == "D") {
			into = KeyCode.D;
			outof = KeyCode.A;
		} else if (k1 == "W") {
			into = KeyCode.W;
			outof = KeyCode.S;
		}
		name = n;
		if(o=="f"){
			unlocked = false;
		} else{
			unlocked = true;
		}
		time = float.Parse (t);
	}

}
