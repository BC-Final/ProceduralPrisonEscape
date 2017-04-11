using UnityEngine;
using System.Collections;
using Gamelogic.Extensions;

public class HackerReferenceManager : Singleton<HackerReferenceManager> {
    [Header("Minimap Icon Prefabs")]
    public GameObject AmmoMachineGun;
    public GameObject AmmoPhaser;
    public GameObject AmmoShotgun;
    public GameObject HealthKit;
    public GameObject CameraIcon;
	public GameObject DatabaseIcon;
    public GameObject DoorIcon;
	public GameObject SectorDoorIcon;
	public GameObject DefenceWallIcon;
	public GameObject DroneIcon;
	public GameObject FuseBoxIcon;
	public GameObject GasPipeIcon;
    public GameObject KeyCardIcon;
	public GameObject LightIcon;
    public GameObject PlayerIcon;
    public GameObject PushButton;
	public GameObject SecurityStationIcon;
	public GameObject SpeakerIcon;
    public GameObject TurretIcon;
	public GameObject VendingMachineIcon;
	public GameObject ModuleIcon;
	public GameObject ObjectiveIcon;

	[Header("Minimap Prefabs")]
    public GameObject Explosion;
	public GameObject LaserShot;

	[Header("Misc Prefabs")]
    public GameObject CodeInputWindow;
	public GameObject ContextMenu;
    public GameObject ContextMenuOption;
    public GameObject Decoder;
}
