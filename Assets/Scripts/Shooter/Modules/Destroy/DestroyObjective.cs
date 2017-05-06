using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjective : AbstractObjective, IDamageable {
	public void ReceiveDamage (Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
		if (pSource.GetComponent<PlayerHealth>() != null) {
			GetComponentInChildren<ParticleSystem>(true).gameObject.SetActive(true);
			SetSolved();
		}
	}

	public Faction Faction { get { return Faction.Neutral; } }
	public GameObject GameObject { get { return gameObject; } }
}
