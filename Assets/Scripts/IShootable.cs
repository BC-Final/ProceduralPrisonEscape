using UnityEngine;
using System.Collections;

public interface IShootable {
	void ReceiveDamage(Vector3 pDirection, Vector3 pPoint, float pDamage);
}
