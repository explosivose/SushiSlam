using UnityEngine;
using System.Collections;

public class SushiSpawner : Singleton<SushiSpawner> {

	public AudioClip sushiSpawnClip;

	public GameObject[] spawnLocations;
	public bool sushiInPlay = false;
	public float sushiSpawnTime;
	public float sushiStartTime;
	public GameObject sushi;

	// Use this for initialization
	void Start () {
		sushiInPlay = false;
		sushiStartTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (sushiInPlay == false) {
			if (Time.time > sushiStartTime + sushiSpawnTime) {
				int spawnSel = Random.Range(0, 4);
				Instantiate(sushi, spawnLocations[spawnSel].transform.position, Quaternion.identity);
				sushiInPlay = true;
				AudioSource.PlayClipAtPoint(sushiSpawnClip, transform.position);
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
