using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using System;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Gamelogic.Extensions;

public class HackerPackageSender : Singleton<HackerPackageSender> {

	[SerializeField]
	private ChatBoxScreen _chatBoxScreen;
	[SerializeField]
	private Transform _minimap;
	private Texture _minimapTexture;

	private static TcpClient client;
	private static NetworkStream stream;
	private static BinaryFormatter formatter;

	// Use this for initialization
	private void Awake () {
		Application.runInBackground = true;
		formatter = new BinaryFormatter();
		try {
			Debug.Log("Connecting to : " + PlayerPrefs.GetString("ConnectionIP") + ":" + PlayerPrefs.GetString("ConnectionPort"));
			int port;
			int.TryParse(PlayerPrefs.GetString("ConnectionPort"), out port);
			client = new TcpClient(PlayerPrefs.GetString("ConnectionIP"), port);
			stream = client.GetStream();
		} catch (SocketException e) {
			if (e.ErrorCode.ToString() == "10061") {
				Debug.Log("Connection refused. Server is propably full");
			} else {
				Debug.Log("Could not connect to server. Errorcode : " + e.ErrorCode);
			}

			Debug.Break();
			SceneManager.LoadScene("MenuScene");
		}
	}

	public TcpClient GetClient () {
		return client;
	}

	public NetworkStream GetStream () {
		return stream;
	}

	public BinaryFormatter GetFormatter () {
		return formatter;
	}

	private void SendPackage (CustomCommands.AbstractPackage package) {
		try {
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, package);
		} catch (SerializationException e) {
			Console.WriteLine("Failed to serialize. Reason: " + e.Message);
			throw;
		}
	}

	public void SendDoorUpdate (HackerDoor door) {
		SendPackage(new CustomCommands.Update.DoorUpdate(door.Id, (int)door.State.Value));
	}

	private void OnProcessExit (object sender, EventArgs e) {
		//TODO Properly disconnect
		SendPackage(new CustomCommands.NotImplementedMessage());
	}

	private void OnApplicationQuit () {
		//TODO Properly disconnect
		SendPackage(new CustomCommands.NotImplementedMessage());
	}
}

