using UnityEngine;
using System.Collections;

public interface IDamageable {
	void ReceiveDamage(IDamageable pSender, Vector3 pDirection, Vector3 pPoint, float pDamage);
	Faction Faction { get; }
	GameObject GameObject { get; }
}
