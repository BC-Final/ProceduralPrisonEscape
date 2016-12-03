using UnityEngine;
using System.Collections;

public class HealthKit : MonoBehaviour, IInteractable {
	[SerializeField]
	private int _amount;

	public void Interact() {
		FindObjectOfType<PlayerHealth>().HealDamage(_amount);
		Destroy(gameObject);
	}
}
