using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkPacket {
	[System.Serializable]
	public abstract class AbstractPacket { }

    namespace Create
    {
        [System.Serializable]
        public class LaserShot : AbstractPacket
        {
            public float startX;
            public float startZ;
            public float targetX;
            public float targetZ;

            public LaserShot(Vector3 startPos, Vector3 endPos)
            {
                startX = startPos.x;
                startZ = startPos.z;
                targetX = endPos.x;
                targetZ = endPos.z;
            }

        }
        [System.Serializable]
        public class KeyCard : AbstractPacket
        {
            public int ID;
            public int[] intArray;
            public Vector3 pos;
            public float colorR;
            public float colorG;
            public float colorB;

            public KeyCard(int pID, int[] pIntArray, Vector3 pPos, float pColorR, float pColorG, float pColorB)
            {
                ID = pID;
                intArray = pIntArray;
                pos = pPos;
                colorR = pColorR;
                colorG = pColorG;
                colorB = pColorB;
            }

        }

    }
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
			public EnemyState State;

			public Turret (int pId, float pPosX, float pPosY, float pRot, EnemyState pState) {
				Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; State = pState;
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
			public bool ChargedExplosion;

			public Pipe (int pId, float pPosX, float pPosY, float pRot, bool pExploded) {
				Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Exploded = pExploded;
			}

			public Pipe (int pId, bool pChargedExplosion) {
				Id = pId; ChargedExplosion = pChargedExplosion;
			}
		}

		[System.Serializable]
		public class Minimap : AbstractPacket {
			public byte[] bytes;
			public Minimap (byte[] nBytes) {
				bytes = nBytes;
			}
		}

        [System.Serializable]
        public class Icon : AbstractPacket
        {
            public int ID;
            public bool active;
            public Icon(int pID, bool pActive)
            {
                ID = pID;
                active = pActive;
            }
        }
    }

	namespace Message {
		[System.Serializable]
		public class DisconnectRequest : AbstractPacket { }
	}
}
