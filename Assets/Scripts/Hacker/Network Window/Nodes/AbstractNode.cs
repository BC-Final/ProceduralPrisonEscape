using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using Gamelogic.Extensions;

[SelectionBase]
public abstract class AbstractNode : MonoBehaviour {
	private static List<AbstractNode> _graph = new List<AbstractNode>();
	public static List<AbstractNode> Graph { get { return _graph; } }

	[SerializeField]
	private List<AbstractNode> _connections;

	[SerializeField]
	private bool _hackable;

	[SerializeField]
	private float _hackTime;
	private ObservedValue<bool> _hacked = new ObservedValue<bool>(false);
	public ObservedValue<bool> Hacked {
		get { return _hacked; }
	}

	private bool _protected;
	public bool Protected { get { return _protected; } }

	[SerializeField]
	private float _adminProtectTime;

	[SerializeField]
	private float _hackedAlarmPacketChance;

	[SerializeField]
	private float _feedbackIntervall;

	private Tweener _moveTweener;

	private Coroutine _hackCoroutine;

	private SecurityNode _nearestSecurityNode;

	private int _id;
	public int Id {
		get { return _id; }
		set { _id = value; }
	}

	public virtual void SetAccessible (bool pAccessible) {
		if (pAccessible) {
			GetComponent<Image>().color = Color.white;
		} else {
			GetComponent<Image>().color = Color.gray;
		}
	}


	public List<AbstractNode> GetConnections () {
		return new List<AbstractNode>(_connections);
	}

	public void AddConnection (AbstractNode pNode) {
		if (!_connections.Contains(pNode)) {
			_connections.Add(pNode);
		}
	}

	public void RemoveConnection (AbstractNode pNode) {
		if (_connections.Contains(pNode)) {
			_connections.Remove(pNode);
		}
	}

	void OnMouseDown () {
		FindObjectOfType<HackerAvatar>().OnClickNode(this);
	}

	protected virtual void Awake () {
		_graph.Add(this);
	}

	protected virtual void Start () {
		if (_connections.Count > 4) {
			Debug.LogError("Too many connections on Node: " + gameObject.name + "; Only 4 connections allowed.");
		}

		foreach (AbstractNode n in _connections) {
			if (n != null) {
				GameObject go = Instantiate(HackerReferenceManager.Instance.ConnenctionLine) as GameObject;
				go.transform.parent = transform;
				go.layer = gameObject.layer;
				LineRenderer lr = go.GetComponent<LineRenderer>();
				lr.SetPosition(0, transform.position);
				lr.SetPosition(1, transform.position + (n.transform.position - transform.position) / 2.0f);
			} else {
				Debug.LogWarning("Empty connection at " + transform.name);
			}
		}

		float smallestDistance = Mathf.Infinity;
		foreach (SecurityNode sn in FindObjectsOfType<SecurityNode>()) {
			//TODO Find the nearest security node with the path not overall distance
			float currentDistance = Vector3.Distance(transform.position, sn.transform.position);
			if (currentDistance < smallestDistance) {
				smallestDistance = currentDistance;
				_nearestSecurityNode = sn;
			}

		}

		if (_feedbackIntervall != 0.0f) {
			StartCoroutine(sendFeedback());
		}
	}

	protected virtual void OnDestroy () {
		_graph.Remove(this);
	}


	public virtual void ReceivePacket (AbstractNode pSender, Packet pPacket) { }

	//TODO Add chance to spawn alarm packet
	protected virtual void GotHacked () {
		if (Random.Range(0.0f, 1.0f) < _hackedAlarmPacketChance) {
			GameObject go = (Instantiate(HackerReferenceManager.Instance.AlarmPacket, transform.position, Quaternion.identity, GetComponentInParent<Canvas>().transform) as GameObject);
			go.GetComponent<Packet>().Send(this, _nearestSecurityNode);
		}
	}

	protected virtual void GotUnhacked () { }



	public bool StartHack (AbstractAvatar pAvatar) {
		if (pAvatar is AdminAvatar || _hackable && !_protected) {
			_hackCoroutine = StartCoroutine(ToggleHack(pAvatar));
			return true;
		}

		return false;
	}

	public void AbortHack () {
		_moveTweener.Kill();
		_moveTweener = transform.DORotate(Vector3.zero, 0.5f);
		Protect();

		if (_hackCoroutine != null) {
			StopCoroutine(_hackCoroutine);
		}
	}

	private IEnumerator ToggleHack (AbstractAvatar pAvatar) {
		_moveTweener = transform.DORotate(new Vector3(0, 0, _hacked.Value ? 0.0f : 45.0f), _hackTime);
		yield return new WaitForSeconds(_hackTime);
		_hacked.Value = !_hacked.Value;

		if (_hacked.Value) {
			GotHacked();
		} else {
			GotUnhacked();
		}

		pAvatar.HackFinished();
	}


	public void Protect () {
		StartCoroutine(protect());
	}

	private IEnumerator protect () {
		_protected = true;
		transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
		transform.DOScale(Vector3.one, _adminProtectTime);
		yield return new WaitForSeconds(_adminProtectTime);
		_protected = false;
	}

	private IEnumerator sendFeedback () {
		yield return new WaitForSeconds(_feedbackIntervall);
		GameObject go = (Instantiate(HackerReferenceManager.Instance.FeedbackPacket, transform.position, Quaternion.identity, GetComponentInParent<Canvas>().transform) as GameObject);
		go.GetComponent<Packet>().Send(this, _nearestSecurityNode);
		StartCoroutine(sendFeedback());
	}

	public virtual void SetReferences (int pAssocId) { }

	protected virtual void changedState () {
		NetworkWindow.Instance.RecalculateAccesibleNodes();
	}
}
