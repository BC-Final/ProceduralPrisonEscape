﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateFramework;

public class ShooterCamera : MonoBehaviour, IShooterNetworked {
	//TODO Maybe put into scriptable object

	[SerializeField]
	private Transform _base;
	public Transform Base { get { return _base; } }

	[SerializeField]
	private Transform _lookPoint;
	public Transform LookPoint { get { return _lookPoint; } }

	[SerializeField]
	private float _lookRotationSpeed;
	public float LookRotationSpeed { get { return _lookRotationSpeed; } }

	[SerializeField]
	private float _seeRange;
	public float SeeRange { get { return _seeRange; } }

	[SerializeField]
	private float _quitIdleRange;
	public float QuitIdleRange { get { return _quitIdleRange; } }

	[SerializeField]
	private float _seeAngle;
	public float SeeAngle { get { return _seeAngle; } }

	[SerializeField]
	private float _triggerTime;
	public float TriggerTime { get { return _triggerTime; } }

	[SerializeField]
	private float _rotationAngle;
	public float RotationAngle { get { return _rotationAngle; } }

	[SerializeField]
	private float _rotationSpeed;
	public float RotationSpeed { get { return _rotationSpeed; } }

	[SerializeField]
	private bool _visualizeView;
	public bool VisualizeView { get { return _visualizeView; } }

	[SerializeField]
	private float _positionSendRate = 0.5f;
	private float _positionSendTimer = 0.0f;

	[SerializeField]
	private float _disabledTime = 5.0f;
	public float DisabledTime { get { return _disabledTime; } }

	private StateMachine<AbstractCameraState> _fsm;

	private int _id;
	public int Id {
		get {
			if (_id == 0) {
				_id = IdManager.RequestId();
			}

			return _id;
		}
	}

	public static void ProcessPacket (NetworkPacket.Update.Camera pPacket) {
		ShooterCamera cam = ShooterPackageSender.GetNetworkedObject<ShooterCamera>(pPacket.Id);

		if (cam != null) {
			switch (pPacket.State) {
				case EnemyState.Stunned: cam.disable(); break;
				case EnemyState.Controlled: cam.control(); break;
			}
		} else {
			Debug.LogError("Trying to access non existent networked object with id " + pPacket.Id);
		}
	}

	public void Initialize () {
		sendUpdate();
	}

	private void sendUpdate () {
		//TODO Include the current camera state
		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Camera(Id, transform.position.x, transform.position.z, _base.rotation.eulerAngles.y, EnemyState.None));
	}

	private void Awake () {
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy () {
		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	private void Start () {
		_fsm = new StateMachine<AbstractCameraState>();

		_fsm.AddState(new CameraIdleState(this, _fsm));
		_fsm.AddState(new CameraGuardState(this, _fsm));
		_fsm.AddState(new CameraDetectState(this, _fsm));
		_fsm.AddState(new CameraDisabledState(this, _fsm));

		_fsm.SetState<CameraIdleState>();
	}


	private void Update () {
		if (_positionSendTimer - Time.time <= 0.0f) {
			_positionSendTimer = Time.time + _positionSendRate;

			sendUpdate();
		}


		_fsm.Step();
	}

	private void disable () {
		_fsm.SetState<CameraDisabledState>();
	}

	private void control () {
		//TODO Implement camera controlling
	}


#if UNITY_EDITOR
	private void OnDrawGizmos () {
		if (_visualizeView) {
			Gizmos.color = Color.white;
			UnityEditor.Handles.color = Color.white;

			if (_seeAngle < 360.0f) {
				Gizmos.DrawLine(_lookPoint.position, _lookPoint.TransformPoint(Quaternion.Euler(0f, _seeAngle / 2f, 0f) * (Vector3.forward * _seeRange)));
				Gizmos.DrawLine(_lookPoint.position, _lookPoint.TransformPoint(Quaternion.Euler(0f, -(_seeAngle / 2f), 0f) * (Vector3.forward * _seeRange)));

				Gizmos.DrawLine(_lookPoint.position, _lookPoint.TransformPoint(Quaternion.Euler(_seeAngle / 2f, 0f, 0f) * (Vector3.forward * _seeRange)));
				Gizmos.DrawLine(_lookPoint.position, _lookPoint.TransformPoint(Quaternion.Euler(-(_seeAngle / 2f), 0f, 0f) * (Vector3.forward * _seeRange)));


				UnityEditor.Handles.DrawWireArc(_lookPoint.position, -_lookPoint.up, _lookPoint.TransformDirection(Quaternion.Euler(0f, _seeAngle / 2f, 0f) * (Vector3.forward * _seeRange).normalized), _seeAngle, _seeRange);
				UnityEditor.Handles.DrawWireArc(_lookPoint.position, -_lookPoint.right, _lookPoint.TransformDirection(Quaternion.Euler(_seeAngle / 2f, 0f, 0f) * (Vector3.forward * _seeRange).normalized), _seeAngle, _seeRange);
			} else {
				UnityEditor.Handles.DrawWireDisc(_lookPoint.position, -_lookPoint.up, _seeRange);
			}
		}
	}
#endif
}