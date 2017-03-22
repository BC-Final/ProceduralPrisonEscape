using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera Parameters", menuName = "Agent Parameters/Camera Parameters")]
public class CameraParameters : ScriptableObject {
	public float ViewRange;
	public float ViewAngle;

	public float RotationSpeed;
	public float RotationAngle;
	public float FollowRotationSpeed;

	public float TriggerDelay;

	public float NetworkUpdateRate;

	public float DisableDuration;
	public float ControllDuration;
}
