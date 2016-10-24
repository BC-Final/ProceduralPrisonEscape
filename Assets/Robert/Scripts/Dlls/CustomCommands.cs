using UnityEngine;
using System.Collections;

namespace CustomCommands
{
	[System.Serializable]
	public abstract class AbstractPackage
	{
	}

	[System.Serializable]
	public class NotImplementedMessage : AbstractPackage
	{
		public string message = "Not implemented";
	}

	[System.Serializable]
	public class DoorUpdate : AbstractPackage
	{
		public int ID;
		public float x;
		public float z;
		public float rotationY;
		public string state;

		public DoorUpdate(int nID, float nX, float nZ, float nRotationY, string nState)
		{
			ID = nID;
			x = nX;
			z = nZ;
			rotationY = nRotationY;
			state = nState;
		}
	}

	[System.Serializable]
	public class DoorChangeState : AbstractPackage
	{
		public int ID;
		public string state;

		public DoorChangeState(int nID, string nState)
		{
			ID = nID;
			state = nState;
		}
	}

	[System.Serializable]
	public class SendMinimapUpdate : AbstractPackage
	{
		public byte[] bytes;
		public SendMinimapUpdate(byte[] nBytes)
		{
			bytes = nBytes;
		}
	}
	[System.Serializable]
	public class PlayerPositionUpdate : AbstractPackage
	{
		public float x;
		public float z;

		public PlayerPositionUpdate(Vector3 pos)
		{
			x = pos.x;
			z = pos.z;
		}
	}

	[System.Serializable]
	public class MinimapUpdateRequest : AbstractPackage
	{

	}

	[System.Serializable]
	public class PlayerPositionUpdateRequest : AbstractPackage
	{

	}
}
