using UnityEngine;
using System.Collections;

public interface IDamageable {
	void ReceiveDamage(Vector3 pDirection, Vector3 pPoint, float pDamage);
}
