using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkPacket {
	[System.Serializable]
	public abstract class AbstractPacket { }

	namespace Creation {
		[System.Serializable]
		public class DoorCreation : AbstractPacket {
			public float PosX, PosY;
			public float Rot;
			public bool Open;
			public bool Locked;

			public DoorCreation (float pPosX, float pPosY, float pRot, bool pOpen, bool pLocked) {
				PosX = pPosX; PosY = pPosY; Rot = pRot; Open = pOpen; Locked = pLocked;
			}
		}
	}

	namespace Update {
		[System.Serializable]
		public class DoorUpdate : AbstractPacket {
			public bool Open;
			public bool Locked;

			public DoorUpdate (bool pOpen, bool pLocked) {
				Open = pOpen; Locked = pLocked;
			}
		}
	}

	namespace Message {

	}
}
