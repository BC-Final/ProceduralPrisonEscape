using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public abstract class AbstractAvatar : MonoBehaviour {
	[SerializeField]
	[Tooltip("The time it takes the avatar to be able to move after a respawn")]
	private float _respawnTime;

	[SerializeField]
	[Tooltip("The speed the Avatar walks in the network (units/s)")]
	protected float _moveSpeed;

	protected AbstractNode _spawnNode;
	public AbstractNode SpawnNode { set { _spawnNode = value; } }

	protected bool _active;
	public bool Active { get { return _active; } }

	protected List<AbstractNode> _path;
	protected Tweener _moveTweener;
	protected Tweener _scaleTweener;

	protected AbstractNode _currentNode;
	public AbstractNode CurrentNode {
		get { return _currentNode; }
	}

	protected AbstractNode _targetNode;
	protected AbstractNode _previousNode;

	protected State _state;
	protected enum State {
		Idle,
		Moving,
		Hacking
	}

	protected Coroutine _deactivateCoroutine;

	/// <summary>
	/// Should be called when a hack on a node is finished
	/// Sets the State of the Avatar beck to idle
	/// </summary>
	public virtual void HackFinished() {
		_state = State.Idle;
	}

	/// <summary>
	/// Gets called at the Start of a Avatar. Please overide and call base function.
	/// Sets the start state and activates the Avatar.
	/// </summary>
	protected virtual void Start() {
		_state = State.Idle;
		_active = true;
	}

	/// <summary>
	/// Gets called every frame. Please override and call base function.
	/// Determines wich state function should be executed.
	/// </summary>
	protected virtual void Update() {
		if (_active) {
			switch (_state) {
				case State.Idle:
					idle();
					break;
				case State.Moving:
					move();
					break;
				case State.Hacking:
					hacking();
					break;
			}
		}
	}

	/// <summary>
	/// Gets called every frame when the Avatar is in the move state
	/// </summary>
	protected abstract void move();

	/// <summary>
	/// Gets called every frame when the Avatar is in the idle state
	/// </summary>
	protected abstract void idle();

	/// <summary>
	/// Gets called every frame when the Avatar is in the hack state
	/// </summary>
	protected abstract void hacking();

	/// <summary>
	/// Gets called when the Avatar gets stunned.
	/// </summary>
	/// <param name="pTime">The amount of the Avatar is stunned (seconds)</param>
	public void Deactivate(float pTime) {
		if (_deactivateCoroutine != null) {
			StopCoroutine(_deactivateCoroutine);
			_scaleTweener.Kill();
			transform.localScale = Vector3.one;
		}

		if (_state == State.Hacking) {
			_currentNode.AbortHack(this);
			_state = State.Idle;
		}

		_deactivateCoroutine = StartCoroutine(deactivate(pTime));
	}

	/// <summary>
	/// Coroutine that deactivates the avatar and reactivates it after a certain amount of time
	/// </summary>
	/// <param name="pTime">The amount of time the Avatar is stunned</param>
	/// <returns></returns>
	private IEnumerator deactivate(float pTime) {
		_moveTweener.Pause();
		 _active = false;
		transform.localScale = Vector3.one / 2.0f;
		_scaleTweener = transform.DOScale(Vector3.one, pTime);
		yield return new WaitForSeconds(pTime);
		_moveTweener.Play();
		_active = true;

		if (this is AdminAvatar) {
			FMODUnity.RuntimeManager.CreateInstance("event:/PE_hacker/PE_hacker_admin_enable").start();
		} 
	}

	/// <summary>
	/// Gets called when a avatar hits another avatar
	/// </summary>
	/// <param name="pOther"></param>
	protected virtual void OnTriggerEnter2D(Collider2D pOther) {
		Deactivate(_respawnTime);
		transform.position = _spawnNode.transform.position;
		_path = null;
		_currentNode = _spawnNode;
		_moveTweener.Kill();
	}
}
