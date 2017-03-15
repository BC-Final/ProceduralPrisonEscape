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
	private static TcpClient _host;
	public static TcpClient Host { get { return _host; } }
	private static BinaryFormatter _formatter = new BinaryFormatter();
	public static BinaryFormatter Formatter { get { return _formatter; } }

	// Use this for initialization
	private void Awake () {
		try {
			Debug.Log("Connecting to : " + PlayerPrefs.GetString("ConnectionIP") + ":" + PlayerPrefs.GetInt("ConnectionPort"));
			int port = PlayerPrefs.GetInt("ConnectionPort");
			_host = new TcpClient(PlayerPrefs.GetString("ConnectionIP", "127.0.0.1"), port);
		} catch (SocketException e) {
			if (e.ErrorCode.ToString() == "10061") {
				Debug.Log("Connection refused. Server is propably full");
			} else {
				Debug.Log("Could not connect to server. Errorcode : " + e.ErrorCode);
			}

			//SceneManager.LoadScene("MenuScene");
		}
	}

	public static void SendPackage (CustomCommands.AbstractPackage package) {
		if (_host != null) {
			try {
				_formatter.Serialize(_host.GetStream(), package);
			} catch (SerializationException e) {
				Debug.LogError("Failed to serialize. Reason: " + e.Message);
				throw;
			}
		}
	}

	public static void SilentlyDisconnect () {
		disconnectHost();

		SceneManager.LoadScene("MenuScene");
	}

	private void OnApplicationQuit () {
		disconnectHost();
	}

	#if UNITY_EDITOR
	private void OnDestroy () {
		disconnectHost();
	}
	#endif

	private static void disconnectHost () {
		if (_host != null) {
			_host.GetStream().Close();
			_host.Close();
			_host = null;
		}
	}
}

