using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
	private CharacterController _controller;

	[SerializeField]
	private float _walkSpeed;

	[SerializeField]
	private float _runSpeed;

	[SerializeField]
	private float _gravity;

	private Vector3 _moveDirection = Vector3.zero;

	private Animator _cameraAnimator;

	private void Start() {
		_controller = GetComponent<CharacterController>();
		_cameraAnimator = Camera.main.GetComponent<Animator>();
	}

	private void Update() {
		if (_controller.isGrounded) {
			//TODO Replace with InputManager
			_moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			_moveDirection = transform.TransformDirection(_moveDirection);


			if (Input.GetKey(KeyCode.LeftShift)) {
				_moveDirection *= _runSpeed;
			} else {
				_moveDirection *= _walkSpeed;
			}

			//_cameraAnimator.SetBool("Running", false);
			//_cameraAnimator.SetBool("Walking", false);

			if (_moveDirection.magnitude > 0.0f) {
				if (Input.GetKey(KeyCode.LeftShift)) {
					//_cameraAnimator.SetBool("Running", true);
				} else {
					//_cameraAnimator.SetBool("Walking", true);
				}
			}
		}

		_moveDirection.y -= _gravity * Time.deltaTime;
		_controller.Move(_moveDirection * Time.deltaTime);
	}
}
