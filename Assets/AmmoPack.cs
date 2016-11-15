using UnityEngine;
using System.Collections;

public class AmmoPack : MonoBehaviour, IInteractable {
	[SerializeField]
	private int _amount;

	public void Interact() {
		//TODO Super Hacky please fix fast
		FindObjectOfType<Weapon_M9>().AddAmmo(_amount);
		Destroy(gameObject);
	}
}
