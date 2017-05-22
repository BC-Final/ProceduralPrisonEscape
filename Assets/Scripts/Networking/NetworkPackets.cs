using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkPacket {
	[System.Serializable]
	public abstract class AbstractPacket { }

	namespace Create {
		[System.Serializable]
		public class Fusebox : AbstractPacket {
			public int Id;
			public float PosX, PosY;
			public bool Charged, Used;

			public Fusebox(int pId, float pPosX, float pPosY, bool pUsed, bool pPrimed) {
				Id = pId; PosX = pPosX; PosY = pPosY; Used = pUsed; Charged = pPrimed;
			}
		}

		[System.Serializable]
		public class SecurityStation : AbstractPacket {
			public int Id;
			public float PosX, PosY;

			public SecurityStation(int pId, float pPosX, float pPosY) { Id = pId; PosX = pPosX; PosY = pPosY; }
		}

		[System.Serializable]
		public class PhaserAmmoIcon : AbstractPacket {
			public int Id;
			public float posX;
			public float posY;

			public PhaserAmmoIcon(int pId, Vector3 pPos) {
				Id = pId;
				posX = pPos.x;
				posY = pPos.z;
			}
		}

		[System.Serializable]
		public class MachineGunAmmoIcon : AbstractPacket {
			public int Id;
			public float posX;
			public float posY;

			public MachineGunAmmoIcon(int pId, Vector3 pPos) {
				Id = pId;
				posX = pPos.x;
				posY = pPos.z;
			}
		}

		[System.Serializable]
		public class ShotgunAmmoIcon : AbstractPacket {
			public int Id;
			public float posX;
			public float posY;

			public ShotgunAmmoIcon(int pId, Vector3 pPos) {
				Id = pId;
				posX = pPos.x;
				posY = pPos.z;
			}
		}

		[System.Serializable]
		public class HealthKitIcon : AbstractPacket {
			public int Id;
			public float posX;
			public float posY;

			public HealthKitIcon(int pId, Vector3 pPos) {
				Id = pId;
				posX = pPos.x;
				posY = pPos.z;
			}
		}

		[System.Serializable]
		public class LaserShot : AbstractPacket {
			public float startX;
			public float startZ;
			public float targetX;
			public float targetZ;

			public LaserShot(Vector3 startPos, Vector3 endPos) {
				startX = startPos.x;
				startZ = startPos.z;
				targetX = endPos.x;
				targetZ = endPos.z;
			}

		}
		[System.Serializable]
		public class KeyCard : AbstractPacket {
			public int Id;
			public int[] intArray;
			public float posX;
			public float posY;
			public float colorR;
			public float colorG;
			public float colorB;

			public KeyCard(int pID, int[] pIntArray, Vector3 pPos, float pColorR, float pColorG, float pColorB) {
				Id = pID;
				intArray = pIntArray;
				posX = pPos.x;
				posY = pPos.z;
				colorR = pColorR;
				colorG = pColorG;
				colorB = pColorB;
			}

		}
		[System.Serializable]
		public class DecodeAddon : AbstractPacket {
			public int DoorId;
			public string CodeString;

			public DecodeAddon(int pDoorId, string pCodeString) {
				DoorId = pDoorId;
				CodeString = pCodeString;
			}
		}

        [System.Serializable]
        public class DuoButtonAddon : AbstractPacket
        {
            public int DoorId;

            public DuoButtonAddon(int pDoorId)
            {
                DoorId = pDoorId;
            }
        }

        [System.Serializable]
		public class CodeLockAddon : AbstractPacket {
			public int Id;
			public int DoorId;
			public string CodeString;

			public CodeLockAddon(int pId, int pDoorId, string pCodeString) {
				Id = pId;
				DoorId = pDoorId;
				CodeString = pCodeString;
			}
		}
		[System.Serializable]
		public class PushButton : AbstractPacket {
			public int Id;
			public float posX;
			public float posY;


			public PushButton(int pId, Vector3 pPos) {
				Id = pId;
				posX = pPos.x;
				posY = pPos.z;
			}
		}
	}

	namespace Update {
		[System.Serializable]
		public class ButtonPush : AbstractPacket {
			public int Id;

			public ButtonPush(int pID) {
				Id = pID;
			}
		}

		[System.Serializable]
		public class ButtonFeedback : AbstractPacket {
			public int Id;
			public float colorR;
			public float colorG;
			public float colorB;

			public ButtonFeedback(int pID, Color pColor) {
				Id = pID;
				colorR = pColor.r;
				colorG = pColor.g;
				colorB = pColor.b;
			}
		}

		[System.Serializable]
		public class Door : AbstractPacket {
			public int Id;
			public float PosX, PosY, Rot;
			public bool Open, Locked;

			public Door(int pId, float pPosX, float pPosY, float pRot, bool pOpen, bool pLocked) {
				Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Open = pOpen; Locked = pLocked;
			}

			public Door(int pId, bool pOpen, bool pLocked) {
				Id = pId; Open = pOpen; Locked = pLocked;
			}

			public Door(int pId, bool pOpen) {
				Id = pId; Open = pOpen;
			}
		}

		[System.Serializable]
		public class Fusebox : AbstractPacket {
			public int Id;
			public bool Charged;

			public Fusebox(int pId, bool pCharged) {
				Id = pId; Charged = pCharged;
			}
		}

		[System.Serializable]
		public class SectorDoor : AbstractPacket {
			public int Id;
			public float PosX, PosY, Rot;
			public bool Open, Locked;

			public SectorDoor(int pId, float pPosX, float pPosY, float pRot, bool pOpen, bool pLocked) {
				Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Open = pOpen; Locked = pLocked;
			}

			public SectorDoor(int pId, bool pOpen, bool pLocked) {
				Id = pId; Open = pOpen; Locked = pLocked;
			}

			public SectorDoor(int pId, bool pOpen) {
				Id = pId; Open = pOpen;
			}
		}

		[System.Serializable]
		public class Player : AbstractPacket {
			public int Id;
			public float PosX, PosY, Rot;
			public float Health;

			public Player(int pId, float pPosX, float pPosY, float pRot, float pHealth) {
				Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Health = pHealth;
			}
		}

		[System.Serializable]
		public class Grenade : AbstractPacket {
			public int Id;
			public float PosX, PosY;

			public Grenade(int pId, float pPosX, float pPosY) {
				Id = pId; PosX = pPosX; PosY = pPosY;
			}
		}

		[System.Serializable]
		public class Module : AbstractPacket {
			public int Id;
			public float PosX, PosY, Rot;
			public bool Solved;

			public Module(int pId, float pPosX, float pPosY, float pRot, bool pSolved) {
				Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Solved = pSolved;
			}

			public Module(int pId, bool pSolved) {
				Id = pId; Solved = pSolved;
			}
		}

		[System.Serializable]
		public class Objective : AbstractPacket {
			public int Id;
			public float PosX, PosY;
			public bool Finished;

			public Objective(int pId, float pPosX, float pPosY) {
				Id = pId; PosX = pPosX; PosY = pPosY;
			}

			public Objective(int pId, bool pFinished) {
				Id = pId; Finished = pFinished;
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

			public Drone(int pId, float pPosX, float pPosY, float pRot, float pHealth, EnemyState pState) {
				Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Health = pHealth; State = pState;
			}

			public Drone(int pId, EnemyState pState) {
				Id = pId; State = pState;
			}
		}

		[System.Serializable]
		public class Turret : AbstractPacket {
			public int Id;
			public float PosX, PosY, Rot;
			public EnemyState State;

			public Turret(int pId, float pPosX, float pPosY, float pRot, EnemyState pState) {
				Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; State = pState;
			}

			public Turret(int pId, EnemyState pState) {
				Id = pId; State = pState;
			}
		}

		[System.Serializable]
		public class Camera : AbstractPacket {
			public int Id;
			public float PosX, PosY, Rot;
			public EnemyState State;

			public Camera(int pId, float pPosX, float pPosY, float pRot, EnemyState pState) {
				Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; State = pState;
			}

			public Camera(int pId, EnemyState pState) {
				Id = pId; State = pState;
			}
		}

		[System.Serializable]
		public class Pipe : AbstractPacket {
			public int Id;
			public float PosX, PosY, Rot;
			public bool Exploded;
			public bool ChargedExplosion;

			public Pipe(int pId, float pPosX, float pPosY, float pRot, bool pExploded) {
				Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Exploded = pExploded;
			}

			public Pipe(int pId, bool pChargedExplosion) {
				Id = pId; ChargedExplosion = pChargedExplosion;
			}
		}

		[System.Serializable]
		public class Minimap : AbstractPacket {
			public byte[] bytes;
			public Minimap(byte[] nBytes) {
				bytes = nBytes;
			}
		}

		[System.Serializable]
		public class Icon : AbstractPacket {
			public int Id;
			public bool used;
			public Icon(int pID, bool pUsed) {
				Id = pID;
				used = pUsed;
			}
		}

		[System.Serializable]
		public class CodeLockCode : AbstractPacket {
			public int Id;
			public string codeString;

			public CodeLockCode(int pID, string pCodeString) {
				Id = pID;
				codeString = pCodeString;
			}
		}
		[System.Serializable]
		public class DisableDoor : AbstractPacket {
			public int Id;

			public DisableDoor(int pID) {
				Id = pID;
			}
		}

		[System.Serializable]
		public class Light : AbstractPacket {
			public int Id;
			public bool IsOn;

			public Light(int pId, bool pIsOn) {
				Id = pId;
				IsOn = pIsOn;
			}
		}

		[System.Serializable]
		public class SecurityStation : AbstractPacket {
			public int Id;

			public SecurityStation(int pId) { Id = pId; }
		}
	}

	namespace States {
		[System.Serializable]
		public class AlarmState : AbstractPacket {
			public bool AlarmIsOn;

			public AlarmState(bool pAlarmIsOn) {
				AlarmIsOn = pAlarmIsOn;
			}
		}
	}

	namespace Messages {
		[System.Serializable]
		public class DisconnectRequest : AbstractPacket { }

		[System.Serializable]
		public class CreationEnd : AbstractPacket { }
	}
}
