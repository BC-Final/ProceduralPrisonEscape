using UnityEngine;
using System.Collections;

public class PlayerInteract : MonoBehaviour {
	[SerializeField]
	private float _range;

	public void SetActive (bool pActive) {
		_active = pActive;
	}

	private bool _active = true;

	void Update () {
		//TODO can interact through walls
		if (_active && Input.GetKeyDown(KeyCode.E)) {
			Transform cam = Camera.main.transform;
			RaycastHit[] hits;

			hits = Physics.RaycastAll(cam.position, cam.forward, _range);

			if (hits.Length > 0) {
				foreach (RaycastHit hit in hits) {
					IInteractable act = hit.collider.GetComponentInParent<IInteractable>();
					if (act != null) act.Interact();
				}
			}
		}
	}
}
