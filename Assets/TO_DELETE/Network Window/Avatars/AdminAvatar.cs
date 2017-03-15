//using UnityEngine;
//using System.Collections.Generic;
//using DG.Tweening;
//using System;

//public class AdminAvatar : AbstractAvatar {
//	/// <summary>
//	/// Sets up the pathfinding
//	/// Overrides start method and calls base.
//	/// </summary>
//	protected override void Start() {
//		base.Start();
//		_currentNode = FindObjectOfType<SecurityNode>();
//		_previousNode = _currentNode;
//	}

//	/// <summary>
//	/// Overrides update method and calls the base
//	/// </summary>
//	protected override void Update() {
//		base.Update();
//	}

//	/// <summary>
//	/// Moves the Avatar with astar pathfinding along the graph.
//	/// Determines if the state should be changed.
//	/// Only gets called, when avatar is active
//	/// </summary>
//	protected override void move() {
//		if (_path != null && _path.Count != 0) {
//			if (_moveTweener == null || !_moveTweener.IsPlaying()) {
//					_currentNode = _path[0];
//					_moveTweener = transform.DOMove(_path[0].transform.position, Vector3.Distance(transform.position, _path[0].transform.position) / _moveSpeed);
//					_path.RemoveAt(0);
//					_moveTweener.SetEase(Ease.Flash);
//			}
//		} else if (_moveTweener == null || !_moveTweener.IsPlaying()) {
//			if (_currentNode.Hacked.Value && _currentNode.StartHack(this)) {
//				_state = State.Hacking;
//			} else {
//				_state = State.Idle;
//			}
//		}
//	}

//	/// <summary>
//	/// Determines what action the Admin should take when on a Node.
//	/// Option A: Look if his security node has known hacked nodes.
//	/// Option B: Walk to random adjacent node
//	/// Only gets called when avatar is active
//	/// </summary>
//	protected override void idle() {
//		SecurityNode sNode = _spawnNode as SecurityNode;

//		if (sNode.KnownHackedNodes.Count != 0) {
//			_targetNode = sNode.KnownHackedNodes.First.Value;
//			sNode.KnownHackedNodes.RemoveFirst();
//		} else {
//			List<AbstractNode> _targetNodes = new List<AbstractNode>(_currentNode.GetConnections().ToArray());;

//			foreach (AbstractNode n in _currentNode.GetConnections()) {
//				if (n.gameObject != _previousNode.gameObject) {
//					_targetNodes.Add(n);
//				}
//			}

//			_targetNodes.RemoveAll(x => x is HackerNode);

//			_previousNode = _currentNode;
//			_targetNode = _targetNodes[UnityEngine.Random.Range(0, _targetNodes.Count)];
//		}

//		_path = AstarPathFinder.CalculatePath(_currentNode, _targetNode);
//		_state = State.Moving;
//	}

//	/// <summary>
//	/// Overrides base function, calls it, and also protects a node the admin hacked.
//	/// </summary>
//	public override void HackFinished() {
//		base.HackFinished();
//		_currentNode.Protect();
//	}

//	/// <summary>
//	/// Overrides base function but does nothing yet.
//	/// </summary>
//	protected override void hacking() {
//		//TODO what can the admin avatar do here?
//	}

//	/// <summary>
//	/// When the admin is disabled and hit by the hacker, it respawns to its spawnpoint
//	/// </summary>
//	/// <param name="pOther"></param>
//	protected override void OnTriggerEnter2D(Collider2D pOther) {
//		if (!_active && pOther.GetComponent<HackerAvatar>() != null) {
//			base.OnTriggerEnter2D(pOther);
//			FMODUnity.RuntimeManager.CreateInstance("event:/PE_hacker/PE_hacker_admin_death").start();
//		}
//	}
//}
