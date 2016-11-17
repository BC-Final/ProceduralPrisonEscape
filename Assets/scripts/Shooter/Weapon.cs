using UnityEngine;
using System.Collections;
using DG.Tweening;

public abstract class Weapon : MonoBehaviour {
	[SerializeField]
	private float _shootDamage;

	[SerializeField]
	private float _shootRange;

	[SerializeField]
	private float _shootRate;

	[SerializeField]
	private float _spreadConeRadius;

	[SerializeField]
	private float _spreadConeLength;

	[SerializeField]
	protected int _magazineCapacity;
	public int MagazineCapacity { get { return _magazineCapacity; } }

	[SerializeField]
	protected int _maxReserveAmmo;
	public int MaxReserveAmmo { get { return _maxReserveAmmo; } }

	[SerializeField]
	protected Transform _muzzlePosition;

	[SerializeField]
	private Transform _aimPosition;
	public Transform AimPosition { get { return _aimPosition; } }

	[SerializeField]
	private float _reloadTime;

	[SerializeField]
	private float _meleeTime;

	[SerializeField]
	private float _aimTime;
	public float AimTime { get { return _aimTime; } }

	[SerializeField]
	private Vector2 _cameraRecoilForce;

	[SerializeField]
	private Vector2 _weaponRotationRecoilForce;

	[SerializeField]
	private float _weaponMoveRecoilForce;

	[SerializeField]
	private float _weaponRecoilApplyTime;

	[SerializeField]
	private float _weaponRecoilReturnTime;

	protected int _magazineContent;
	public int MagazineContent { get { return _magazineContent; } }

	protected int _reserveAmmo;
	public int ReserveAmmo { get { return _reserveAmmo; } }

	protected bool _canShoot = true;
	protected bool _reloading = false;

	protected virtual void Awake() {
		_magazineContent = _magazineCapacity;
		_reserveAmmo = _magazineCapacity;
	}

	public void AddAmmo(int pAmount) {
		_reserveAmmo = Mathf.Min(_maxReserveAmmo, _reserveAmmo + pAmount);
	}

	protected abstract void spawnBullet(Vector3 pHitPoint);
	protected abstract void spawnDecal(Vector3 pHitPoint, Vector3 pHitNormal, Transform pHitTransform);

	protected IEnumerator reload() {
		_canShoot = false;
		_reloading = true;
		yield return new WaitForSeconds(_reloadTime);
		_magazineContent = (_reserveAmmo >= _magazineCapacity) ? _magazineCapacity : _reserveAmmo;
		_reserveAmmo -= _magazineContent;
		_canShoot = true;
		_reloading = false;
	}

	protected IEnumerator shoot() {
		_canShoot = false;

		_magazineContent = Mathf.Max(_magazineContent - 1, 0);

		RaycastHit hit;

		if (Physics.Raycast(Camera.main.transform.position, calulateShootDirection(), out hit, _shootRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) {
			spawnBullet(hit.point);
			spawnDecal(hit.point, hit.normal, hit.transform);

			if (hit.rigidbody != null && hit.rigidbody.GetComponent<IDamageable>() != null) {
				hit.rigidbody.GetComponent<IDamageable>().ReceiveDamage(Camera.main.transform.forward, hit.point, _shootDamage);
			}
		} else {
			spawnBullet(_muzzlePosition.position + _muzzlePosition.forward * _shootRange);
		}

		//TODO This is very costly I think
		Sequence recoilSequence = DOTween.Sequence();
		recoilSequence.Append(transform.DOLocalRotate(new Vector3(-_weaponRotationRecoilForce.x, Random.Range(-_weaponRotationRecoilForce.y, _weaponRotationRecoilForce.y), 0.0f), _weaponRecoilApplyTime));
		recoilSequence.Join(transform.DOLocalMove(transform.localPosition + new Vector3(0.0f, 0.0f, -_weaponMoveRecoilForce), _weaponRecoilApplyTime));
		recoilSequence.Append(transform.DOLocalRotate(Vector3.zero, _weaponRecoilReturnTime));
		recoilSequence.Join(transform.DOLocalMove(transform.localPosition, _weaponRecoilApplyTime));

		yield return new WaitForSeconds(_shootRate);
		_canShoot = true;
	}

	private Vector3 calulateShootDirection() {
		//TODO Modify with walkspeed, aim, and length of fire
		float randomRadius = UnityEngine.Random.Range(0, _spreadConeRadius);
		float randomAngle = UnityEngine.Random.Range(0, 2 * Mathf.PI);

		Vector3 rayDir = new Vector3(
			randomRadius * Mathf.Cos(randomAngle),
			randomRadius * Mathf.Sin(randomAngle),
			_spreadConeLength
			);

		return Camera.main.transform.TransformDirection(rayDir.normalized);
	}

#if UNITY_EDITOR
	private void OnDrawGizmos() {
		UnityEditor.Handles.color = Color.white;
		UnityEditor.Handles.DrawWireDisc(Camera.main.transform.position + Camera.main.transform.forward * _spreadConeLength, Camera.main.transform.forward, _spreadConeRadius);
	}
#endif
}
