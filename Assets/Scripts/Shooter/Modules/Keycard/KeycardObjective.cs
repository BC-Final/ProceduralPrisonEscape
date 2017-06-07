using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Gamelogic.Extensions;

public class KeycardObjective : AbstractObjective, IInteractable {
	[Space]
	[Header("Events")]

	[SerializeField]
	private UnityEngine.Events.UnityEvent _onInteract;

	public void Interact() {
		KeycardHolder holder = FindObjectOfType<KeycardHolder>();

		holder.AddKeycard(this);
		transform.parent = holder.transform;

		Rigidbody body = GetComponent<Rigidbody>();
		body.useGravity = false;
		body.isKinematic = true;

		GetComponentInChildren<Collider>().enabled = false;


		Sequence s = DOTween.Sequence()
			.Append(transform.DOLocalMove(Vector3.down / 2.0f, 0.25f).OnComplete(() => gameObject.SetActive(false)))
			.Join(transform.DORotate(Random.insideUnitSphere, 0.25f));

		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			s.Join(r.material.DOFade(0.0f, 0.2f));
		}

		_onInteract.Invoke();
	}

	public void Insert() {
		SetSolved();
		Destroy(this.gameObject);
	}
}
