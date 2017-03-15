//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Gamelogic.Extensions;

//public class HackerCamera {
//	private static List<HackerCamera> _cameras = new List<HackerCamera>();

//	private MinimapCamera _minimapCamera;
//	private CameraNode _cameraNode;

//	private ObservedValue<bool> _hacked;
//	private ObservedValue<bool> _accessible;
//	private ObservedValue<bool> _disabled;
//	private ObservedValue<bool> _seesPlayer;

//	public ObservedValue<bool> SeesPlayer {
//		get { return _seesPlayer; }
//	}

//	private int _id;

//	public int Id {
//		get { return _id; }
//	}

//	public CameraNode CamerNode {
//		set {
//			_cameraNode = value;
//			_cameraNode.Hacked.OnValueChange += () => { _hacked.Value = _cameraNode.Hacked.Value; };
//		}
//	}

//	public ObservedValue<bool> Accessible {
//		get { return _accessible; }
//	}

//	public static void CreateCamera (CustomCommands.Creation.CameraCreation pPackage) {
//		HackerCamera camera = new HackerCamera();
//		camera._id = pPackage.Id;

//		MinimapCamera minimapCamera = MinimapManager.Instance.CreateMinimapCamera(pPackage.X, pPackage.Z, pPackage.Rot);
//		minimapCamera.InitialTransform(pPackage.Rot);

//		camera._hacked = new ObservedValue<bool>(false);
//		camera._accessible = new ObservedValue<bool>(false);
//		camera._disabled = new ObservedValue<bool>(pPackage.State);
//		camera._seesPlayer = new ObservedValue<bool>(false);

//		//TODO subscribe to seesplayer
		

//		minimapCamera.AssociatedCamera = camera;
//		camera._minimapCamera = minimapCamera;

//		_cameras.Add(camera);
//	}

//	public static void UpdateCamera (int pId, float pRot, bool pSeesPlayer) {
//		GetCameraById(pId)._minimapCamera.UpdateTransform(pRot);
//		GetCameraById(pId)._seesPlayer.Value = pSeesPlayer;
//	}

//	public void DisableCamera () {
//		if (_hacked.Value) {
//			_disabled.Value = false;
//			HackerPackageSender.SendPackage(new CustomCommands.Update.DisableCamera(Id));
//		}
//	}

//	public static void EnableCamera (int pId) {
//		GetCameraById(pId)._disabled.Value = true;
//	}

//	public static HackerCamera GetCameraById (int pId) {
//		return _cameras.Find(x => x.Id == pId);
//	}
//}
