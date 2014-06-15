using UnityEngine;
using System.Collections;

public class CharacterSelection : Singleton<CharacterSelection> {

	public int p1Choice = 0;
	public int p2Choice = 1;
	
	public Transform[] selection;
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
			float p1 = Input.GetAxisRaw("P1Move");
			if (p1 > 0f) {
				p1Choice ++;
				if (p1Choice > selection.Length-1) p1Choice = 1;
				yield return new WaitForSeconds(0.2f);
			} 
			if (p1 < 0f) {
				p1Choice --;
				if (p1Choice < 0) p1Choice = selection.Length-1;
				yield return new WaitForSeconds(0.2f);
			}
			p1selector.position = selection[p1Choice].position + Vector3.up;
			yield return new WaitForFixedUpdate();
		} 
	}
	
	IEnumerator p2select() {
		while(true)
		{
			float p2 = Input.GetAxisRaw("P2Move");
			if (p2 > 0f) {
				p2Choice ++;
				if (p2Choice > selection.Length-1) p2Choice = 1;
				yield return new WaitForSeconds(0.2f);
			} 
			if (p2 < 0f) {
				p2Choice --;
				if (p2Choice < 0) p2Choice = selection.Length-1;
				yield return new WaitForSeconds(0.2f);
			}
			p2selector.position = selection[p2Choice].position + Vector3.up;
			yield return new WaitForFixedUpdate();
		} 
	}
}
