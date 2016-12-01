using UnityEngine;
using System.Collections;

public class LaserShot : MonoBehaviour {

	private LineRenderer _renderer;
	private Color _color;
	private float _currentTime;
	[SerializeField]
	private float _endTime = 0.5f;

	// Use this for initialization
	void Start () {
		_renderer = GetComponent<LineRenderer>();
		_renderer.SetPosition(0, transform.position);
		_color = Color.red;
		
    }
	
	// Update is called once per frame
	void Update () {
		_currentTime += Time.deltaTime;
		float perc = _currentTime / _endTime;
		float value = Mathf.Clamp(1 - perc, 0, 1);
		_color.a = value;
		_renderer.material.color = _color;
        if (value <= 0)
		{
			DestroyObject(this.gameObject);
		}
	}

	public void SetTargetPos(Vector3 targetPos)
	{
		if(_renderer == null)
		{
			_renderer = GetComponent<LineRenderer>();
		}
		_renderer.SetPosition(1, targetPos);
	}
}
