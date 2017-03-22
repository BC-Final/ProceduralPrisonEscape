using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret Parameters", menuName = "Agent Parameters/Turret Parameters")]
public class TurretParameters : ScriptableObject {

	public float AttackDamage;
	public float AttackRange;
	public float AttackRate;
	public float AttackAngle;

	public float SpreadConeLength;
	public float SpreadConeRadius;

	public float ViewRange;

	public float HorizontalRotationSpeed;
	public float VerticalRotationSpeed;

	public float DeployDuration;
	public float ScanDuration;

	public float ScanRotationSpeed;
	public float ScanRotationAngle;

	public float DisableDuration;
	public float ControllDuration;

	public float MaxHorizontalRotation;

	public float NetworkUpdateRate;
}
