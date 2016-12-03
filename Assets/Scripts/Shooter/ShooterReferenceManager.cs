﻿using UnityEngine;
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
}
