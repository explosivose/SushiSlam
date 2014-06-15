using UnityEngine;
using System.Collections;

public static class Wait {

	public static IEnumerator ForRealSeconds(float time) {
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time) {
			yield return null;
		}
	}
}
