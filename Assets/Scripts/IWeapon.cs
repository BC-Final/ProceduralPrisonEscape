using UnityEngine;
using System.Collections;

public interface IWeapon {
	void Shoot();
	bool Reload();
	void Melee();
	//void AddAmmo(float pAmount);
}
