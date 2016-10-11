using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class SimpleEnemy : MonoBehaviour, IShootable {
	[SerializeField]
	private float _health;

	[SerializeField]
	private float _seeRange;

	[SerializeField]
	private bool _visualize;

	[SerializeField]
	private float _aiTickRate;
	private float _nextTick;

	[SerializeField]
	private float _rotationSpeed;

	private NavMeshAgent _agent;
	private GameObject _player;
	private Animator _animator;

	private void Start() {
		_agent = GetComponent<NavMeshAgent>();
		_player = GameObject.FindGameObjectWithTag("Player");
		_animator = GetComponent<Animator>();

		Collider[] col = GetComponentsInChildren<Collider>();

		foreach (Collider c in col) {
			c.isTrigger = true;
		}
	}

	private void Update() {
		if (_health > 0) {
			if (Vector3.Distance(transform.position, _player.transform.position) <= _seeRange) {
				if (_nextTick - Time.time <= 0) {
					_nextTick = Time.time + _aiTickRate;
					RaycastHit hit;

					//TODO Restore previous layer
					foreach (Transform t in GetComponentsInChildren<Transform>()) {
						t.gameObject.layer = 2;
					}

					if (Physics.Raycast(transform.position, (_player.transform.position - transform.position).normalized, out hit, _seeRange)) {
						if (hit.collider.tag == "Player") {
							_agent.SetDestination(_player.transform.position);
						}
					}

					foreach (Transform t in GetComponentsInChildren<Transform>()) {
						t.gameObject.layer = 0;
					}
				}
			}

			if (_agent.velocity.magnitude >= 0.1f) {
				_animator.SetBool("running", true);
			} else {
				_animator.SetBool("running", false);
			}

			if (Vector3.Distance(transform.position, _player.transform.position) <= 2.5f) {
				_animator.SetTrigger("attack");
				RotateTowards(_player.transform);
			}
		}
	}

	public void ReceiveDamage(Vector3 pDirection, Vector3 pPoint, float pDamage) {
		_health -= pDamage;

		_lastShootPos = transform.InverseTransformPoint(pPoint);

		if (_health <= 0) {
			_lastFlyDirection = transform.InverseTransformDirection(pDirection);
			_animator.SetTrigger("death");
			_agent.enabled = false;
			Collider[] col = GetComponentsInChildren<Collider>();

			foreach (Collider c in col) {
				c.enabled = false;
			}
		}
	}

	public void MakeDamage() {

	}

	public void FootStep() {

	}

	public void Disapear() {
		Destroy(this.gameObject);
	}

	private void RotateTowards(Transform target) {
		Vector3 direction = (new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
	}


	private Vector3 _lastShootPos = Vector3.zero;
	private Vector3 _lastFlyDirection = Vector3.zero;

	private void OnDrawGizmos() {
		if (_visualize) {
			if (_lastShootPos != Vector3.zero) {
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(transform.TransformPoint(_lastShootPos), 0.1f);
			}

			if (_lastFlyDirection != Vector3.zero) {
				Gizmos.color = Color.blue;
				Gizmos.DrawRay(transform.TransformPoint(_lastShootPos), transform.TransformDirection(_lastFlyDirection));
			}
		}
	}
}