using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrenadeAmmoPack : MonoBehaviour, IInteractable {
	public void Interact() {
		GrenadeThrow g = FindObjectOfType<GrenadeThrow>();

		if (g.NoOfGrenades < g.MaxGrenades) {
			g.NoOfGrenades = g.NoOfGrenades + 1;

			transform.parent = FindObjectOfType<PlayerHealth>().transform;

			GetComponent<Rigidbody>().useGravity = false;
			GetComponent<Rigidbody>().isKinematic = true;
			GetComponentInChildren<Collider>().enabled = false;

			Sequence s = DOTween.Sequence();
			s.Append(transform.DOLocalMove(Vector3.down / 2.0f, 0.25f).OnComplete(() => Destroy(this.gameObject)));
			s.Join(transform.DORotate(Random.insideUnitSphere, 0.25f));
			foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
				s.Join(r.material.DOFade(0.0f, 0.2f));
			}
		}
	}
}
