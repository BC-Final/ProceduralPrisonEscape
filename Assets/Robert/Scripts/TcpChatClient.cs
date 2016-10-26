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

public class TcpChatClient : MonoBehaviour
{
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
		_chatBoxScreen.RegisterChatInputHandler(onChatTextEntered);
		_chatBoxScreen.ClearChat();
		try
		{
			client = new TcpClient("localhost", 55556);
			stream = client.GetStream();
			//Thread ReadThread = new Thread(PackageReader);
			//ReadThread.Start();
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


			Application.Quit();
			Debug.Break();
		}
	}
	//Do things when you input something in the chat window
	private void onChatTextEntered()
	{
		//Get Message from Chat input
		string msg = _chatBoxScreen.GetChatInput();
		_chatBoxScreen.SetChatInput("");
		_chatBoxScreen.AddChatLine(msg);

		//Process, send Message and recieve Response
		if (!ProcessMessage(msg))
		{
			Debug.Log("Error processing message");
		}
		_chatBoxScreen.ScrollToBottom();
	}

	//Getters

	public ChatBoxScreen GetChatBoxScreen()
	{
		return _chatBoxScreen;
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
	/// Processes the message.Then checks if and what request should be send. 
	/// </summary>
	/// <param name="pMessage">Message that was input</param>
	/// <returns>Returns true if Processed correctly and request was send. Returns false when no request was send</returns>
	private bool ProcessMessage(string pMessage)
	{
		String[] substrings = pMessage.Split(' ');
	
		switch (substrings[0].ToUpper())
		{
			case "!MINIMAP":
				{
					SendRequest(new CustomCommands.MinimapUpdateRequest());
					break;
				}
			case "!PLAYERPOS":
				{
					SendRequest(new CustomCommands.PlayerPositionUpdateRequest());
					break;
				}
			default:
				{
					_chatBoxScreen.AddChatLine("Don't know what to do? Try !help");
					return false;
				}
		}
		return true;
	}
	//
	////Read incoming bytes and returns them when finished
	//private byte[] ReadBytes(int pByteCount)
	//{
	//	byte[] bytes = new byte[pByteCount];
	//	int bytesRead = 0;
	//	int totalBytesRead = 0;
	//
	//	try
	//	{
	//		while (totalBytesRead != pByteCount && (bytesRead = stream.Read(bytes, totalBytesRead, pByteCount - totalBytesRead)) > 0)
	//		{
	//			Debug.Log("Total Bytes Read : " + totalBytesRead);
	//			totalBytesRead += bytesRead;
	//		}
	//	}
	//	catch {
	//		Console.WriteLine("Something went wrong");
	//	}
	//
	//	return (totalBytesRead == pByteCount) ? bytes : null;
	//}
	//
	////First gets Message size in bytes then gets Message in bytes itself.
	//private byte[] ReceiveMessage()
	//{
	//	int byteCountToRead = BitConverter.ToInt32(ReadBytes(8), 0);
	//	return ReadBytes(byteCountToRead);
	//}
	//
	////Starts listening to Message with ReceiveMessage(). When finished listening returns message.
	//private string ReceiveString(Encoding pEncoding)
	//{
	//	return pEncoding.GetString(ReceiveMessage());
	//}
	//
	////Starts sending Message size then sends Message itself
	//private void SendMessage(byte[] pMessage)
	//{
	//	stream.Write(BitConverter.GetBytes(pMessage.Length), 0, 4);
	//	stream.Write(pMessage, 0, pMessage.Length);
	//}
	//
	////Starts sending a string Message 
	//private void SendString(string pMessage, Encoding pEncoding)
	//{
	//	SendMessage(pEncoding.GetBytes(pMessage));
	//}

	/// <summary>
	/// Sends a custom request to the server
	/// </summary>
	/// <param name="req">Request type</param>
	private void SendRequest(CustomCommands.AbstractPackage req)
	{
		try
		{
			BinaryFormatter formatter = new BinaryFormatter();
			Debug.Log(req.ToString());
			formatter.Serialize(stream, req);
		}
		catch (SerializationException e)
		{
			Console.WriteLine("Failed to serialize. Reason: " + e.Message);
			throw;
		}

	}

	public void SendDoorUpdate(Door door)
	{
		SendRequest(new CustomCommands.Update.DoorUpdate(door.Id, door.GetDoorState().ToString()));
	}
	//NOT USED! TO BE DELETED
	//private void ReadResponse(CustomCommands.AbstractPackage response)
	//{
	//	if (response is CustomCommands.NotImplementedMessage){
	//		CustomCommands.NotImplementedMessage NIM = response as CustomCommands.NotImplementedMessage;
	//		_chatBoxScreen.AddChatLine(NIM.message);
	//	}
	//	if (response is CustomCommands.SendMinimapUpdate){
	//		//Debug.Log("Updating Minimap...");
	//		//CustomCommands.SendMinimapUpdate MU = response as CustomCommands.SendMinimapUpdate;
	//		//Texture2D tex = new Texture2D(2, 2);
	//		//tex.LoadImage(MU.bytes);
	//		//_minimap.GetComponent<Renderer>().material.mainTexture = tex;
	//		//Debug.Log("Minimap Updated");
	//		_chatBoxScreen.AddChatLine("Minimap Update");
	//	}
	//}

	//Exit Methods
	private void OnProcessExit(object sender, EventArgs e)
	{
		SendRequest(new CustomCommands.NotImplementedMessage());
	}

	private void OnApplicationQuit()
	{
		SendRequest(new CustomCommands.NotImplementedMessage());
	}
}

