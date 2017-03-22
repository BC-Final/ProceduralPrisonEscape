using UnityEngine;
using System.Collections;

public interface IDamageable {
	void ReceiveDamage(IDamageable pSender, Vector3 pDirection, Vector3 pPoint, float pDamage);
	void AddToDestroyEvent (System.Action<GameObject> pAction);
	void RemoveFromDestroyEvent (System.Action<GameObject> pAction);
	Faction Faction { get; }
	GameObject GameObject { get; }
}
