﻿using UnityEngine;
using System.Collections;

namespace CustomCommands {

	/// <summary>
	/// Update Packages
	/// </summary>
	namespace Update {

		[System.Serializable]
		public class AlarmUpdate : AbstractPackage {
			public bool state;

			public AlarmUpdate (bool pState) {
				state = pState;
			}
		}

		//TODO Send this at beginning of game
		[System.Serializable]
		public class AlarmTimerUpdate : AbstractPackage {
			public float timer;

			public AlarmTimerUpdate (float nTimer) {
				timer = nTimer;
			}
		}

		[System.Serializable]
		public class DoorUpdate : AbstractPackage {
			public int ID;
			public int state;

			public DoorUpdate (int nID, int nState) {
				ID = nID;
				state = nState;
			}
		}

		[System.Serializable]
		public class FireWallUpdate : AbstractPackage {
			public int ID;
			public bool destroyed;

			public FireWallUpdate (int nID, bool nDestroyed) {
				ID = nID;
				destroyed = nDestroyed;
			}
		}

		[System.Serializable]
		public class MinimapUpdate : AbstractPackage {
			public byte[] bytes;
			public MinimapUpdate (byte[] nBytes) {
				bytes = nBytes;
			}
		}

		[System.Serializable]
		public class PlayerPositionUpdate : AbstractPackage {
			public float x;
			public float z;
			public float rotation;

			public PlayerPositionUpdate (Vector3 pos, float nRotation) {
				x = pos.x;
				z = pos.z;
				rotation = nRotation;
			}
		}
		[System.Serializable]
		public class DroneUpdate : AbstractPackage {
			public int id;
			public int hpPercent;
			public float x;
			public float z;
			public float rotation;

			public DroneUpdate (int nID, int nHpPercent, Vector3 pos, float nRotation) {
				id = nID;
				hpPercent = nHpPercent;
				x = pos.x;
				z = pos.z;
				rotation = nRotation;
			}
		}

		[System.Serializable]
		public class CameraUpdate : AbstractPackage {
			public int id;
			public float x;
			public float z;
			public float rotation;
			public bool seesPlayer;

			public CameraUpdate (int nID, Vector3 pos, float nRotation, bool pSeesPlayer) {
				id = nID;
				seesPlayer = pSeesPlayer;
				x = pos.x;
				z = pos.z;
				rotation = nRotation;
			}
		}

		[System.Serializable]
		public class DisableCamera : AbstractPackage {
			public int Id;

			public DisableCamera (int pId) {
				Id = pId;
			}
		}

		[System.Serializable]
		public class EnableCamera : AbstractPackage {
			public int Id;

			public EnableCamera (int pId) {
				Id = pId;
			}
		}

		[System.Serializable]
		public class DisableTurret : AbstractPackage {
			public int Id;

			public DisableTurret (int pId) {
				Id = pId;
			}
		}

		[System.Serializable]
		public class EnableTurret : AbstractPackage {
			public int Id;

			public EnableTurret (int pId) {
				Id = pId;
			}
		}

		[System.Serializable]
		public class TurretUpdate : AbstractPackage {
			public int id;
			public int hpPercent;
			public float x;
			public float z;
			public float rotation;

			public TurretUpdate (int nID, int nHpPercent, Vector3 pos, float nRotation) {
				id = nID;
				hpPercent = nHpPercent;
				x = pos.x;
				z = pos.z;
				rotation = nRotation;
			}
		}

		namespace Items {
			[System.Serializable]
			public class ItemUpdate : AbstractPackage {
				public int ID;
				public bool collected;
				public float x;
				public float z;

				public ItemUpdate (int nID, bool nCollected, Vector3 pos) {
					ID = nID;
					collected = nCollected;
					x = pos.x;
					z = pos.z;
				}
			}
		}
	}

	/// <summary>
	/// Object Creation
	/// </summary>
	namespace Creation {
		[System.Serializable]
		public class NodeCreation : AbstractPackage {
			public float X;
			public float Y;
			public int ID;
			public int NodeType;
			public int AsocID;
			public int[] ConnectionIds;

			public NodeCreation (float pX, float pY, int pId, int pNodeType, int pAsocId, int[] pConnectionIds) {
				X = pX;
				Y = pY;
				ID = pId;
				NodeType = pNodeType;
				AsocID = pAsocId;
				ConnectionIds = pConnectionIds;
			}
		}

		[System.Serializable]
		public class OnCreationEnd : AbstractPackage {
			public OnCreationEnd () { }
		}

