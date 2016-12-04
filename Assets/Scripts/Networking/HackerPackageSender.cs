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

	private static TcpClient _client;
	private static NetworkStream _stream;
	private static BinaryFormatter _formatter;

	// Use this for initialization
	private void Awake () {
		Application.runInBackground = true;
		_formatter = new BinaryFormatter();

		try {
			Debug.Log("Connecting to : " + PlayerPrefs.GetString("ConnectionIP") + ":" + PlayerPrefs.GetInt("ConnectionPort"));
			int port = PlayerPrefs.GetInt("ConnectionPort");
			_client = new TcpClient(PlayerPrefs.GetString("ConnectionIP", "127.0.0.1"), port);
			_stream = _client.GetStream();
		} catch (SocketException e) {
			if (e.ErrorCode.ToString() == "10061") {
				Debug.Log("Connection refused. Server is propably full");
			} else {
				Debug.Log("Could not connect to server. Errorcode : " + e.ErrorCode);
			}

			SceneManager.LoadScene("MenuScene");
		}
	}

	public TcpClient GetClient () {
		return _client;
	}

	public NetworkStream GetStream () {
		return _stream;
	}

	public static void SendPackage (CustomCommands.AbstractPackage package) {
		try {
			_formatter.Serialize(_stream, package);
		} catch (SerializationException e) {
			Console.WriteLine("Failed to serialize. Reason: " + e.Message);
			throw;
		}
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

