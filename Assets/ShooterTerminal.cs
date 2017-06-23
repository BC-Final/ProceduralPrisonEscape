using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTerminal : MonoBehaviour, IInteractable, IShooterNetworked
{
	private ShooterNetworkId _id = new ShooterNetworkId();
	public ShooterNetworkId Id { get { return _id; } }

	public void Initialize()
	{
		//TODO Only initialze when shooter saw object or database
		ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.Terminal.Creation(Id, transform.position));
	}

	private Renderer _cubeLight;
	private Light _light;
	[SerializeField]
	Color ActivateColor;
	[SerializeField]
	Transform RevealArea;

	public void Interact()
	{
		_cubeLight.material.color = ActivateColor;
		_light.color = ActivateColor;
		Vector2 pPos = new Vector2(RevealArea.position.x, RevealArea.position.z);
		Vector2 pSize = new Vector2(RevealArea.localScale.x, RevealArea.localScale.z);
		ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.Terminal.RevealArea(pPos, pSize));
		ShooterPackageSender.SendPackage(new NetworkPacket.GameObjects.HackerMinimapCamera.MoveCameraTowardsLocation(RevealArea.position));
	}

	// Use this for initialization
	void Start () {
		_cubeLight = GetComponentInChildren<Renderer>();
		_light = GetComponentInChildren<Light>();
	}

	private void Awake()
	{
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
