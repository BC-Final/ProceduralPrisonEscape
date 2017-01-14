using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Extensions;

public class HackerCamera {
	private static List<HackerCamera> _cameras = new List<HackerCamera>();

	private MinimapCamera _minimapCamera;
	private CameraNode _cameraNode;

	private ObservedValue<bool> _hacked;
	private ObservedValue<bool> _accessible;
	private ObservedValue<bool> _disabled;
	private int _id;

	public int Id {
		get { return _id; }
	}

	public static void CreateCamera (CustomCommands.Creation.CameraCreation pPackage) {
		HackerCamera camera = new HackerCamera();
		camera._id = pPackage.Id;

		MinimapCamera minimapCamera = MinimapManager.Instance.CreateMinimapCamera(pPackage.X, pPackage.Z);
		minimapCamera.InitialTransform(pPackage.Rot);

		camera._hacked = new ObservedValue<bool>(false);
		camera._accessible = new ObservedValue<bool>(false);
		camera._disabled = new ObservedValue<bool>(pPackage.State);

		minimapCamera.AssociatedCamera = camera;
		camera._minimapCamera = minimapCamera;

		if (camera == null) {
			Debug.Log("LOLOLOL");
		}

		_cameras.Add(camera);
	}

	public static void UpdateCamera (int pId, float pRot, bool pSeesPlayer) {
		GetCameraById(pId)._minimapCamera.UpdateTransform(pRot);
	}

	public static void EnableCamera (int pId) {
		GetCameraById(pId)._disabled.Value = false;
	}

	public static HackerCamera GetCameraById (int pId) {
		return _cameras.Find(x => x.Id == pId);
	}
}
