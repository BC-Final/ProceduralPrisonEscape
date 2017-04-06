using UnityEngine;
using System.Collections;
using Gamelogic.Extensions;

public class ShooterReferenceManager : Singleton<ShooterReferenceManager> {
	[Header("Weapon Prefabs")]
	public GameObject Phaser;
	public GameObject Machinegun;
	public GameObject Mininglaser;

	[Header("Pickup Prefabs")]
	public GameObject DroneBeacon;

	[Header("Effect Prefabs")]
	public GameObject BulletHole;
	public GameObject LaserShot;
	public GameObject BulletTracer;

	[Header("Other Prefabs")]
	public GameObject ExplodingDrone;
    public GameObject KeyCodeInputWindow;

    [Header("Icons")]
    public Sprite KeycardIcon;

	[Header("User Interface")]
	public GameObject HitMarker;
}
