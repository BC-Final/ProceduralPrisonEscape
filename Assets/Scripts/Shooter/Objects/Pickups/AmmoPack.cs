using UnityEngine;
using System.Collections;

public abstract class AmmoPack : MonoBehaviour, IInteractable {
	[SerializeField]
	protected int _amount;

	public virtual void Interact() {
		Destroy(gameObject);
	}
}
