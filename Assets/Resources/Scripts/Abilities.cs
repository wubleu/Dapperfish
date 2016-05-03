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

	public static int Summon(float x, float y, float z) {
		GameObject summon = new GameObject ();
		return summon.AddComponent<Summon> ().init(new Vector3(x, y, z));
	}

	private static GameObject makeSphere(Vector3 pos, int spr, float scale, float rad, string animation) {
		GameObject spell = new GameObject();
		spell.transform.position = pos;

		SphereCollider coll = spell.AddComponent<SphereCollider>();
		coll.isTrigger = true;
		coll.radius = rad;

		Sprite[] cSprites = Resources.LoadAll<Sprite>("Textures/Spell Effects Sprite Sheet");
		GameObject model = new GameObject();
		model.transform.parent = spell.transform;
		model.transform.localPosition = new Vector3(0, 1, 0);
		model.transform.localEulerAngles = new Vector3(90, 0, 0);
		model.transform.localScale = new Vector3(scale, scale, scale);
		model.AddComponent<SpriteRenderer>().sprite = cSprites[spr];

		model.AddComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/" + animation);

		return spell;
	}

	public static void Blight(Vector3 pos) {
		GameObject spell = makeSphere(pos, 0, 0.7f, 1.5f, "Blight Controller");
		spell.name = "Blight";
		spell.AddComponent<SpellEffect>().init(0.4f);
	}

	public static void Root(Vector3 pos, params bool[] isEnemy) {
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
		spell.AddComponent<SphereCollider> ().isTrigger = true;
		spell.name = "Root";
		bool enemy;
		if (isEnemy.Length > 0) {
			enemy = true;
		} else {
			enemy = false;
		}
		spell.AddComponent<SpellEffect>().init(2f, enemy);
	}

	public static void Damage(Vector3 pos, float angle, params bool[] isEnemy) {
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Sprite[] cSprites = Resources.LoadAll<Sprite> ("Textures/Spell Effects Sprite Sheet");
		GameObject spell = new GameObject();
		spell.AddComponent<SpriteRenderer>();
		spell.AddComponent<Animator>();
		spell.GetComponent<SpriteRenderer> ().sprite = cSprites [10];
		spell.transform.position = new Vector3 (pos.x, .5f, pos.z) + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
		spell.transform.localScale = new Vector3(.5f, .5f, .5f);
		spell.transform.localEulerAngles = new Vector3 (90, GameObject.Find("Necromancer").transform.localEulerAngles.y, 0);
		Animator anim = spell.GetComponent<Animator> ();
		anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Animations/Wave Controller");
		spell.AddComponent<SphereCollider> ().isTrigger = true;
		spell.transform.LookAt(mouse);
		spell.name = "Damage";
		bool enemy;
		if (isEnemy.Length > 0) {
			enemy = true;
		} else {
			enemy = false;
		}
		spell.AddComponent<SpellEffect>().init(2, enemy, 5, new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle))); 
	}

	public static void Blink(Transform me, float angle, params Vector3[] dest) {
		Sprite[] cSprites = Resources.LoadAll<Sprite> ("Textures/Teleport Sprite Sheet");
		GameObject start = new GameObject ();
		start.AddComponent<SpriteRenderer> ();
		start.AddComponent<Animator> ();
		start.GetComponent<SpriteRenderer> ().sprite = cSprites [0];
		start.transform.position = new Vector3 (me.position.x, me.position.y, me.position.z);
		start.transform.localEulerAngles = new Vector3 (90, 0, 0);
		Animator anim = start.GetComponent<Animator> ();
		string animationName;
		if (dest.Length == 1) {
			animationName = "Animations/Boss Blink Start Controller";
			start.transform.localScale *= 2;
		} else {
			animationName = "Animations/Blink Start Controller";
		}
		anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> (animationName);
		start.name = "Blink Start";
		GameObject.Destroy (start.gameObject, .5f);
		GameObject end = new GameObject ();
		end.AddComponent<SpriteRenderer> ();
		end.AddComponent<Animator> ();
		end.GetComponent<SpriteRenderer> ().sprite = cSprites [0];
		if (dest.Length == 1) {
			me.transform.position = dest [0];
			animationName = "Animations/Boss Blink End Controller";
			end.transform.localScale *= 2;
		} else {
			me.Translate (4 * new Vector3 (Mathf.Cos (angle), 0, Mathf.Sin (angle)));
			animationName = "Animations/Blink End Controller";
		}
		end.transform.position = new Vector3 (me.position.x, me.position.y, me.position.z);
		end.transform.localEulerAngles = new Vector3 (90, 0, 0);
		anim = end.GetComponent<Animator> ();
		anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> (animationName);
		end.name = "Blink End";
		//end.GetComponent<SpriteRenderer> ().sortingLayerName = "PlayerController";
		//end.GetComponent<SpriteRenderer> ().sortingOrder = 5;
		GameObject.Destroy (end.gameObject, .3f);
	}

	public static bool Bullet(Vector3 pos, float angle, params bool[] isEnemy) {
		Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//GameObject spell = AOE(.3f, Color.black, pos + 0.5f * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
		Sprite[] cSprites = Resources.LoadAll<Sprite> ("Textures/Spell Effects Sprite Sheet");
		GameObject spell = new GameObject();
		spell.AddComponent<SpriteRenderer>();
		spell.AddComponent<Animator>();
		spell.GetComponent<SpriteRenderer> ().sprite = cSprites [13];
		spell.transform.position = new Vector3 (pos.x, .5f, pos.z) + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
		spell.transform.localScale = new Vector3(.4f, .4f, .4f);
		spell.transform.localEulerAngles = new Vector3 (90, GameObject.Find("Necromancer").transform.localEulerAngles.y, 0);
		Animator anim = spell.GetComponent<Animator> ();
		anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController> ("Animations/Projectile Controller");
		spell.AddComponent<SphereCollider>();
		spell.transform.LookAt(mouse);
		spell.GetComponent<SphereCollider>().isTrigger = true;
		spell.name = "Bullet";
		bool enemy;
		if (isEnemy.Length > 0) {
			enemy = true;
		} else {
			enemy = false;
		}
		float speed = 10;
		if (enemy) {
			speed = 5f;
		}
		spell.AddComponent<SpellEffect>().init(1, enemy, speed, new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
		return true;
	}

	public static bool Arrow(Vector3 pos, float angle, bool enemy, float range, float damage) {
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
		spell.AddComponent<SpellEffect>().init(range, enemy, 10, new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)), damage);
		return true;
	}

	public static Vector3 NormalizeVector(Vector3 vector, params bool[] isZ) {
		Vector3 direction;
		if (isZ.Length == 0) {
			direction = new Vector3 (vector.x, vector.y);
		} else {
			direction = new Vector3 (vector.x, vector.y);
		}
		float directionMagnitude = Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
		direction = new Vector3(direction.x / directionMagnitude, direction.y / directionMagnitude);
		return direction;
	}
}
