﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkPacket {
	[System.Serializable]
	public abstract class AbstractPacket {
		public bool isLatePacket = false;
		abstract public void Invoke();
	}

	namespace GameObjects
	{
		namespace Lasergate
		{
			[System.Serializable]
			public class Creation : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public bool Active;

				public Creation(int pId, float pPosX, float pPosY, float pRot, bool pActive)
				{
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Active = pActive;
				}

				override public void Invoke()
				{
					LasergateMapIcon.ProcessPacket(this);
				}
			}
			[System.Serializable]
			public class hUpdate : AbstractPacket
			{
				public int Id;
				public bool Active;

				public hUpdate(int pId, bool pActive)
				{
					Id = pId; Active = pActive;
				}

				override public void Invoke()
				{
					ShooterLasergate.ProcessPacket(this);
				}
			}
			[System.Serializable]
			public class sUpdate : AbstractPacket
			{
				public int Id;
				public bool Active;

				public sUpdate(int pId, bool pActive)
				{
					Id = pId; Active = pActive;
				}

				override public void Invoke()
				{
					LasergateMapIcon.ProcessPacket(this);
				}
			}
		}
		namespace Fusebox
		{
			public class Creation : AbstractPacket
			{
				public int Id;
				public float PosX, PosY;
				public bool Charged, Used;

				public Creation(int pId, float pPosX, float pPosY, bool pUsed, bool pPrimed)
				{
					Id = pId; PosX = pPosX; PosY = pPosY; Used = pUsed; Charged = pPrimed;
				}
				override public void Invoke()
				{
					FuseboxMapIcon.ProcessPacket(this);
				}
			}
			[System.Serializable]
			public class Update : AbstractPacket
			{
				public int Id;
				public bool Charged;

				public Update(int pId, bool pCharged)
				{
					Id = pId; Charged = pCharged;
				}
				override public void Invoke()
				{
					FuseboxMapIcon.ProcessPacket(this);
				}
			}

		}
		namespace Button
		{
			[System.Serializable]
			public class Creation : AbstractPacket
			{
				public int Id;
				public float posX;
				public float posY;

				public Creation(int pId, Vector3 pPos)
				{
					Id = pId;
					posX = pPos.x;
					posY = pPos.z;
				}
				override public void Invoke()
				{
					PushButtonMapIcon.ProcessPacket(this);
				}
			}
			public class hUpdate : AbstractPacket
			{
				public int Id;

				public hUpdate(int pID)
				{
					Id = pID;
				}
				override public void Invoke()
				{
					ButtonTerminal.ProccessPacket(this);
				}
			}
			[System.Serializable]
			public class sUpdate : AbstractPacket
			{
				public int Id;
				public float colorR;
				public float colorG;
				public float colorB;
				public int buttonNumber;


				public sUpdate(int pID, Color pColor, int pButtonNumber = 0)
				{
					Id = pID;
					colorR = pColor.r;
					colorG = pColor.g;
					colorB = pColor.b;
					buttonNumber = pButtonNumber;
				}
				override public void Invoke()
				{
					PushButtonMapIcon.ProcessPacket(this);
				}
			}
		}
		namespace Door
		{
			[System.Serializable]
			public class Creation : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public bool Open, Locked;

				public Creation(int pId, float pPosX, float pPosY, float pRot, bool pOpen, bool pLocked)
				{
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Open = pOpen; Locked = pLocked;
				}

				public override void Invoke()
				{
					DoorMapIcon.ProcessPacket(this);
				}
			}

			public class hUpdate : AbstractPacket
			{
				public int Id;
				public bool Open, Locked;

				public hUpdate(int pId, bool pOpen, bool pLocked)
				{
					Id = pId; Open = pOpen; Locked = pLocked;
				}

				public override void Invoke()
				{
					ShooterDoor.ProcessPacket(this);
				}
			}

			public class sUpdate : AbstractPacket
			{
				public int Id;
				public bool Open, Locked;

				public sUpdate(int pId, bool pOpen, bool pLocked)
				{
					Id = pId; Open = pOpen; Locked = pLocked;
				}

				public override void Invoke()
				{
					DoorMapIcon.ProcessPacket(this);
				}
			}

		
			namespace Addons {
				[System.Serializable]
				public class AddDecodeAddon : AbstractPacket
				{
					public int DoorId;
					public string CodeString;

					public AddDecodeAddon(int pDoorId, string pCodeString)
					{
						DoorId = pDoorId;
						CodeString = pCodeString;
					}

					public override void Invoke()
					{
						DoorMapIcon.AddAddon(this);
					}
				}

				[System.Serializable]
				public class AddDuoButtonAddon : AbstractPacket
				{
					public int DoorId;

					public AddDuoButtonAddon(int pDoorId)
					{
						DoorId = pDoorId;
					}

					public override void Invoke()
					{
						DoorMapIcon.AddAddon(this);
					}
				}

				[System.Serializable]
				public class AddCodeLockAddon  : AbstractPacket
				{
					public int Id;
					public int DoorId;
					public string CodeString;

					public AddCodeLockAddon(int pId, int pDoorId, string pCodeString)
					{
						Id = pId;
						DoorId = pDoorId;
						CodeString = pCodeString;
					}

					public override void Invoke()
					{
						DoorMapIcon.AddAddon(this);
					}
				}

				[System.Serializable]
				public class hCodeLockUpdate : AbstractPacket
				{
					public int Id;
					public string codeString;

					public hCodeLockUpdate(int pID, string pCodeString)
					{
						Id = pID;
						codeString = pCodeString;
					}

					public override void Invoke()
					{
						CodeLock.ProcessPacket(this);
					}
				}
			}
			namespace Other
			{
				/// <summary>
				/// Deactivates the door permanently.
				/// </summary>
				[System.Serializable]
				public class DeactivateDoor : AbstractPacket
				{
					public int Id;

					public DeactivateDoor(int pID)
					{
						Id = pID;
					}

					public override void Invoke()
					{
						DoorMapIcon.ProcessPacket(this);
					}
				}
				/// <summary>
				/// Deactivates the door until renabled.
				/// </summary>
				[System.Serializable]
				public class DisableDoorOptions : AbstractPacket
				{
					public int Id;

					public DisableDoorOptions(int pID)
					{
						Id = pID;
					}

					public override void Invoke()
					{
						DoorMapIcon.ProcessPacket(this);
					}
				}
				/// <summary>
				/// Enables the door until renabled.
				/// </summary>
				[System.Serializable]
				public class EnableDoorOptions : AbstractPacket
				{
					public int Id;

					public EnableDoorOptions(int pID)
					{
						Id = pID;
					}

					public override void Invoke()
					{
						DoorMapIcon.ProcessPacket(this);
					}
				}
			}
			
		}
		namespace Firewall
		{
			[System.Serializable]
			public class Creation : AbstractPacket
			{
				public int Id;
				public float PosX, PosY;
				public bool Active;

				public Creation(int pId, float pPosX, float pPosY, bool active)
				{
					Id = pId;
					PosX = pPosX;
					PosY = pPosY;
					Active = active;
				}

				public override void Invoke()
				{
					FirewallMapIcon.ProcessPacket(this);
				}
			}

			[System.Serializable]
			public class sUpdate : AbstractPacket
			{
				public int Id;
				public bool Active;

				public sUpdate(int pId, bool pActive)
				{
					Id = pId; Active = pActive;
				}

				public override void Invoke()
				{
					FirewallMapIcon.ProcessPacket(this);
				}
			}
		}
		namespace SecurityStation
		{
			[System.Serializable]
			public class Creation : AbstractPacket
			{
				public int Id;
				public float PosX, PosY;

				public Creation(int pId, float pPosX, float pPosY) { Id = pId; PosX = pPosX; PosY = pPosY; }

				public override void Invoke()
				{
					SecuritystationMapIcon.ProcessPacket(this);
				}
			}

			[System.Serializable]
			public class sUpdate : AbstractPacket
			{
				public int Id, state;

				public sUpdate(int pID, int pState)
				{
					Id = pID;
					state = pState;
				}

				public override void Invoke()
				{
					SecuritystationMapIcon.ProcessPacket(this);
				}

				
			}
			[System.Serializable]
			public class hUpdate : AbstractPacket
			{
				public int Id;

				public hUpdate(int pId) { Id = pId; }

				public override void Invoke()
				{
					ShooterSecurityStation.ProcessPacket(this);
				}
			}

		}
		namespace PickUpIcon
		{
			[System.Serializable]
			public class PhaserAmmoCreation : AbstractPacket
			{
				public int Id;
				public float posX;
				public float posY;

				public PhaserAmmoCreation (int pId, Vector3 pPos)
				{
					Id = pId;
					posX = pPos.x;
					posY = pPos.z;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
			[System.Serializable]
			public class MachineGunAmmoCreation : AbstractPacket
			{
				public int Id;
				public float posX;
				public float posY;

				public MachineGunAmmoCreation(int pId, Vector3 pPos)
				{
					Id = pId;
					posX = pPos.x;
					posY = pPos.z;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
			[System.Serializable]
			public class ShotgunAmmoCreation : AbstractPacket
			{
				public int Id;
				public float posX;
				public float posY;

				public ShotgunAmmoCreation(int pId, Vector3 pPos)
				{
					Id = pId;
					posX = pPos.x;
					posY = pPos.z;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
			[System.Serializable]
			public class HealthKitCreation : AbstractPacket
			{
				public int Id;
				public float posX;
				public float posY;

				public HealthKitCreation(int pId, Vector3 pPos)
				{
					Id = pId;
					posX = pPos.x;
					posY = pPos.z;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
			[System.Serializable]
			public class sIconUpdate : AbstractPacket
			{
				public int Id;
				public bool used;
				public sIconUpdate(int pID, bool pUsed)
				{
					Id = pID;
					used = pUsed;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
		}
		namespace Lasershot
		{
			[System.Serializable]
			public class Creation : AbstractPacket
			{
				public float startX;
				public float startZ;
				public float targetX;
				public float targetZ;

				public Creation(Vector3 startPos, Vector3 endPos)
				{
					startX = startPos.x;
					startZ = startPos.z;
					targetX = endPos.x;
					targetZ = endPos.z;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
		}
		namespace Keycard
		{
			[System.Serializable]
			public class Creation : AbstractPacket
			{
				public int Id;
				public int[] intArray;
				public float posX;
				public float posY;
				public float colorR;
				public float colorG;
				public float colorB;

				public Creation(int pID, int[] pIntArray, Vector3 pPos, float pColorR, float pColorG, float pColorB)
				{
					Id = pID;
					intArray = pIntArray;
					posX = pPos.x;
					posY = pPos.z;
					colorR = pColorR;
					colorG = pColorG;
					colorB = pColorB;

					isLatePacket = true;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}

			[System.Serializable]
			public class sCollected : AbstractPacket
			{
				public float colorR;
				public float colorG;
				public float colorB;

				public sCollected(Color pColor)
				{
					colorR = pColor.r;
					colorG = pColor.g;
					colorB = pColor.b;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
		}
		namespace SectorDoor
		{
			[System.Serializable]
			public class Creation : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public bool Open, Locked;

				public Creation(int pId, float pPosX, float pPosY, float pRot, bool pOpen, bool pLocked)
				{
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Open = pOpen; Locked = pLocked;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}

			[System.Serializable]
			public class sUpdate : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public bool Open, Locked;

				public sUpdate(int pId, bool pOpen, bool pLocked)
				{
					Id = pId; Open = pOpen; Locked = pLocked;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}

			[System.Serializable]
			public class hUpdate : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public bool Open, Locked;

				public hUpdate(int pId, bool pOpen, bool pLocked)
				{
					Id = pId; Open = pOpen; Locked = pLocked;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}

		}
		namespace Player
		{
			[System.Serializable]
			public class sUpdate : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public float Health;

				public sUpdate(int pId, float pPosX, float pPosY, float pRot, float pHealth)
				{
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Health = pHealth;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
		}
		namespace Grenade
		{
			[System.Serializable]
			public class Update : AbstractPacket
			{
				public int Id;
				public float PosX, PosY;

				public Update(int pId, float pPosX, float pPosY)
				{
					Id = pId; PosX = pPosX; PosY = pPosY;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
		}
		namespace Module
		{
			[System.Serializable]
			public class Creation : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public bool Solved;

				public Creation(int pId, float pPosX, float pPosY, float pRot, bool pSolved)
				{
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Solved = pSolved;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
			[System.Serializable]
			public class sUpdate : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public bool Solved;

				public sUpdate(int pId, bool pSolved)
				{
					Id = pId; Solved = pSolved;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
			[System.Serializable]
			public class hUpdate : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public bool Solved;

				public hUpdate(int pId, bool pSolved)
				{
					Id = pId; Solved = pSolved;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
		}
		namespace Objective
		{
			[System.Serializable]
			public class Creation : AbstractPacket
			{
				public int Id;
				public float PosX, PosY;
				public bool Finished;

				public Creation(int pId, float pPosX, float pPosY)
				{
					Id = pId; PosX = pPosX; PosY = pPosY;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
			[System.Serializable]
			public class sUpdate : AbstractPacket
			{
				public int Id;
				public float PosX, PosY;
				public bool Finished;

				public sUpdate(int pId, bool pFinished)
				{
					Id = pId; Finished = pFinished;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
			[System.Serializable]
			public class hUpdate : AbstractPacket
			{
				public int Id;
				public float PosX, PosY;
				public bool Finished;

				public hUpdate(int pId, bool pFinished)
				{
					Id = pId; Finished = pFinished;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
		}
		namespace Drone
		{
			[System.Serializable]
			public class sUpdate : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public float Health;
				public EnemyState State;
				//public bool Shielded;
				//TODO Include info like weapon and armor and stuff

				public sUpdate(int pId, float pPosX, float pPosY, float pRot, float pHealth, EnemyState pState)
				{
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Health = pHealth; State = pState;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
			[System.Serializable]
			public class sUpdateShort : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public float Health;
				public EnemyState State;
				//public bool Shielded;
				//TODO Include info like weapon and armor and stuff

				public sUpdateShort(int pId, EnemyState pState)
				{
					Id = pId; State = pState;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
		}
		namespace Turret
		{
			[System.Serializable]
			public class Creation : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public EnemyState State;

				public Creation(int pId, float pPosX, float pPosY, float pRot, EnemyState pState)
				{
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; State = pState;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
			[System.Serializable]
			public class sUpdate : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public EnemyState State;

				public sUpdate(int pId, EnemyState pState)
				{
					Id = pId; State = pState;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
			[System.Serializable]
			public class hUpdate : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public EnemyState State;

				public hUpdate(int pId, EnemyState pState)
				{
					Id = pId; State = pState;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}

		}
		namespace Camera
		{
			[System.Serializable]
			public class Creation : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public EnemyState State;

				public Creation(int pId, float pPosX, float pPosY, float pRot, EnemyState pState)
				{
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; State = pState;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
			[System.Serializable]
			public class sUpdate : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public EnemyState State;

				public sUpdate(int pId, EnemyState pState)
				{
					Id = pId; State = pState;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
			[System.Serializable]
			public class hUpdate : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public EnemyState State;

				public hUpdate(int pId, EnemyState pState)
				{
					Id = pId; State = pState;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
		}
		namespace Pipe
		{
			[System.Serializable]
			public class Creation : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public bool Exploded;
				public bool ChargedExplosion;

				public Creation(int pId, float pPosX, float pPosY, float pRot, bool pExploded)
				{
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Exploded = pExploded;
				}

				public Pipe(int pId, bool pChargedExplosion)
				{
					Id = pId; ChargedExplosion = pChargedExplosion;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
			[System.Serializable]
			public class sUpdate : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public bool Exploded;
				public bool ChargedExplosion;


				public sUpdate(int pId, bool pChargedExplosion)
				{
					Id = pId; ChargedExplosion = pChargedExplosion;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
			[System.Serializable]
			public class hUpdate : AbstractPacket
			{
				public int Id;
				public float PosX, PosY, Rot;
				public bool Exploded;
				public bool ChargedExplosion;


				public hUpdate(int pId, bool pChargedExplosion)
				{
					Id = pId; ChargedExplosion = pChargedExplosion;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}

		}
		namespace Minimap
		{
			[System.Serializable]
			public class Minimap : AbstractPacket
			{
				public byte[] bytes;
				public Minimap(byte[] nBytes)
				{
					bytes = nBytes;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}

		}
		namespace Light
		{
			[System.Serializable]
			public class sUpdate : AbstractPacket
			{
				public int Id;
				public bool IsOn;

				public sUpdate(int pId, bool pIsOn)
				{
					Id = pId;
					IsOn = pIsOn;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
			[System.Serializable]
			public class pUpdate : AbstractPacket
			{
				public int Id;
				public bool IsOn;

				public pUpdate(int pId, bool pIsOn)
				{
					Id = pId;
					IsOn = pIsOn;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}

		}
		namespace HackerMinimapCamera
		{
			[System.Serializable]
			public class MoveCameraTowardsLocation : AbstractPacket
			{
				public float posX, posY;

				public MoveCameraTowardsLocation(Vector3 pPos)
				{
					posX = pPos.x;
					posY = pPos.z;
				}

				public override void Invoke()
				{
					throw new NotImplementedException();
				}
			}
		}
	}

	namespace States {
		[System.Serializable]
		public class AlarmState : AbstractPacket {
			public bool AlarmIsOn;

			public AlarmState(bool pAlarmIsOn) {
				AlarmIsOn = pAlarmIsOn;
			}
			public override void Invoke()
			{
				throw new NotImplementedException();
			}
		}

	}

	namespace Messages {
		[System.Serializable]
		public class DisconnectRequest : AbstractPacket {
			public override void Invoke()
			{
				throw new NotImplementedException();
			}
		}

		[System.Serializable]
		public class CreationEnd : AbstractPacket {
			public override void Invoke()
			{
				throw new NotImplementedException();
			}
		}

		
	}
}
