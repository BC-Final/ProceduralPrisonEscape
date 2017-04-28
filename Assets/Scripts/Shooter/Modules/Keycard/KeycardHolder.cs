using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardHolder : MonoBehaviour {
	private List<KeycardObjective> _keycards = new List<KeycardObjective>();

	public bool HasKeycard(KeycardObjective pKeycard) {
		return _keycards.Find(x => x.gameObject == pKeycard.gameObject) != null;
	}

	public void AddKeycard(KeycardObjective pKeycard) {
		_keycards.Add(pKeycard);
	}

	public void RemoveKeycard(KeycardObjective pKeycard) {
		_keycards.Remove(_keycards.Find(x => x.gameObject == pKeycard.gameObject));
	}
}
