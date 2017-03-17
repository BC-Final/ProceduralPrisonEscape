using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkPacket {
	[System.Serializable]
	public abstract class AbstractPacket { }

	namespace Update {
		[System.Serializable]
		public class Door : AbstractPacket {
			public int Id;
			public float PosX, PosY, Rot;
			public bool Open, Locked;
			public bool HasLocationData;

			public Door (int pId, float pPosX, float pPosY, float pRot, bool pOpen, bool pLocked) {
				Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Open = pOpen; Locked = pLocked;
			}

			public Door (int pId, bool pOpen, bool pLocked) {
				Id = pId; Open = pOpen; Locked = pLocked;
			}

			public Door (int pId, bool pOpen) {
				Id = pId; Open = pOpen;
			}
		}

		[System.Serializable]
		public class Player : AbstractPacket {
			public int Id;
			public float PosX, PosY, Rot;
			public float Health;

			public Player (int pId, float pPosX, float pPosY, float pRot, float pHealth) {
				Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Health = pHealth;
			}
		}

		[System.Serializable]
		public class Drone : AbstractPacket {
			public int Id;
			public float PosX, PosY, Rot;
			public float Health;
			public EnemyState State;
			//public bool Shielded;
			//TODO Include info like weapon and armor and stuff

			public Drone (int pId, float pPosX, float pPosY, float pRot, float pHealth, EnemyState pState) {
				Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Health = pHealth; State = pState;
			}

			public Drone (int pId, EnemyState pState) {
				Id = pId; State = pState;
			}
		}

		[System.Serializable]
		public class Turret : AbstractPacket {
			public int Id;
			public float PosX, PosY, Rot;
			public float Health;
			public EnemyState State;

			public Turret (int pId, float pPosX, float pPosY, float pRot, float pHealth, EnemyState pState) {
				Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Health = pHealth; State = pState;
			}

			public Turret (int pId, EnemyState pState) {
				Id = pId; State = pState;
			}
		}

		[System.Serializable]
		public class Camera : AbstractPacket {
			public int Id;
			public float PosX, PosY, Rot;
			public EnemyState State;

			public Camera (int pId, float pPosX, float pPosY, float pRot, EnemyState pState) {
				Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; State = pState;
			}

			public Camera (int pId, EnemyState pState) {
				Id = pId; State = pState;
			}
		}

		[System.Serializable]
		public class Pipe : AbstractPacket {
			public int Id;
			public float PosX, PosY, Rot;
			public bool Exploded;
			public PipeExplodeType ExplodeType;

			public Pipe (int pId, float pPosX, float pPosY, float pRot, bool pExploded) {
				Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Exploded = pExploded;
				ExplodeType = PipeExplodeType.None;
			}

			public Pipe (int pId, PipeExplodeType pExplodeType) {
				Id = pId; ExplodeType = pExplodeType;
			}
		}

		[System.Serializable]
		public class Minimap : AbstractPacket {
			public byte[] bytes;
			public Minimap (byte[] nBytes) {
				bytes = nBytes;
			}
		}
	}

	namespace Message {
		public class DisconnectRequest : AbstractPacket { }
	}
}
