﻿using UnityEngine;
using System.Collections;

namespace CustomCommands {
	/// <summary>
	/// Update Packages
	/// </summary>
	namespace Update {

        [System.Serializable]
        public class AlarmUpdate : AbstractPackage
        {
            public bool state;
            
            public AlarmUpdate (bool nState)
            {
                state = nState;
            }
        }

        [System.Serializable]
        public class AlarmTimerUpdate : AbstractPackage
        {
            public float timer;

            public AlarmTimerUpdate(float nTimer)
            {
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
		public class EnemyUpdate : AbstractPackage {
			public int id;
			public int hpPercent;
			public float x;
			public float z;
			public float rotation;

			public EnemyUpdate (int nID, int nHpPercent, Vector3 pos, float nRotation) {
				id = nID;
				hpPercent = nHpPercent;
				x = pos.x;
				z = pos.z;
				rotation = nRotation;
			}
		}

        [System.Serializable]
        public class CameraUpdate : AbstractPackage
        {
            public int id;
            public int hpPercent;
            public float x;
            public float z;
            public float rotation;

            public CameraUpdate(int nID, int nHpPercent, Vector3 pos, float nRotation)
            {
                id = nID;
                hpPercent = nHpPercent;
                x = pos.x;
                z = pos.z;
                rotation = nRotation;
            }
        }

        [System.Serializable]
        public class TurretUpdate : AbstractPackage
        {
            public int id;
            public int hpPercent;
            public float x;
            public float z;
            public float rotation;

            public TurretUpdate(int nID, int nHpPercent, Vector3 pos, float nRotation)
            {
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
		namespace Items {
			[System.Serializable]
			public class KeyCardCreation : AbstractPackage {
				public int ID;
				public float x;
				public float z;

				public KeyCardCreation (int nID, float nX, float nZ) {
					ID = nID;
					x = nX;
					z = nZ;
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
        namespace Messaging
        {
            [System.Serializable]
            public class TextMessage : AbstractPackage
            {
                public string message;
                public TextMessage (string nMessage)
                {
                    message = nMessage;
                }
            }
        }
	}

    namespace Communication
    {
        namespace Hacker
        {
            [System.Serializable]
            public class EnemyWaveDispatched : AbstractPackage
            {
            }
        }

        namespace Shooter
        {
            [System.Serializable]
            public class NeedHealth : AbstractPackage
            {
            }
            [System.Serializable]
            public class NeedHelp : AbstractPackage
            {
            }
            [System.Serializable]
            public class NeedAmmo : AbstractPackage
            {
            }
        }
    }

	[System.Serializable]
	public abstract class AbstractPackage { }

	[System.Serializable]
	public class DisconnectPackage : AbstractPackage { }

	[System.Serializable]
	public class RefuseConnectionPackage : AbstractPackage { }

	[System.Serializable]
	public class ServerShutdownPackage : AbstractPackage { }

	[System.Serializable]
	public class NotImplementedMessage : AbstractPackage {
		public string message = "Not implemented";
	}
}
