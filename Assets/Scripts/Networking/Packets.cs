using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Packets {
	namespace Effects {
		public class LaserShot { }
	}

	namespace Icons {
		namespace Pickup {
			public class Create {
				public int Id; public float PosX, PosZ; IconType Type;
				public Create (int pId, float pPosX, float pPosZ, IconType pType) { Id = pId; PosX = pPosX; PosZ = pPosZ; Type = pType; }
			}

			public class Update {
				public int Id; public float PosX, PosZ;
				public Update(int pId, float pPosX, float pPosZ) { Id = pId; PosX = pPosX; PosZ = pPosZ; }
			}

			public class Remove {
				public int Id;
				public Remove(int pId) { Id = pId; }
			}
		}
	}

	namespace Agents {
		namespace Player {
			public class Create {
				public int Id; public float PosX, PosZ, Rot, Health, TickRate;
				public Create(int pId, float pPosX, float pPosZ, float pRot, float pHealth, float pTickRate) { Id = pId; PosX = pPosX; PosZ = pPosZ; Rot = pRot; Health = pHealth; TickRate = pTickRate; }
			}

			public class Update {
				public int Id; public float PosX, PosZ, Rot, Health;
				public Update(int pId, float pPosX, float pPosZ, float pRot, float pHealth) { Id = pId; PosX = pPosX; PosZ = pPosZ; Rot = pRot; Health = pHealth; }
			}
		}

		namespace Drones {
			public class Create {
				public int Id; public float PosX, PosZ, Rot, TickRate, Health; public EnemyState State;
				public Create(int pId, float pPosX, float pPosZ, float pRot, float pHealth, float pTickRate, EnemyState pState) { Id = pId; PosX = pPosX; PosZ = pPosZ; Rot = pRot; Health = pHealth; TickRate = pTickRate; State = pState; }
			}

			public class Update {
				public int Id; public float PosX, PosZ, Rot, Health; public EnemyState State;
				public Update(int pId, float pPosX, float pPosZ, float pRot, float pHealth, EnemyState pState) { Id = pId; PosX = pPosX; PosZ = pPosZ; Rot = pRot; Health = pHealth; State = pState; }
			}
		}

		namespace Turret {
			public class Create {
				public int Id; public float PosX, PosZ, Rot, TickRate; public EnemyState State;
				public Create(int pId, float pPosX, float pPosZ, float pRot, float pHealth, float pTickRate, EnemyState pState) { Id = pId; PosX = pPosX; PosZ = pPosZ; Rot = pRot; TickRate = pTickRate; State = pState; }
			}

			public class Update {
				public int Id; public float Rot; public EnemyState State;
				public Update(int pId, float pRot, EnemyState pState) { Id = pId; Rot = pRot; State = pState; }
			}

			public class Disable { }
			public class Control { }
		}

		namespace Camera {
			public class Create {
				public int Id; public float PosX, PosZ, Rot, TickRate; public EnemyState State;
				public Create(int pId, float pPosX, float pPosZ, float pRot, float pHealth, float pTickRate, EnemyState pState) { Id = pId; PosX = pPosX; PosZ = pPosZ; Rot = pRot; TickRate = pTickRate; State = pState; }
			}

			public class Update {
				public int Id; public float Rot; public EnemyState State;
				public Update(int pId, float pRot, EnemyState pState) { Id = pId; Rot = pRot; State = pState; }
			}

			public class Disable { }
			public class Control { }
		}
	}

	namespace Interactables {
		namespace Door {
			public class Create { }
			public class Update { }

			public class SetOpen { }
			public class SetLocked { }
		}

		namespace Sectordoor {
			public class Create { }
			public class Update { }

			public class SetOpen { }
			public class SetLock { }
		}

		namespace Grenade {
			public class Create { }
			public class Update { }

			public class Explode { }
		}

		namespace Gaspipe {
			public class Create { }
			public class Update { }

			public class Explode { }
		}

		namespace Fusebox {
			public class Create { }
			public class Update { }

			public class Prime { }
		}

		namespace SecurityStation {
			public class Create { }
			public class Update { }

			public class Interact { }
		}

		namespace Light {
			public class Create { }
			public class Update { }

			public class SetOn { }
			public class Explode { }
		}
	}

	namespace Gamestates {
		namespace Alarm {
			public class SetState { }
		}
	}

	namespace Modules {
		namespace Module {
			public class Create { }
			public class Update { }
		}

		namespace Objective {
			public class Create { }
			public class Update { }
			public class Remove { }
		}
	}


	[System.Serializable]
	public class AbstracktPacket { }
}