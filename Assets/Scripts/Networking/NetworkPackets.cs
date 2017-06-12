using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

namespace NetworkPacket {
	[MessagePackObject]
	public abstract class AbstractPacket {
		[Key(7)]
		public bool isLatePacket = false;

		public AbstractPacket() { }
		abstract public void Invoke();
	}

	namespace GameObjects {
		namespace Lasergate {
			[MessagePackObject]
			public class Creation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }
				[Key(3)]
				public float Rot { get; set; }
				[Key(4)]
				public bool Active { get; set; }

				public Creation(int pId, float pPosX, float pPosY, float pRot, bool pActive) {
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Active = pActive;
				}

				override public void Invoke() {
					LasergateMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class hUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public bool Active { get; set; }

				public hUpdate(int pId, bool pActive) {
					Id = pId; Active = pActive;
				}

				override public void Invoke() {
					ShooterLasergate.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class sUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public bool Active { get; set; }

				public sUpdate(int pId, bool pActive) {
					Id = pId; Active = pActive;
				}

				override public void Invoke() {
					LasergateMapIcon.ProcessPacket(this);
				}
			}
		}

		namespace Fusebox {
			[MessagePackObject]
			public class Creation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }
				[Key(3)]
				public bool Charged { get; set; }
				[Key(4)]
				public bool Used { get; set; }

				public Creation(int pId, float pPosX, float pPosY, bool pUsed, bool pPrimed) {
					Id = pId; PosX = pPosX; PosY = pPosY; Used = pUsed; Charged = pPrimed;
				}

				override public void Invoke() {
					FuseboxMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class hUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public bool Charged { get; set; }

				public hUpdate(int pId, bool pCharged) {
					Id = pId; Charged = pCharged;
				}

				override public void Invoke() {
					ShooterFusebox.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class sUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public int TargetId { get; set; }
				[Key(2)]
				public bool Used { get; set; }

				public sUpdate(int pId, bool pUsed) {
					Id = pId; Used = pUsed;
				}

				override public void Invoke() {
					FuseboxMapIcon.ProcessPacket(this);
				}
			}
		}
		namespace Button {
			[MessagePackObject]
			public class Creation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }

				public Creation(int pId, Vector3 pPos) {
					Id = pId;
					PosX = pPos.x;
					PosY = pPos.z;
				}

				override public void Invoke() {
					PushButtonMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class hUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }

				public hUpdate(int pID) {
					Id = pID;
				}

				override public void Invoke() {
					ButtonTerminal.ProccessPacket(this);
				}
			}
			[MessagePackObject]
			public class sUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float ColorR { get; set; }
				[Key(2)]
				public float ColorG { get; set; }
				[Key(3)]
				public float ColorB { get; set; }
				[Key(4)]
				public int ButtonNumber { get; set; }


				public sUpdate(int pID, Color pColor, int pButtonNumber = 0) {
					Id = pID;
					ColorR = pColor.r;
					ColorG = pColor.g;
					ColorB = pColor.b;
					ButtonNumber = pButtonNumber;
				}

