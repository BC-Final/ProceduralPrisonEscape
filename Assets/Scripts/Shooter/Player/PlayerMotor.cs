using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour {
	[SerializeField]
	private float _speed = 6.0F;

	[SerializeField]
	private float _jumpSpeed = 8.0F;

	[SerializeField]
	private float _gravity = 20.0F;


	private CharacterController _ctrl;
	private Vector3 _moveDirection = Vector3.zero;

	private void Start() {
		_ctrl = GetComponent<CharacterController>();
	}

	private void Update() {
		if (_ctrl.isGrounded) {
			_moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			_moveDirection = transform.TransformDirection(_moveDirection);
			_moveDirection *= _speed;

			if (Input.GetButton("Jump")) {
				_moveDirection.y = _jumpSpeed;
			}
		}

		_moveDirection.y -= _gravity * Time.deltaTime;
		_ctrl.Move(_moveDirection * Time.deltaTime);
	}
}
