using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KeycardObjective : AbstractObjective, IInteractable {
	public void Interact() {
		FindObjectOfType<KeycardHolder>().AddKeycard(this);

		transform.parent = FindObjectOfType<KeycardHolder>().transform;

		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponentInChildren<Collider>().enabled = false;


		Sequence s = DOTween.Sequence();
		s.Append(transform.DOLocalMove(Vector3.down / 2.0f, 0.25f).OnComplete(() => gameObject.SetActive(false)));
		s.Join(transform.DORotate(Random.insideUnitSphere, 0.25f));
		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			s.Join(r.material.DOFade(0.0f, 0.2f));
		}
	}

	public void Insert() {
		SetSolved();
		Destroy(this.gameObject);
	}
}
