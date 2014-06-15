using UnityEngine;
using System.Collections;

public class MusicManager : Singleton<MusicManager> {

	public float maxVolume = 0.5f;
	public AudioClip menuMusic;
	public AudioClip menuStart;
	public AudioClip fightMusic;
	public AudioClip fightStart;
	public AudioClip sushiSlam;
	
	void Awake() {
		gameObject.AddComponent<AudioSource>();
		audio.loop = true;
		DontDestroyOnLoad(this.gameObject);
		menuMusic = Resources.Load ("Music/charselect") as AudioClip;
		menuStart = Resources.Load ("Music/intro") as AudioClip;
		fightMusic = Resources.Load ("Music/basicbeat") as AudioClip;
		fightStart = Resources.Load("Music/ready") as AudioClip;
		sushiSlam = Resources.Load("Music/sushislam (speed me up!)") as AudioClip;
	}
	
	void Update() {
		if (audio.clip == sushiSlam) {
			Debug.Log ("derp");
			audio.pitch += Time.deltaTime * 0.01f;
			if (audio.pitch > 1.4f) audio.pitch = 1.4f;
		}
		else {
			audio.pitch = 1f;
		}
	}
	
	public IEnumerator FadeOut() {
		while (audio.volume > 0.1f) {
			audio.volume -= 0.01f;
			yield return new WaitForEndOfFrame();
		}
		audio.Stop();
	}
	
	public IEnumerator FastFadeOut() {
		while (audio.volume > 0.1f) {
			audio.volume -= 0.05f;
			yield return new WaitForEndOfFrame();
		}
		audio.Stop();
	}
	
	public IEnumerator MainMenu() {
		yield return StartCoroutine(FastFadeOut());
		AudioSource.PlayClipAtPoint(menuStart, Vector3.zero);
		yield return new WaitForSeconds(2.5f);
		audio.clip = menuMusic;
		audio.volume = maxVolume;
		audio.Play();
	}
	
	public IEnumerator FightStart() {
		yield return StartCoroutine(FastFadeOut());
		AudioSource.PlayClipAtPoint(fightStart, Vector3.zero);
		yield return new WaitForSeconds(8f);
		audio.clip = fightMusic;
		audio.volume = maxVolume;
		audio.Play();
	}
	
	public IEnumerator Fight() {
		yield return new WaitForSeconds(1f);
		audio.clip = fightMusic;
		audio.volume = maxVolume;
		audio.Play();
	}
	
	public IEnumerator Sushi() {
		yield return StartCoroutine(FastFadeOut());
		audio.clip = sushiSlam;
		audio.volume = maxVolume;
		audio.Play();
	}
	

	
}
