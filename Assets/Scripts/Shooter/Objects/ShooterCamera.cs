using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateFramework;
using System.Linq;

[SelectionBase]
public class ShooterCamera : MonoBehaviour, IShooterNetworked, IDamageable {
	[SerializeField]
	private CameraParameters _parameters;
	public CameraParameters Parameters { get { return _parameters; } }

	[Header("References")]
	[SerializeField]
	private Transform _base;
	public Transform Base { get { return _base; } }

	[SerializeField]
	private Transform _lookPoint;
	public Transform LookPoint { get { return _lookPoint; } }

	[SerializeField]
	private SphereCollider _viewTrigger;
	public SphereCollider ViewTrigger { get { return _viewTrigger; } }

	[Header("Debug")]
	[SerializeField]
	private bool _visualizeView;
	public bool VisualizeView { get { return _visualizeView; } }

	private StateMachine<AbstractCameraState> _fsm;

	private Timer _networkUpdateTimer;

	[SerializeField]
	private Faction _faction = Faction.Prison;
	public Faction Faction { get { return _faction; } }

	private List<ITargetable> _possibleTargets = new List<ITargetable>();
	public Transform[] PossibleTargets {
		get {
			return _possibleTargets.FindAll(x => x.Faction != _faction).Select(x => x.GameObject.transform).ToArray();
		}
	}


	private System.Action<GameObject> _destroyEvent;

	public void AddToDestroyEvent (System.Action<GameObject> pObject) {
		_destroyEvent += pObject;
	}

	public void RemoveFromDestroyEvent (System.Action<GameObject> pObject) {
		_destroyEvent -= pObject;
	}

	public GameObject GameObject { get { return gameObject; } }

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
		_networkUpdateTimer = TimerManager.CreateTimer("Camera Network Update", false).SetDuration(_parameters.NetworkUpdateRate).SetLoops(-1).AddCallback(() => sendUpdate()).Start();
	}

	private void sendUpdate () {
		EnemyState state = _faction == Faction.Neutral ? EnemyState.Controlled : _fsm.GetState() is CameraDisabledState ? EnemyState.Stunned : _fsm.GetState() is CameraDetectState ? EnemyState.SeesPlayer : EnemyState.None;

		ShooterPackageSender.SendPackage(new NetworkPacket.Update.Camera(Id, transform.position.x, transform.position.z, _base.rotation.eulerAngles.y, state));
	}

	private void Awake () {
		ShooterPackageSender.RegisterNetworkObject(this);
	}

	private void OnDestroy () {
		if (_networkUpdateTimer != null) {
			_networkUpdateTimer.Stop();
			sendUpdate();
		}

		ShooterPackageSender.UnregisterNetworkedObject(this);
	}

	public void ReceiveDamage (Transform pSource, Vector3 pHitPoint, float pDamage, float pForce) {
		_fsm.GetState().ReceiveDamage(pSource, pHitPoint, pDamage, pForce);
	}

	private void Start () {
		_fsm = new StateMachine<AbstractCameraState>();

		//_fsm.AddState(new CameraIdleState(this, _fsm));
		_fsm.AddState(new CameraGuardState(this, _fsm));
		_fsm.AddState(new CameraDetectState(this, _fsm));
		_fsm.AddState(new CameraDisabledState(this, _fsm));

		_fsm.SetState<CameraGuardState>();

		_viewTrigger.radius = _parameters.ViewRange;
	}

	[SerializeField]
	private string CurrentState;

	private void Update () {
		CurrentState = _fsm.GetState().GetType().Name;

		_fsm.Step();
	}

	private void disable () {
		_fsm.SetState<CameraDisabledState>();
	}

	private void control () {
		_faction = Faction.Neutral;
		TimerManager.CreateTimer("Camera Controlled", true).SetDuration(_parameters.ControllDuration).AddCallback(() => _faction = Faction.Prison).Start();
	}


	private void OnTriggerEnter (Collider other) {
		ITargetable d = other.GetComponentInParent<ITargetable>();

		if (d != null && other.gameObject.layer != LayerMask.NameToLayer("RangeTrigger")) {
			d.AddToDestroyEvent(g => onTargetDestroyed(g));
			_possibleTargets.Add(d);
		}
	}

	private void OnTriggerExit (Collider other) {
		ITargetable d = other.GetComponentInParent<ITargetable>();

		if (d != null && other.gameObject.layer != LayerMask.NameToLayer("RangeTrigger")) {
			d.RemoveFromDestroyEvent(g => onTargetDestroyed(g));
			_possibleTargets.RemoveAll(x => x.GameObject == d.GameObject);
		}
	}

	private void onTargetDestroyed (GameObject pObject) {
		if (pObject.layer != LayerMask.NameToLayer("RangeTrigger")) {
			_possibleTargets.RemoveAll(x => x.GameObject == pObject);
		}
	}


#if UNITY_EDITOR
	private void OnDrawGizmos () {
		if (_visualizeView) {
			Gizmos.color = Color.white;
			UnityEditor.Handles.color = Color.white;

			if (_parameters.ViewAngle < 360.0f) {
				Gizmos.DrawLine(_lookPoint.position, _lookPoint.TransformPoint(Quaternion.Euler(0f, _parameters.ViewAngle / 2f, 0f) * (Vector3.forward * _parameters.ViewRange)));
				Gizmos.DrawLine(_lookPoint.position, _lookPoint.TransformPoint(Quaternion.Euler(0f, -(_parameters.ViewAngle / 2f), 0f) * (Vector3.forward * _parameters.ViewRange)));

				Gizmos.DrawLine(_lookPoint.position, _lookPoint.TransformPoint(Quaternion.Euler(_parameters.ViewAngle / 2f, 0f, 0f) * (Vector3.forward * _parameters.ViewRange)));
				Gizmos.DrawLine(_lookPoint.position, _lookPoint.TransformPoint(Quaternion.Euler(-(_parameters.ViewAngle / 2f), 0f, 0f) * (Vector3.forward * _parameters.ViewRange)));


				UnityEditor.Handles.DrawWireArc(_lookPoint.position, -_lookPoint.up, _lookPoint.TransformDirection(Quaternion.Euler(0f, _parameters.ViewAngle / 2f, 0f) * (Vector3.forward * _parameters.ViewRange).normalized), _parameters.ViewAngle, _parameters.ViewRange);
				UnityEditor.Handles.DrawWireArc(_lookPoint.position, -_lookPoint.right, _lookPoint.TransformDirection(Quaternion.Euler(_parameters.ViewAngle / 2f, 0f, 0f) * (Vector3.forward * _parameters.ViewRange).normalized), _parameters.ViewAngle, _parameters.ViewRange);
			} else {
				UnityEditor.Handles.DrawWireDisc(_lookPoint.position, -_lookPoint.up, _parameters.ViewRange);
			}
		}
	}
#endif
}