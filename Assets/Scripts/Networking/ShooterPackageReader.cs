using System;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;

public class ShooterPackageReader : MonoBehaviour
{
	private static List<TcpClient> _clients;
	private BinaryFormatter _formatter;

	private void Start() {
		_clients = new List<TcpClient>();
		_formatter = new BinaryFormatter();
	}

	private void Update() {
		foreach (TcpClient client in _clients) {
			try {
				if (client.Available != 0) {
					CustomCommands.AbstractPackage response = _formatter.Deserialize(client.GetStream()) as CustomCommands.AbstractPackage;
					ReadPackage(response);
				}
			}
			catch (Exception e) {
				Debug.Log("Error" + e.ToString());
			}
		}
	}
	/*
	private void ReadResponse(CustomCommands.AbstractPackage response)
	{
		if (response is CustomCommands.NotImplementedMessage)
		{
			CustomCommands.NotImplementedMessage NIM = response as CustomCommands.NotImplementedMessage;
			Debug.Log("NotImplemented");
		}
		if (response is CustomCommands.Update.MinimapUpdate)
		{
			//CustomCommands.SendMinimapUpdate MU = response as CustomCommands.SendMinimapUpdate;
			//Texture2D tex = new Texture2D(2, 2);
			//tex.LoadImage(MU.bytes);
			//_minimap.GetComponent<Renderer>().material.mainTexture = tex;
			//Debug.Log("Minimap Updated");
			//_chatBoxScreen.AddChatLine("Minimap Update");
		}
		if (response is CustomCommands.Update.PlayerPositionUpdate)
		{
			//Debug.Log("Updating Player Position");
			//CustomCommands.PlayerPositionUpdate PPU = response as CustomCommands.PlayerPositionUpdate;
			//_minimapManager.UpdateMinimapPlayer(new Vector3(PPU.x, 0, PPU.z));
		}
		if (response is CustomCommands.Creation.DoorCreation)
		{
			//Debug.Log("Updating Door");
			CustomCommands.Creation.DoorCreation DU = response as CustomCommands.Creation.DoorCreation;
			//Debug.Log("Update for Door : " + DU.ID);
			if (!_doorManager.DoorAlreadyExists(DU.ID))
			{
				Debug.Log("-Door does not exists ");
				//_doorManager.CreateDoor(new Vector3(DU.x / _minimapScale.x, 0, DU.z / _minimapScale.y), DU.rotationY, DU.state, DU.ID);
			}
			else
			{
				//Debug.Log("-Door exists");
				_doorManager.UpdateDoor(DU);
			}
		}
		if (response is CustomCommands.Update.DoorUpdate)
		{
			//Debug.Log("Updating Door");
			CustomCommands.Update.DoorUpdate DU = response as CustomCommands.Update.DoorUpdate;
			//Debug.Log("Update for Door : " + DU.ID);
			if (!_doorManager.DoorAlreadyExists(DU.ID))
			{
				Debug.Log("-Door does not exists ");
				//_doorManager.CreateDoor(new Vector3(DU.x / _minimapScale.x, 0, DU.z / _minimapScale.y), DU.rotationY, DU.state, DU.ID);
			}
			else
			{
				//Debug.Log("-Door exists");
				_doorManager.UpdateDoorState(DU);
			}
		}
	}
	*/

	/// <summary>
	/// Reading incoming Packages
	/// </summary>
	/// <param name="package"></param>
	private void ReadPackage(CustomCommands.AbstractPackage package)
	{
		if(package is CustomCommands.Update.DoorUpdate) { ReadPackage(package as CustomCommands.Update.DoorUpdate); return;}

		//If package method not found
		Debug.Log("ERROR!!! NOT SUITABLE METHOD FOR THIS PACKAGE FOUND");
	}

	private void ReadPackage(CustomCommands.Update.DoorUpdate package)
	{
		ShooterDoor.UpdateDoor(package);
	}

	public static void SetClients(List<TcpClient> clients)
	{
		_clients = clients;
	}
}