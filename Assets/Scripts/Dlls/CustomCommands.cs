using UnityEngine;
using System.Collections;

namespace CustomCommands
{
	/// <summary>
	/// Update Packages
	/// </summary>
	namespace Update
	{
		[System.Serializable]
		public class DoorUpdate : AbstractPackage
		{
			public int ID;
			public string state;

			public DoorUpdate(int nID, string nState)
			{
				ID = nID;
				state = nState;
			}
		}

		[System.Serializable]
		public class FireWallUpdate : AbstractPackage
		{
			public int ID;
			public bool destroyed;

			public FireWallUpdate(int nID, bool nDestroyed)
			{
				ID = nID;
				destroyed = nDestroyed;
			}
		}

		[System.Serializable]
		public class KeyCardUpdate : AbstractPackage
		{
			public int ID;
			public bool collected;

			public KeyCardUpdate(int nID, bool nCollected)
			{
				ID = nID;
				collected = nCollected;
			}
		}

		[System.Serializable]
		public class MinimapUpdate : AbstractPackage
		{
			public byte[] bytes;
			public MinimapUpdate(byte[] nBytes)
			{
				bytes = nBytes;
			}
		}

		[System.Serializable]
		public class PlayerPositionUpdate : AbstractPackage
		{
			public float x;
			public float z;
			public float rotation;

			public PlayerPositionUpdate(Vector3 pos,float nRotation)
			{
				x = pos.x;
				z = pos.z;
				rotation = nRotation;
			}
		}
	}

	/// <summary>
	/// Object Creation
	/// </summary>
	namespace Creation
	{
		[System.Serializable]
		public class DoorCreation : AbstractPackage
		{
			public int ID;
			public float x;
			public float z;
			public float rotationY;
			public string state;

			public DoorCreation(int nID, float nX, float nZ, float nRotationY, string nState)
			{
				ID = nID;
				x = nX;
				z = nZ;
				rotationY = nRotationY;
				state = nState;
			}
		}

		[System.Serializable]
		public class FireWallCreation : AbstractPackage
		{
			public int ID;
			public float x;
			public float z;
			public bool state;
			public int[] doorIDs;

			public FireWallCreation(int nID, float nX, float nZ, bool nState, int[] nDoorIDs)
			{
				ID = nID;
				x = nX;
				z = nZ;
				state = nState;
				doorIDs = nDoorIDs;
			}
		}

		[System.Serializable]
		public class KeyCardCreation : AbstractPackage
		{
			public int ID;
			public float x;
			public float z;
			public bool collected;

			public KeyCardCreation(int nID, float nX, float nZ, bool nCollected)
			{
				ID = nID;
				x = nX;
				z = nZ;
				collected = nCollected;
			}
		}
	}

		[System.Serializable]
		public abstract class AbstractPackage{}

		[System.Serializable]
		public class NotImplementedMessage : AbstractPackage
		{
			public string message = "Not implemented";
		}


	/// <summary>
	/// OLD. Dont use. To be deleted
	/// </summary>
		[System.Serializable]
		public class MinimapUpdateRequest : AbstractPackage
		{

		}

		[System.Serializable]
		public class PlayerPositionUpdateRequest : AbstractPackage
		{

		}
}
