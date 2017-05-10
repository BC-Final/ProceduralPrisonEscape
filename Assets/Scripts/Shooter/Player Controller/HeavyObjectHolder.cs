using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[SelectionBase]
public class HeavyObjectHolder : MonoBehaviour {
	[SerializeField]
	private float _pickUpTime;

	[SerializeField]
	private float _putDownTime;

	private BatteryObjective _cube;

	private bool _carrying;
	private bool _dropping;

	public void Pickup (BatteryObjective pCube) {
		pCube.transform.parent = this.transform;

		GetComponent<WeaponHolder>().DisableWeapons();
		GetComponentInParent<PlayerInteract>().SetActive(false);
		GetComponentInParent<PlayerMotor>().SetCanMove(false);
		//TODO Play animation

		DOTween.Sequence()
		.Append(pCube.transform.DOLocalMove(Vector3.zero, _pickUpTime))
		.Join(pCube.transform.DOLocalRotate(Vector3.zero, _pickUpTime))
		.AppendCallback(() => finishedPickup());

		_cube = pCube;
	}

	private void finishedPickup () {
		GetComponentInParent<PlayerMotor>().SetCanMove(true);
		GetComponentInParent<PlayerMotor>().SetSlowMove(true);
		_carrying = true;

		foreach (Transform t in _cube.GetComponentsInChildren<Transform>()) {
			t.gameObject.layer = LayerMask.NameToLayer("Weapons");
		}
	}

	private void finishedDrop () {
		_cube.Drop();

		foreach (Transform t in _cube.GetComponentsInChildren<Transform>()) {
			t.gameObject.layer = LayerMask.NameToLayer("Default");
		}

		GetComponentInParent<MouseLook>().SetCanControl(true);
		GetComponentInParent<PlayerMotor>().SetCanMove(true);
		GetComponentInParent<PlayerMotor>().SetSlowMove(false);
		GetComponent<WeaponHolder>().EnableWeapons();
		FindObjectOfType<PlayerInteract>().SetActive(true);

		_cube = null;
	}

	public void Update () {
		if (_carrying && Input.GetKeyDown(KeyCode.E)) {
			RaycastHit hit;

			if (!Physics.BoxCast(transform.parent.parent.position, Vector3.one / 2.0f,  transform.parent.parent.forward, transform.parent.parent.rotation, 1.25f, LayerMask.GetMask("Environment"))) {
				if (Physics.BoxCast(transform.parent.parent.position + transform.parent.parent.forward * 1.25f, Vector3.one / 2.0f, Vector3.down, transform.parent.parent.rotation, 1.5f, LayerMask.GetMask("Environment"))) {
					if (Physics.Raycast(transform.parent.parent.position + transform.parent.parent.forward * 1.25f, Vector3.down, out hit, LayerMask.GetMask("Environment"))) {
						GetComponentInParent<PlayerMotor>().SetCanMove(false);
						GetComponentInParent<MouseLook>().SetCanControl(false);

						_carrying = false;

						_cube.transform.parent = null;
						//_cube.transform.position = hit.point + Vector3.up;

						DOTween.Sequence()
						.Append(_cube.transform.DOMove(transform.parent.parent.position + transform.parent.parent.forward * 1.25f, _putDownTime / 2.0f))
						.Join(_cube.transform.DORotate(transform.parent.parent.rotation.eulerAngles, _putDownTime / 2.0f))
						.Append(_cube.transform.DOMove(hit.point + Vector3.up / 2.0f, _putDownTime / 2.0f))
						.AppendCallback(() => finishedDrop());
					}
				}
			}
		}
	}

	private void OnDrawGizmos() {
		Gizmos.DrawRay(transform.parent.parent.position, transform.parent.parent.forward * 2.0f);
		Gizmos.DrawRay(transform.parent.parent.position + transform.parent.parent.forward * 1.25f, Vector3.down * 1.5f);
	}
}
