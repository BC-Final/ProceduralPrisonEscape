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

public class HackerPackageSender : MonoBehaviour
{
	public static HackerPackageSender _instance;
	public static HackerPackageSender GetInstance()
	{
		if (_instance == null)
		{
			_instance = FindObjectOfType<HackerPackageSender>();
			if (_instance == null)
			{
				Debug.Log("ERROR!!! PACKAGE SENDER NOT FOUND");
			}
		}
		return _instance;
	}

	[SerializeField]
	private ChatBoxScreen _chatBoxScreen;
	[SerializeField]
	private Transform _minimap;
	private Texture _minimapTexture;

	private static TcpClient client;
	private static NetworkStream stream;
	private static BinaryFormatter formatter;

	// Use this for initialization
	private void Awake()
	{
		Application.runInBackground = true;
		formatter = new BinaryFormatter();
		try
		{
			Debug.Log("Connecting to : " + PlayerPrefs.GetString("ConnectionIP") + ":" + PlayerPrefs.GetString("ConnectionPort"));
			int port;
			int.TryParse(PlayerPrefs.GetString("ConnectionPort"), out port);
            client = new TcpClient(PlayerPrefs.GetString("ConnectionIP"), port);
			stream = client.GetStream();
		}
		catch (SocketException e)
		{
			if (e.ErrorCode.ToString() == "10061")
			{
				Debug.Log("Connection refused. Server is propably full");
			}
			else
			{
				Debug.Log("Could not connect to server. Errorcode : " + e.ErrorCode);
			}

			Debug.Break();
			SceneManager.LoadScene("MenuScene");
		}
	}

	public TcpClient GetClient()
	{
		return client;
	}

	public NetworkStream GetStream()
	{
		return stream;
	}

	public BinaryFormatter GetFormatter()
	{
		return formatter;
	}
	//Message Management

	/// <summary>
	/// Sends a custom request to the server
	/// </summary>
	/// <param name="package">Request type</param>
	private void SendPackage(CustomCommands.AbstractPackage package)
	{
		try
		{
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, package);
		}
		catch (SerializationException e)
		{
			Console.WriteLine("Failed to serialize. Reason: " + e.Message);
			throw;
		}

	}

	public void SendDoorUpdate(HackerDoor door)
	{
		SendPackage(new CustomCommands.Update.DoorUpdate(door.GetID(), door.GetDoorState().ToString()));
	}

	//Exit Methods
	private void OnProcessExit(object sender, EventArgs e)
	{
		SendPackage(new CustomCommands.NotImplementedMessage());
	}

	private void OnApplicationQuit()
	{
		SendPackage(new CustomCommands.NotImplementedMessage());
	}
}

