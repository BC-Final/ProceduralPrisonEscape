using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Packets {
	namespace Effects {
		public class LaserShot { }
	}

	namespace Icons {
		namespace PistolAmmo {
			public class Create { }
			public class Update { }
			public class Remove { }
		}

		namespace MachinegunAmmo {
			public class Create { }
			public class Update { }
			public class Remove { }
		}

		namespace MininglaserAmmo {
			public class Create { }
			public class Update { }
			public class Remove { }
		}

		namespace GrenadeAmmo {
			public class Create { }
			public class Update { }
			public class Remove { }
		}

		namespace Healthkit {
			public class Create { }
			public class Update { }
			public class Remove { }
		}
	}

	namespace Agents {
		namespace Player {
			public class Create {
				public int Id; public float PosX, PosZ, Rot, Health;
				
			}

			public class Update { }

			public class SetDead { }
		}

		namespace Drones {
			public class Create { }
			public class Update { }
		}

		namespace Turret {
			public class Create { }
			public class Update { }

			public class Disable { }
			public class Control { }
		}

		namespace Camera {
			public class Create { }
			public class Update { }

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