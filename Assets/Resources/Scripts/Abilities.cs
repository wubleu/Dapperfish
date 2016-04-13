using UnityEngine;
using System.Collections;

public static class Abilities {

	private static GameObject AOE(float radius, Color color, Vector3 pos) {
		GameObject spell = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		spell.transform.position = pos;
		spell.transform.localScale = new Vector3(radius, radius, radius);

		spell.GetComponent<SphereCollider>().isTrigger = true;
		spell.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Textures/Sphere");
		spell.AddComponent<SpellEffect>();

		return spell;
	}

	public static void Blight(Vector3 pos) {
		GameObject spell = AOE(3, Color.green, pos);
		spell.name = "Blight";
	}

	public static void Root(Vector3 pos) {
		GameObject spell = AOE(3, Color.blue, pos);
		spell.name = "Root";
	}

	public static void Damage(Vector3 pos, float angle) {
		GameObject spell = AOE(2, Color.green, pos);
		spell.GetComponent<SpellEffect>().angle = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
		spell.name = "Damage";
	}

	public static void Blink(float angle, Transform me) {
		me.Translate(4 * new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
	}
}

//		GameObject spell = new GameObject();
//		spell.transform.parent = GameObject.Find("Game Manager").transform;
//		spell.transform.localPosition = pos;
//		spell.transform.eulerAngles = new Vector3(90, 0, 0);

//		CircleCollider2D col = spell.AddComponent<CircleCollider2D>();
//		col.isTrigger = true;
//		col.radius = radius;
//		MeshFilter filter = spell.AddComponent<MeshFilter>();
//
//		MeshRenderer rend = spell.GetComponent<MeshRenderer>();
//		rend.material.color = new Color(1, 1, 1, 0.5f);

//		SpriteRenderer rend = spell.AddComponent<SpriteRenderer>();
//		rend.sprite = Resources.Load<Sprite>("Textures/Circle2");
//		rend.color = color;
//		GameObject spell = new GameObject();
//		spell.transform.position = pos;
//		spell.transform.localScale = new Vector3(6, 1, 1);
//
//		spell.AddComponent<BoxCollider2D>().isTrigger = true;
//		spell.AddComponent<SpellEffect>();

//		SpriteRenderer rend = spell.AddComponent<SpriteRenderer>();
//		rend.sprite = Resources.Load<Sprite>("Textures/Square");
//		rend.color = Color.red;