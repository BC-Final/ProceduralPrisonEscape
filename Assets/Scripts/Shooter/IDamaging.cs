using UnityEngine;
using System.Collections;

public interface IDamaging {
	void Shoot();
	bool Reload();
	void Melee();
	//void AddAmmo(float pAmount);
}
