using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drone Parameters", menuName = "Agent Parameters/Drone Parameters")]
public class DroneParameters : ScriptableObject {
	public enum DroneAttackType {
		Ranged,
		Melee
	}

	public DroneAttackType AttackType;
	public float AttackDamage;
	public float AttackRange;
	public float AttackRate;
	public float AttackAngle;
	public float AttackForce;

	public float SpreadConeLength;
	public float SpreadConeRadius;

	public float AwarenessRange;
	public float ViewRange;
	public float ViewAngle;

	public float MaximumHealth;

	public float PathTickRate;

	public float IdleReactionTime;
	public float AlertReactionTime;

	public int SearchProbeCount;
	public float SearchProbeRadius;

	public float RotationSpeed;

	public float NetworkUpdateRate;

	public float StunDuration;
}
