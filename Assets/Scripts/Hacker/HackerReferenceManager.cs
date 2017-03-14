using UnityEngine;
using System.Collections;
using Gamelogic.Extensions;

public class HackerReferenceManager : Singleton<HackerReferenceManager> {
	[Header("Minimap Icon Sprites")]
	public Sprite KeycardIcon;
	public Sprite HealthpackIcon;
	public Sprite AmmobagIcon;

    [Header("Minimap Icon Prefabs")]
    public GameObject PlayerIcon;
    public GameObject DoorIcon;
    public GameObject FireWallIcon;
    public GameObject PickupIcon;
	public GameObject DroneIcon;
    public GameObject CameraIcon;
    public GameObject TurretIcon;

	[Header("Minimap Prefabs")]
	public GameObject LaserShot;

	[Header("Network Avatar Prefabs")]
	public GameObject HackerAvatar;
	public GameObject AdminAvatar;

	[Header("Network Packet Prefabs")]
	public GameObject AlarmPacket;
	public GameObject FeedbackPacket;
	public GameObject SuspicionPacket;

	[Header("Network Node Prefabs")]
	public GameObject BaseNode;
	public GameObject DatabaseNode;
	public GameObject DoorNode;
	public GameObject FirewallNode;
	public GameObject DispenserNode;
	public GameObject HackerNode;
	public GameObject SecurityNode;
	public GameObject CameraNode;
	public GameObject TurretNode;

	[Header("Other Network Prefabs")]
	public GameObject ConnenctionLine;
}
