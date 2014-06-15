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
