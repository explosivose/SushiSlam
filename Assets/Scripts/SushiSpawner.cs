using UnityEngine;
using System.Collections;

public class SushiSpawner : MonoBehaviour {
	public GameObject[] spawnLocations;
	public bool sushiInPlay = false;
	public float sushiSpawnTime;
	public float sushiStartTime;
	public GameObject sushi;

	// Use this for initialization
	void Start () {
		sushiInPlay = false;
		sushiSpawnTime = 15;
		sushiStartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (sushiInPlay == false) {
			if (Time.time > sushiStartTime + sushiSpawnTime) {
				int spawnSel = Random.Range(0, 4);
				Instantiate(sushi, spawnLocations[spawnSel].transform.position, Quaternion.identity);
				sushiInPlay = true;
			}
		}
	}

	public IEnumerator StartSpawn() {
		Debug.Log ("respawning sushi");
		sushiInPlay = false;
		sushiStartTime = Time.time;
		yield return null;
	}
}