		[System.Serializable]
		public class DoorCreation : AbstractPackage {
			public int ID;
			public float x;
			public float z;
			public float rotationY;
			public int state;
			public bool requireKeycard;

			public DoorCreation (int nID, float nX, float nZ, float nRotationY, int nState, bool reqKeycard) {
				ID = nID;
				x = nX;
				z = nZ;
				rotationY = nRotationY;
				state = nState;
				requireKeycard = reqKeycard;
			}
		}

		[System.Serializable]
		public class FireWallCreation : AbstractPackage {
			public int ID;
			public float x;
			public float z;
			public bool state;

			public FireWallCreation (int nID, float nX, float nZ, bool nState) {
				ID = nID;
				x = nX;
				z = nZ;
				state = nState;
			}
		}

		[System.Serializable]
		public class CameraCreation : AbstractPackage {
			public int Id;
			public float Rot;
			public float X;
			public float Z;
			public bool State;

			public CameraCreation (int pId, float pRot, float pX, float pZ, bool pState) {
				Id = pId;
				Rot = pRot;
				X = pX;
				Z = pZ;
				State = pState;
			}
		}

		[System.Serializable]
		public class DroneCreation : AbstractPackage {
			public int Id;
			public int HealthPercent;
			public float X;
			public float Z;
			public float Rot;

			public DroneCreation (int pId, int pHealthPercent, Vector3 pPos, float pRot) {
				Id = pId;
				HealthPercent = pHealthPercent;
				X = pPos.x;
				Z = pPos.z;
				Rot = pRot;
			}
		}

		[System.Serializable]
		public class TurretCreation : AbstractPackage {
			public int Id;
			public float X;
			public float Z;
			public float Rot;
			public int HealthPercent;

			public TurretCreation (int pId, float pX, float pZ, float pRot, int pHealthPercent) {
				Id = pId;
				X = pX;
				Z = pZ;
				Rot = pRot;
				HealthPercent = pHealthPercent;
			}
		}

		namespace Items {
			[System.Serializable]
			public class KeyCardCreation : AbstractPackage {
				public int ID;
				public float x;
				public float z;
				public int color;
				public int[] doorArray;

				public KeyCardCreation (int nID, int[] nDoorArray, float nX, float nZ, int nColor) {
					ID = nID;
					doorArray = nDoorArray;
					x = nX;
					z = nZ;
					color = nColor;
				}
			}

			[System.Serializable]
			public class AmmoPackCreation : AbstractPackage {
				public int ID;
				public float x;
				public float z;
				public bool collected;

				public AmmoPackCreation (int nID, float nX, float nZ, bool nCollected) {
					ID = nID;
					x = nX;
					z = nZ;
					collected = nCollected;
				}
			}

			[System.Serializable]
			public class HealthKitCreation : AbstractPackage {
				public int ID;
				public float x;
				public float z;
				public bool collected;

				public HealthKitCreation (int nID, float nX, float nZ, bool nCollected) {
					ID = nID;
					x = nX;
					z = nZ;
					collected = nCollected;
				}
			}

			[System.Serializable]
			public class DroneBeaconCreation : AbstractPackage {
				public int ID;
				public float x;
				public float z;
				public bool collected;

				public DroneBeaconCreation (int nID, float nX, float nZ, bool nCollected) {
					ID = nID;
					x = nX;
					z = nZ;
					collected = nCollected;
				}
			}
		}

		namespace Shots {
			[System.Serializable]
			public class LaserShotCreation : AbstractPackage {
				public float startX;
				public float startZ;
				public float targetX;
				public float targetZ;

				public LaserShotCreation (Vector3 startPos, Vector3 endPos) {
					startX = startPos.x;
					startZ = startPos.z;
					targetX = endPos.x;
					targetZ = endPos.z;
				}

			}
		}
	}

	namespace Communication {
		namespace Hacker {
			[System.Serializable]
			public class EnemyWaveDispatched : AbstractPackage {
			}
		}

		namespace Shooter {
			[System.Serializable]
			public class NeedHealth : AbstractPackage {
			}
			[System.Serializable]
			public class NeedHelp : AbstractPackage {
			}
			[System.Serializable]
			public class NeedAmmo : AbstractPackage {
			}
		}
	}

	[System.Serializable]
	public class RefuseConnectionPackage : AbstractPackage { }

	[System.Serializable]
	public class ServerShutdownPackage : AbstractPackage { }

	[System.Serializable]
	public abstract class AbstractPackage { }
}
