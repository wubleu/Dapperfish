using UnityEngine;
using System.Collections;

public static class Abilities {

	private static GameObject AOE(float radius, Color color, Vector3 pos) {
		GameObject spell = new GameObject();
		spell.transform.position = pos;
		spell.transform.localScale = new Vector3(radius, radius, 1);

		spell.AddComponent<CircleCollider2D>().isTrigger = true;
		spell.AddComponent<SpellEffect>();

		SpriteRenderer rend = spell.AddComponent<SpriteRenderer>();
		rend.sprite = Resources.Load<Sprite>("Textures/Circle2");
		rend.color = color;

		return spell;
	}

	public static void Blight(Vector3 pos) {
		GameObject spell = AOE(3, Color.green, pos);
		spell.name = "Blight";
	}

	public static void Root(Vector3 pos) {
		GameObject spell = AOE(4, Color.blue, pos);
		spell.name = "Root";
	}

	public static void Damage(Vector3 pos) {
		GameObject spell = new GameObject();
		spell.name = "Damage";
		spell.transform.position = pos;
		spell.transform.localScale = new Vector3(6, 1, 1);

		spell.AddComponent<BoxCollider2D>().isTrigger = true;
		spell.AddComponent<SpellEffect>();

		SpriteRenderer rend = spell.AddComponent<SpriteRenderer>();
		rend.sprite = Resources.Load<Sprite>("Textures/Square");
		rend.color = Color.red;
	}

	public static void Blink(Vector3 pos, Transform me) {
		if (Vector3.Distance(pos, me.position) > 5) {
			me.Translate(5 * Vector3.Normalize(pos - me.position));
		} else {
			me.localPosition = pos;
		}
	}
}
