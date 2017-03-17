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
	private static List<IHackerNetworked> _networkObjects = new List<IHackerNetworked>();

	private static TcpClient _host;
	private static BinaryFormatter _formatter = new BinaryFormatter();

	public static TcpClient Host { get { return _host; } }
	public static BinaryFormatter Formatter { get { return _formatter; } }

	public static T GetNetworkedObject<T> (int pId) where T : class, IHackerNetworked {
		IHackerNetworked temp = _networkObjects.Find(x => x.Id == pId);
		return temp is T ? temp as T : default(T);
	}

	/// <summary>
	/// Adds a networked object to the reference list
	/// </summary>
	/// <param name="pObject"></param>
	public static void RegisterNetworkObject (IHackerNetworked pObject) {
		_networkObjects.Add(pObject);
	}

	/// <summary>
	/// Removes a networked object from the reference list
	/// </summary>
	/// <param name="pObject"></param>
	public static void UnregisterNetworkedObject (IHackerNetworked pObject) {
		_networkObjects.Remove(pObject);
	}

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

			SceneManager.LoadScene("MenuScene");
		}
	}

	public static void SendPackage (NetworkPacket.AbstractPacket pPacket) {
		if (_host != null) {
			try {
				_formatter.Serialize(_host.GetStream(), pPacket);
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

