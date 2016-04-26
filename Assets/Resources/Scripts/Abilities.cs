using UnityEngine;
using System.Collections;

public static class Abilities {

	private static GameObject AOE(float radius, Color color, Vector3 pos) {
		GameObject spell = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		spell.transform.position = pos;
		spell.transform.localScale = new Vector3(radius, radius, radius);

		spell.GetComponent<SphereCollider>().isTrigger = true;
		spell.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Textures/Sphere");
		spell.GetComponent<MeshRenderer> ().material.color = color;

		return spell;
	}

	public static void Blight(Vector3 pos) {
		//GameObject spell = AOE(3, new Color(0, 1, 0, .4f), pos);
		Sprite[] cSprites = Resources.LoadAll<Sprite> ("Textures/Spell Effects Sprite Sheet");
		GameObject spell = new GameObject();
		spell.AddComponent<SpriteRenderer>();
		spell.AddComponent<Animator>();
		spell.GetComponent<SpriteRenderer> ().sprite = cSprites [0];
		spell.transform.position = pos;
		spell.transform.localScale = new Vector3(.75f, .75f, .75f);
		spell.transform.localEulerAngles = new Vector3(90, 0, 0);
		Animator anim = spell.GetComponent<Animator> ();
		anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Animations/Blight Controller");
		spell.AddComponent<SphereCollider>();
		spell.GetComponent<SphereCollider>().isTrigger = true;
		spell.name = "Blight";
		spell.AddComponent<SpellEffect>().init(.4f);
	}

	public static void Root(Vector3 pos) {
		Sprite[] cSprites = Resources.LoadAll<Sprite> ("Textures/Spell Effects Sprite Sheet");
		GameObject spell = new GameObject();
		spell.AddComponent<SpriteRenderer>();
		spell.AddComponent<Animator>();
		spell.GetComponent<SpriteRenderer> ().sprite = cSprites [8];
		spell.transform.position = pos;
		spell.transform.localScale = new Vector3(.8f, .8f, .8f);
		spell.transform.localEulerAngles = new Vector3(90, 0, 0);
		Animator anim = spell.GetComponent<Animator> ();
		anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Animations/Root Controller");
		spell.AddComponent<SphereCollider>();
		spell.name = "Root";
		spell.AddComponent<SpellEffect>().init(2f);
	}

	public static void Damage(Vector3 pos, float angle) {
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Sprite[] cSprites = Resources.LoadAll<Sprite> ("Textures/Spell Effects Sprite Sheet");
		GameObject spell = new GameObject();
		spell.AddComponent<SpriteRenderer>();
		spell.AddComponent<Animator>();
		spell.GetComponent<SpriteRenderer> ().sprite = cSprites [10];
		spell.transform.position = pos;
		spell.transform.position = new Vector3 (spell.transform.position.x, .5f, spell.transform.position.z);
		spell.transform.localScale = new Vector3(.5f, .5f, .5f);
		spell.transform.localEulerAngles = new Vector3 (90, GameObject.Find("Necromancer").transform.localEulerAngles.y, 0);
		Animator anim = spell.GetComponent<Animator> ();
		anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Animations/Wave Controller");
		spell.AddComponent<SphereCollider>();
		spell.transform.LookAt(mouse);
		spell.GetComponent<SphereCollider>().isTrigger = true;
		spell.name = "Damage";
		spell.AddComponent<SpellEffect>().init(2, 5, new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle))); 
	}

	public static void Blink(Transform me, float angle) {
		me.Translate(4 * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
	}

	public static bool Bullet(Vector3 pos, float angle) {
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//GameObject spell = AOE(.3f, Color.black, pos + 0.5f * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
		Sprite[] cSprites = Resources.LoadAll<Sprite> ("Textures/Spell Effects Sprite Sheet");
		GameObject spell = new GameObject();
		spell.AddComponent<SpriteRenderer>();
		spell.AddComponent<Animator>();
		spell.GetComponent<SpriteRenderer> ().sprite = cSprites [13];
		spell.transform.position = pos;
		spell.transform.position = new Vector3 (spell.transform.position.x, .5f, spell.transform.position.z);
		spell.transform.localScale = new Vector3(.4f, .4f, .4f);
		spell.transform.localEulerAngles = new Vector3 (90, GameObject.Find("Necromancer").transform.localEulerAngles.y, 0);
		Animator anim = spell.GetComponent<Animator> ();
		anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Animations/Projectile Controller");
		spell.AddComponent<SphereCollider>();
		spell.transform.LookAt(mouse);
		spell.GetComponent<SphereCollider>().isTrigger = true;
		spell.name = "Bullet";
		spell.AddComponent<SpellEffect>().init(1, 10, new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
		return true;
	}

	public static bool Arrow(Vector3 pos, float angle, bool enemy) {
		GameObject spell = AOE(.3f, Color.red, pos + 0.5f * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
		/*Sprite[] cSprites = Resources.LoadAll<Sprite> ("Textures/Skeleton Archer Sprite Sheet");
		GameObject spell = new GameObject();
		spell.AddComponent<SpriteRenderer>();
		if (enemy) {
			spell.GetComponent<SpriteRenderer> ().sprite = cSprites [6];
		} else {spell.GetComponent<SpriteRenderer> ().sprite = cSprites [7];
		}
		spell.transform.position = pos;
		spell.transform.position = new Vector3 (spell.transform.position.x, .5f, spell.transform.position.z);
		spell.transform.localScale = new Vector3(1, 1, 1);
		spell.AddComponent<BoxCollider>();*/
		spell.name = "Arrow";
		spell.AddComponent<SpellEffect>().init(1, 10, new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)), enemy);
		return true;
	}

	public static Vector3 NormalizeVector(Vector3 vector, params bool[] isZ) {
		Vector3 direction;
		if (isZ[0] == null) {
			direction = new Vector3 (vector.x, vector.y);
		} else {
			direction = new Vector3 (vector.x, vector.y);
		}
		float directionMagnitude = Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
		direction = new Vector3(direction.x / directionMagnitude, direction.y / directionMagnitude);
		return direction;
	}
}
