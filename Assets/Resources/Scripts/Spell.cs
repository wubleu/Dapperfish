using UnityEngine;
using System.Collections;

public class Spell : MonoBehaviour {

	// PARAMETERS


	float lifetime;


	// Use this for initialization
	protected void init (Vector3 pos, float size, float lifeLength, int spriteIndex, string spriteSheet, string animatorName, string name) {
		lifetime = lifeLength;
		Sprite[] cSprites = Resources.LoadAll<Sprite> ("Textures/" + spriteSheet);
		gameObject.AddComponent<SpriteRenderer>();
		gameObject.AddComponent<Animator>();
		gameObject.GetComponent<SpriteRenderer> ().sprite = cSprites [spriteIndex];
		gameObject.transform.position = pos;
		gameObject.transform.localScale = new Vector3(size, size, size);
		gameObject.transform.localEulerAngles = new Vector3(90, 0, 0);
		Animator anim = gameObject.GetComponent<Animator> ();
		anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Animations/" + animatorName);
		gameObject.AddComponent<SphereCollider> ().isTrigger = true;
		gameObject.name = name;
	}
	
	// Update is called once per frame
	protected void Update () {
		if ((lifetime -= Time.deltaTime) < 0) {
			Destroy (gameObject);
		}
	}
}
