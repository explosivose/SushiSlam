using UnityEngine;
using System.Collections;

public class SushiSpawner : MonoBehaviour {
	public GameObject[] spawnLocations;
	public bool sushiInPlay = false;
	public float sushiSpawnTime;
	public float sushiStartTime;

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
				int spawnSel = Random.Range(1, 5);
				Instantiate(Sushi, spawnLocations[spawnSel].transform.position);
				sushiInPlay = true;
			}
		}
	}

	public IEnumerator StartSpawn() {
		sushiInPlay = false;
		sushiStartTime = Time.time;
		yield return null;
	}
}