				override public void Invoke() {
					PushButtonMapIcon.ProcessPacket(this);
				}
			}
		}
		namespace Door {
			[MessagePackObject]
			public class Creation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }
				[Key(3)]
				public float Rot { get; set; }
				[Key(4)]
				public bool Open { get; set; }
				[Key(5)]
				public bool Locked { get; set; }

				public Creation(int pId, float pPosX, float pPosY, float pRot, bool pOpen, bool pLocked) {
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Open = pOpen; Locked = pLocked;
				}

				public override void Invoke() {
					DoorMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class hUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public bool Open { get; set; }
				[Key(2)]
				public bool Locked { get; set; }

				public hUpdate(int pId, bool pOpen, bool pLocked) {
					Id = pId; Open = pOpen; Locked = pLocked;
				}

				public override void Invoke() {
					ShooterDoor.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class sUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public bool Open { get; set; }
				[Key(2)]
				public bool Locked { get; set; }

				public sUpdate(int pId, bool pOpen, bool pLocked) {
					Id = pId; Open = pOpen; Locked = pLocked;
				}

				public override void Invoke() {
					DoorMapIcon.ProcessPacket(this);
				}
			}

			namespace Addons {
				[MessagePackObject]
				public class AddDecodeAddon : AbstractPacket {
					[Key(0)]
					public int DoorId { get; set; }
					[Key(1)]
					public string CodeString { get; set; }

					public AddDecodeAddon(int pDoorId, string pCodeString) {
						DoorId = pDoorId;
						CodeString = pCodeString;

						isLatePacket = true;
					}

					public override void Invoke() {
						DoorMapIcon.AddAddon(this);
					}
				}

				[MessagePackObject]
				public class AddDuoButtonAddon : AbstractPacket {
					[Key(0)]
					public int DoorId { get; set; }

					public AddDuoButtonAddon(int pDoorId) {
						DoorId = pDoorId;

						isLatePacket = true;
					}

					public override void Invoke() {
						DoorMapIcon.AddAddon(this);
					}
				}

				[MessagePackObject]
				public class AddCodeLockAddon : AbstractPacket {
					[Key(0)]
					public int Id { get; set; }
					[Key(1)]
					public int DoorId { get; set; }
					[Key(2)]
					public string CodeString { get; set; }

					public AddCodeLockAddon(int pId, int pDoorId, string pCodeString) {
						Id = pId;
						DoorId = pDoorId;
						CodeString = pCodeString;

						isLatePacket = true;
					}

					public override void Invoke() {
						DoorMapIcon.AddAddon(this);
					}
				}

				[MessagePackObject]
				public class hCodeLockUpdate : AbstractPacket {
					[Key(0)]
					public int Id { get; set; }
					[Key(1)]
					public string codeString { get; set; }

					public hCodeLockUpdate(int pID, string pCodeString) {
						Id = pID;
						codeString = pCodeString;
					}

					public override void Invoke() {
						CodeLock.ProcessPacket(this);
					}
				}
			}
			namespace Other {
				/// <summary>
				/// Deactivates the door permanently.
				/// </summary>
				[MessagePackObject]
				public class DeactivateDoor : AbstractPacket {
					[Key(0)]
					public int Id { get; set; }

					public DeactivateDoor(int pID) {
						Id = pID;
					}

					public override void Invoke() {
						DoorMapIcon.ProcessPacket(this);
					}
				}
				/// <summary>
				/// Deactivates the door until renabled.
				/// </summary>
				[MessagePackObject]
				public class DisableDoorOptions : AbstractPacket {
					[Key(0)]
					public int Id { get; set; }

					public DisableDoorOptions(int pID) {
						Id = pID;
					}

					public override void Invoke() {
						DoorMapIcon.ProcessPacket(this);
					}
				}
				/// <summary>
				/// Enables the door until renabled.
				/// </summary>
				[MessagePackObject]
				public class EnableDoorOptions : AbstractPacket {
					[Key(0)]
					public int Id { get; set; }

					public EnableDoorOptions(int pID) {
						Id = pID;
					}

					public override void Invoke() {
						DoorMapIcon.ProcessPacket(this);
					}
				}
			}

		}
		namespace Firewall {
			[MessagePackObject]
			public class Creation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }
				[Key(3)]
				public bool Active { get; set; }

				public Creation(int pId, float pPosX, float pPosY, bool active) {
					Id = pId;
					PosX = pPosX;
					PosY = pPosY;
					Active = active;
				}

				public override void Invoke() {
					FirewallMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class sUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public bool Active { get; set; }

				public sUpdate(int pId, bool pActive) {
					Id = pId; Active = pActive;
				}

				public override void Invoke() {
					FirewallMapIcon.ProcessPacket(this);
				}
			}
		}
		namespace SecurityStation {
			[MessagePackObject]
			public class Creation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }

				public Creation(int pId, float pPosX, float pPosY) { Id = pId; PosX = pPosX; PosY = pPosY; }

				public override void Invoke() {
					SecuritystationMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class sUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public int State { get; set; }

				public sUpdate(int pID, int pState) {
					Id = pID;
					State = pState;
				}

				public override void Invoke() {
					SecuritystationMapIcon.ProcessPacket(this);
				}


			}

			[MessagePackObject]
			public class hUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }

				public hUpdate(int pId) { Id = pId; }

				public override void Invoke() {
					ShooterSecurityStation.ProcessPacket(this);
				}
			}

		}
		namespace PickUpIcon {
			[MessagePackObject]
			public class PhaserAmmoCreation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float posX { get; set; }
				[Key(2)]
				public float posY { get; set; }

				public PhaserAmmoCreation(int pId, Vector3 pPos) {
					Id = pId;
					posX = pPos.x;
					posY = pPos.z;
				}

				public override void Invoke() {
					PickUpMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class MachineGunAmmoCreation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float posX { get; set; }
				[Key(2)]
				public float posY { get; set; }

				public MachineGunAmmoCreation(int pId, Vector3 pPos) {
					Id = pId;
					posX = pPos.x;
					posY = pPos.z;
				}

				public override void Invoke() {
					PickUpMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class ShotgunAmmoCreation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float posX { get; set; }
				[Key(2)]
				public float posY { get; set; }

				public ShotgunAmmoCreation(int pId, Vector3 pPos) {
					Id = pId;
					posX = pPos.x;
					posY = pPos.z;
				}

				public override void Invoke() {
					PickUpMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class HealthKitCreation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float posX { get; set; }
				[Key(2)]
				public float posY { get; set; }

				public HealthKitCreation(int pId, Vector3 pPos) {
					Id = pId;
					posX = pPos.x;
					posY = pPos.z;
				}

				public override void Invoke() {
					PickUpMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class sIconUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public bool used { get; set; }

				public sIconUpdate(int pID, bool pUsed) {
					Id = pID;
					used = pUsed;
				}

				public override void Invoke() {
					AbstractMapIcon.ProcessPacket(this);
				}
			}
		}
		namespace Lasershot {
			[MessagePackObject]
			public class Creation : AbstractPacket {
				[Key(0)]
				public float startX { get; set; }
				[Key(1)]
				public float startZ { get; set; }
				[Key(2)]
				public float targetX { get; set; }
				[Key(3)]
				public float targetZ { get; set; }

				public Creation(Vector3 startPos, Vector3 endPos) {
					startX = startPos.x;
					startZ = startPos.z;
					targetX = endPos.x;
					targetZ = endPos.z;
				}

				public override void Invoke() {
					MinimapManager.Instance.CreateShot(this);
				}
			}
		}

		namespace Keycard {
			[MessagePackObject]
			public class Creation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public int[] intArray { get; set; }
				[Key(2)]
				public float posX { get; set; }
				[Key(3)]
				public float posY { get; set; }
				[Key(4)]
				public float colorR { get; set; }
				[Key(5)]
				public float colorG { get; set; }
				[Key(6)]
				public float colorB { get; set; }

				public Creation(int pID, int[] pIntArray, Vector3 pPos, float pColorR, float pColorG, float pColorB) {
					Id = pID;
					intArray = pIntArray;
					posX = pPos.x;
					posY = pPos.z;
					colorR = pColorR;
					colorG = pColorG;
					colorB = pColorB;

					isLatePacket = true;
				}

				public override void Invoke() {
					KeycardMapIcon.ProcessPacket(this);
					DoorMapIcon.AddAddon(this);
				}
			}

			[MessagePackObject]
			public class sCollected : AbstractPacket {
				[Key(0)]
				public float colorR { get; set; }
				[Key(1)]
				public float colorG { get; set; }
				[Key(2)]
				public float colorB { get; set; }

				public sCollected(Color pColor) {
					colorR = pColor.r;
					colorG = pColor.g;
					colorB = pColor.b;
				}

				public override void Invoke() {
					HUDCardHolder.GetInstance().AddCard(new Color(colorR, colorG, colorB));
				}
			}
		}

		namespace SectorDoor {
			[MessagePackObject]
			public class Creation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }
				[Key(3)]
				public float Rot { get; set; }
				[Key(4)]
				public bool Open { get; set; }
				[Key(5)]
				public bool Locked { get; set; }

				public Creation(int pId, float pPosX, float pPosY, float pRot, bool pOpen, bool pLocked) {
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Open = pOpen; Locked = pLocked;
				}

				public override void Invoke() {
					SectorDoorMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class sUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }
				[Key(3)]
				public float Rot { get; set; }
				[Key(4)]
				public bool Open { get; set; }
				[Key(5)]
				public bool Locked { get; set; }

				public sUpdate(int pId, bool pOpen, bool pLocked) {
					Id = pId; Open = pOpen; Locked = pLocked;
				}

				public override void Invoke() {
					SectorDoorMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class hUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }
				[Key(3)]
				public float Rot { get; set; }
				[Key(4)]
				public bool Open { get; set; }
				[Key(5)]
				public bool Locked { get; set; }

				public hUpdate(int pId, bool pOpen, bool pLocked) {
					Id = pId; Open = pOpen; Locked = pLocked;
				}

				public override void Invoke() {
					ShooterSectorDoor.ProcessPacket(this);
				}
			}

		}

		namespace Player {
			[MessagePackObject]
			public class sUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }
				[Key(3)]
				public float Rot { get; set; }
				[Key(4)]
				public float Health { get; set; }

				public sUpdate(int pId, float pPosX, float pPosY, float pRot, float pHealth) {
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Health = pHealth;
				}

				public override void Invoke() {
					PlayerMapIcon.ProcessPacket(this);
				}
			}
		}

		namespace Grenade {
			[MessagePackObject]
			public class sUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }

				public sUpdate(int pId, float pPosX, float pPosY) {
					Id = pId; PosX = pPosX; PosY = pPosY;
				}

				public override void Invoke() {
					GrenadeMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class hUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }

				public hUpdate(int pId, float pPosX, float pPosY) {
					Id = pId; PosX = pPosX; PosY = pPosY;
				}

				public override void Invoke() {
					ShooterGrenade.ProcessPacket(this);
				}
			}
		}

		namespace Module {
			[MessagePackObject]
			public class Creation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }
				[Key(3)]
				public float Rot { get; set; }
				[Key(4)]
				public bool Solved { get; set; }

				public Creation(int pId, float pPosX, float pPosY, float pRot, bool pSolved) {
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Solved = pSolved;
				}

				public override void Invoke() {
					ModuleMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class sUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }
				[Key(3)]
				public float Rot { get; set; }
				[Key(4)]
				public bool Solved { get; set; }

				public sUpdate(int pId, bool pSolved) {
					Id = pId; Solved = pSolved;
				}

				public override void Invoke() {
					ModuleMapIcon.ProcessPacket(this);
				}
			}
		}
		namespace Objective {
			[MessagePackObject]
			public class Creation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }
				[Key(3)]
				public bool Finished { get; set; }

				public Creation(int pId, float pPosX, float pPosY) {
					Id = pId; PosX = pPosX; PosY = pPosY;
				}

				public override void Invoke() {
					ObjectiveMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class sUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }
				[Key(3)]
				public bool Finished { get; set; }

				public sUpdate(int pId, bool pFinished) {
					Id = pId; Finished = pFinished;
				}

				public override void Invoke() {
					ObjectiveMapIcon.ProcessPacket(this);
				}
			}
		}
		namespace Drone {
			[MessagePackObject]
			public class sUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }
				[Key(3)]
				public float Rot { get; set; }
				[Key(4)]
				public float Health { get; set; }
				[Key(5)]
				public EnemyState State { get; set; }
				//public bool Shielded;
				//TODO Include info like weapon and armor and stuff

				public sUpdate(int pId, float pPosX, float pPosY, float pRot, float pHealth, EnemyState pState) {
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Health = pHealth; State = pState;
				}

				public override void Invoke() {
					DroneMapIcon.ProcessPacket(this);
				}
			}
		}

		namespace Turret {
			[MessagePackObject]
			public class Creation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }
				[Key(3)]
				public float Rot { get; set; }
				[Key(4)]
				public EnemyState State { get; set; }

				public Creation(int pId, float pPosX, float pPosY, float pRot, EnemyState pState) {
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; State = pState;
				}

				public override void Invoke() {
					TurretMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class sUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float Rot { get; set; }
				[Key(2)]
				public EnemyState State { get; set; }

				public sUpdate(int pId, float pRot, EnemyState pState) {
					Id = pId; Rot = pRot; State = pState;
				}

				public override void Invoke() {
					TurretMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class hUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public EnemyState State { get; set; }

				public hUpdate(int pId, EnemyState pState) {
					Id = pId; State = pState;
				}

				public override void Invoke() {
					ShooterTurret.ProcessPacket(this);
				}
			}

		}

		namespace Camera {
			[MessagePackObject]
			public class Creation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }
				[Key(3)]
				public float Rot { get; set; }
				[Key(4)]
				public EnemyState State { get; set; }

				public Creation(int pId, float pPosX, float pPosY, float pRot, EnemyState pState) {
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; State = pState;
				}

				public override void Invoke() {
					CameraMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class sUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float Rot { get; set; }
				[Key(2)]
				public EnemyState State { get; set; }

				public sUpdate(int pId, float pRot, EnemyState pState) {
					Id = pId; Rot = pRot; State = pState;
				}

				public override void Invoke() {
					CameraMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class hUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public EnemyState State { get; set; }

				public hUpdate(int pId, EnemyState pState) {
					Id = pId; State = pState;
				}

				public override void Invoke() {
					ShooterCamera.ProcessPacket(this);
				}
			}
		}
		namespace Gaspipe {
			[MessagePackObject]
			public class Creation : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public float PosX { get; set; }
				[Key(2)]
				public float PosY { get; set; }
				[Key(3)]
				public float Rot { get; set; }
				[Key(4)]
				public bool Exploded { get; set; }
				[Key(5)]
				public bool ChargedExplosion { get; set; }

				public Creation(int pId, float pPosX, float pPosY, float pRot, bool pExploded) {
					Id = pId; PosX = pPosX; PosY = pPosY; Rot = pRot; Exploded = pExploded;
				}

				public override void Invoke() {
					GaspipeMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class sUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public bool Exploded { get; set; }


				public sUpdate(int pId, bool pExploded) {
					Id = pId; Exploded = pExploded;
				}

				public override void Invoke() {
					GaspipeMapIcon.ProcessPacket(this);
				}
			}

			[MessagePackObject]
			public class hUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public bool ChargedExplosion { get; set; }

				public hUpdate(int pId, bool pChargedExplosion) {
					Id = pId; ChargedExplosion = pChargedExplosion;
				}

				public override void Invoke() {
					ShooterGaspipe.ProcessPacket(this);
				}
			}

		}
		namespace Minimap {
			[MessagePackObject]
			public class Creation : AbstractPacket {
				[Key(0)]
				public byte[] bytes { get; set; }

				public Creation(byte[] nBytes) {
					bytes = nBytes;
				}

				public override void Invoke() {
					HackerMinimap.ProcessPacket(this);
				}
			}

		}
		namespace Light {
			[MessagePackObject]
			public class sUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public bool IsOn { get; set; }
				//TODO Add position

				public sUpdate(int pId, bool pIsOn) {
					Id = pId;
					IsOn = pIsOn;
				}

				public override void Invoke() {
					throw new NotImplementedException();
				}
			}

			[MessagePackObject]
			public class hUpdate : AbstractPacket {
				[Key(0)]
				public int Id { get; set; }
				[Key(1)]
				public bool IsOn { get; set; }

				public hUpdate(int pId, bool pIsOn) {
					Id = pId;
					IsOn = pIsOn;
				}

				public override void Invoke() {
					throw new NotImplementedException();
				}
			}
		}

		namespace HackerMinimapCamera {
			[MessagePackObject]
			public class MoveCameraTowardsLocation : AbstractPacket {
				[Key(0)]
				public float posX { get; set; }
				[Key(1)]
				public float posY { get; set; }

				public MoveCameraTowardsLocation(Vector3 pPos) {
					posX = pPos.x;
					posY = pPos.z;
				}

				public override void Invoke() {
					MinimapManager.Instance.ProcessPacket(this);
				}
			}
		}
	}

	namespace States {
		[MessagePackObject]
		public class AlarmState : AbstractPacket {
			[Key(0)]
			public bool AlarmIsOn { get; set; }

			public AlarmState(bool pAlarmIsOn) {
				AlarmIsOn = pAlarmIsOn;
			}
			public override void Invoke() {
				HackerAlarmManager.Instance.ProcessPacket(this);
			}
		}
	}

	namespace Messages {
		[MessagePackObject]
		public class DisconnectRequest : AbstractPacket {
			public override void Invoke() {
				HackerPackageSender.SilentlyDisconnect();
			}
		}

		[MessagePackObject]
		public class CreationEnd : AbstractPacket {
			public override void Invoke() {
				HackerPackageReader.GetInstance().OnCreationEnd();
			}
		}
	}
}
