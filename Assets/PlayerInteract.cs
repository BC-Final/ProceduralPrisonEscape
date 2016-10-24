﻿using UnityEngine;
using System.Collections;

public class PlayerInteract : MonoBehaviour {
	[SerializeField]
	private float _range;

	void Update () {
		if (Input.GetKeyDown(KeyCode.E)) {
			Transform cam = Camera.main.transform;
			RaycastHit[] hits;

			hits = Physics.RaycastAll(cam.position, cam.forward, _range);

			if (hits.Length > 0) {
				foreach (RaycastHit hit in hits) {
					IInteractable act = hit.collider.GetComponent<IInteractable>();
					if (act != null) act.Interact();
				}
			}
		}
	}
}
