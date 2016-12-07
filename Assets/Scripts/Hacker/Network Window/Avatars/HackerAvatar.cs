using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class HackerAvatar : AbstractAvatar {
	private Vector2 _targetDirection;
	
	/// <summary>
	/// This method should be called when a node is clicked.
	/// The Hacker Avatar will then calculate a path to that node.
	/// </summary>
	/// <param name="pNode">The clicked node</param>
	public void OnClickNode(AbstractNode pNode) {
		if (_state == State.Idle) {
			_targetNode = pNode;
			_path = AstarPathFinder.CalculatePath(_currentNode, _targetNode);
			_state = State.Moving;
			_currentNode.CurrentNode = false;
			_currentNode.ToggleContext(false, this);
		}
	}
	

	/// <summary>
	/// Sets up the pathfinding
	/// Overrides start method and calls base.
	/// </summary>
	protected override void Start() {
		base.Start();
		_spawnNode = FindObjectOfType<HackerNode>();
		_currentNode = _spawnNode;
		NetworkWindow.Instance.RecalculateAccesibleNodes();
	}

	/// <summary>
	/// Overrides update method and calls the base
	/// </summary>
	protected override void Update() {
		base.Update();
	}

	/// <summary>
	/// Moves the Avatar with astar pathfinding along the graph.
	/// Determines if the state should be changed.
	/// </summary>
	protected override void move() {
		if (_path != null && _path.Count != 0) {
			if (_moveTweener == null || !_moveTweener.IsPlaying()) {
				if (!(_path[0] is FirewallNode) || (_path[0] as FirewallNode).Accessible) {
					_currentNode = _path[0];
					_moveTweener = transform.DOMove(_path[0].transform.position, Vector3.Distance(transform.position, _path[0].transform.position) / _moveSpeed);
					_path.RemoveAt(0);
					_moveTweener.SetEase(Ease.Flash);
				} else {
					_path = null;
				}
			}
		} else if (_moveTweener == null || !_moveTweener.IsPlaying()) {
			_state = State.Idle;
			_currentNode.CurrentNode = true;
			_currentNode.ToggleContext(true, this);
		}
	}


	/// <summary>
	/// Starts hacking the current node, when corresponding key is pressed
	/// </summary>
	protected override void idle() {
		
	}
	/// <summary>
	/// Aborts hacking.
	/// </summary>
	protected override void hacking() {

	}

	/// <summary>
	/// When the admin is active and hit by the hacker, the hacker spawns at his node and is stunned
	/// </summary>
	/// <param name="pOther"></param>
	protected override void OnTriggerEnter2D(Collider2D pOther) {
		AdminAvatar av = pOther.GetComponent<AdminAvatar>();

		if (av != null && av.Active) {
			_currentNode.AbortHack(this);
			_currentNode.ToggleContext(false, this);
			base.OnTriggerEnter2D(pOther);
		}
	}

	public void StartHack() {
		_state = State.Hacking;
	}

	public void AbortHack () {
		_state = State.Idle;
	}
}