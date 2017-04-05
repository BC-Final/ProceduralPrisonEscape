using UnityEngine;
using System.Collections;

public interface IDamageable {
	//void ReceiveDamage(IDamageable pSender, Vector3 pDirection, Vector3 pPoint, float pDamage);
	void ReceiveDamage (Transform pSource, Vector3 pHitPoint, float pDamage, float pForce);
	Faction Faction { get; }
	GameObject GameObject { get; }
}
