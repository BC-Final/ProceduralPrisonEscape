using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour, IInteractable {
	private bool _accessible = true;
	private bool _used;

	[SerializeField]
	private GameObject _spawnItemPrefab;

	public void Interact() {
		if (_accessible && !_used) {
			Instantiate(_spawnItemPrefab, transform.position + -transform.right / 2.0f, Quaternion.identity);
			_used = true;
		}
	}
}
