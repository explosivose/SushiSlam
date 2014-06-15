using UnityEngine;
using System.Collections;

public class CharacterSelection : Singleton<CharacterSelection> {

	public int numberOfChoices = 2;
	
	public int p1Choice = 1;
	public int p2Choice = 2;
	
	public Transform p1selector;
	public Transform p2selector;
	
	// Use this for initialization
	void Start () {
		StartCoroutine(p1select());
		StartCoroutine(p2select());
	}
	

	IEnumerator p1select() {
		while(true)
		{
			float p1 = Input.GetAxis("P1Move");
			if (p1 > 0f) {
				p1Choice ++;
				if (p1Choice > numberOfChoices) p1Choice = 1;
				yield return new WaitForSeconds(0.2f);
			} 
			if (p1 < 0f) {
				p1Choice --;
				if (p1Choice < 1) p1Choice = numberOfChoices;
				yield return new WaitForSeconds(0.2f);
			}
			yield return new WaitForFixedUpdate();
		} 
	}
	
	IEnumerator p2select() {
		while(true)
		{
			float p2 = Input.GetAxis("P1Move");
			if (p2 > 0f) {
				p2Choice ++;
				if (p2Choice > numberOfChoices) p2Choice = 1;
				yield return new WaitForSeconds(0.2f);
			} 
			if (p2 < 0f) {
				p2Choice --;
				if (p2Choice < 1) p2Choice = numberOfChoices;
				yield return new WaitForSeconds(0.2f);
			}
			yield return new WaitForFixedUpdate();
		} 
	}
}
