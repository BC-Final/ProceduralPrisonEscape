using UnityEngine;
using System.Collections;
using Gamelogic.Extensions;

public class HackerReferenceManager : Singleton<HackerReferenceManager> {
    [Header("Minimap Icon Prefabs")]
    public GameObject PlayerIcon;
    public GameObject DoorIcon;
	public GameObject DroneIcon;
    public GameObject CameraIcon;
    public GameObject TurretIcon;
	public GameObject GasPipeIcon;
	public GameObject FuseBoxIcon;
	public GameObject DefenceWallIcon;
	public GameObject LightIcon;
	public GameObject SpeakerIcon;
	public GameObject VendingMachineIcon;
	public GameObject DatabaseIcon;
	public GameObject SecurityStationIcon;

	[Header("Minimap Prefabs")]
	public GameObject LaserShot;
}
