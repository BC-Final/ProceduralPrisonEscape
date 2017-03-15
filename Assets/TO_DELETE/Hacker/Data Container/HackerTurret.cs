//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Gamelogic.Extensions;

//public class HackerTurret {
//	private static List<HackerTurret> _turrets = new List<HackerTurret>();

//	private MinimapTurret _minimapTurret;
//	private TurretNode _turretNode;

//	private ObservedValue<bool> _hacked;
//	private ObservedValue<bool> _accessible;
//	private ObservedValue<bool> _disabled;
//	private float _healthPercent;
//	//TODO Create Enum to reflect state
//	private int _id;

//	public int Id {
//		get { return _id; }
//	}

//	public TurretNode TurretNode {
//		set {
//			_turretNode = value;
//			_turretNode.Hacked.OnValueChange += () => { _hacked.Value = _turretNode.Hacked.Value; };
//		}
//	}

//	public ObservedValue<bool> Accessible {
//		get { return _accessible; }
//	}

//	public static void CreateTurret (CustomCommands.Creation.TurretCreation pPackage) {
//		HackerTurret turret = new HackerTurret();
//		turret._id = pPackage.Id;

//		MinimapTurret minimapTurret = MinimapManager.Instance.CreateMinimapTurret(pPackage.X, pPackage.Z, pPackage.Rot);
//		minimapTurret.InitialTransform(pPackage.Rot);

//		turret._healthPercent = pPackage.HealthPercent;

//		turret._hacked = new ObservedValue<bool>(false);
//		turret._accessible = new ObservedValue<bool>(false);
//		//TODO Add this
//		//turret._disabled = new ObservedValue<bool>(pPackage.State);
//		turret._disabled = new ObservedValue<bool>(false);


//		minimapTurret.AssociatedTurret = turret;
//		turret._minimapTurret = minimapTurret;

//		_turrets.Add(turret);
//	}

//	public static void UpdateTurret (CustomCommands.Update.TurretUpdate pPackage) {
//		GetTurretById(pPackage.id)._minimapTurret.UpdateTransform(pPackage.rotation);
//	}

//	public void DisableTurret () {
//		if (_hacked.Value) {
//			_disabled.Value = false;
//			HackerPackageSender.SendPackage(new CustomCommands.Update.DisableTurret(Id));
//		}
//	}

//	public static void EnableTurret (int pId) {
//		GetTurretById(pId)._disabled.Value = true;
//	}

//	public static HackerTurret GetTurretById (int pId) {
//		return _turrets.Find(x => x.Id == pId);
//	}
//}
