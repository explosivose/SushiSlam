using UnityEngine;
using System.Collections;

public class SushiCamera : Singleton<SushiCamera> {

	public void Initialise(Transform player1, Transform player2) {
		p1 = player1;
		p2 = player2;
		ready = true;
	}
	
	public Transform p1, p2;
	public bool ready = false;
	// Update is called once per frame
	void Update () {
		if(!ready) return;
		Vector3 average = (p1.position + p2.position)/2f;
		float distance = Vector3.Distance(p1.position, p2.position);
		average.z = -10f;
		transform.position = average;
		camera.orthographicSize = distance;
	}
}
