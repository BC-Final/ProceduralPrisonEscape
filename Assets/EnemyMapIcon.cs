using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMapIcon : AbstractMapIcon
{
    public static void CreateInstance(Vector2 pPos, float rot)
    {
        GameObject go = (GameObject)Instantiate(HackerReferenceManager.Instance.DroneIcon, new Vector3(pPos.x / MinimapManager.scale, pPos.y / MinimapManager.scale, 0), Quaternion.Euler(0, rot, 0));
    }

    private float _health;

    private Vector3 _oldPos;
    private Vector3 _newPos;

    private Quaternion _oldRot;
    private Quaternion _newRot;

    private float _lastUpdateTime = 0.0f;

    private float _currentLerpTime = 0.0f;
    private float _lerpTime = 0.5f;

    // Update is called once per frame
    private void Update()
    {
        _currentLerpTime += Time.deltaTime;
        if (_currentLerpTime > _lerpTime)
        {
            _currentLerpTime = _lerpTime;
        }

        if (_lerpTime == 0.0f)
        {
            _lerpTime = 0.5f;
        }

        float perc = _currentLerpTime / _lerpTime;

        transform.position = Vector3.Lerp(_oldPos, _newPos, perc);
        transform.rotation = Quaternion.Slerp(_oldRot, _newRot, perc);
    }

    private void InitialTransform(Vector3 pPos, float pRot)
    {
        _lastUpdateTime = Time.time;

        _oldPos = pPos;
        _newPos = pPos;

        _oldRot = Quaternion.Euler(0, pRot, 0);
        _newRot = Quaternion.Euler(0, pRot, 0);
    }

    private void UpdateTransform(Vector3 pPos, float pRot)
    {
        _lerpTime = Time.time - _lastUpdateTime;
        _lastUpdateTime = Time.time;
        _currentLerpTime = 0f;

        _oldPos = transform.position;
        _newPos = pPos;

        _oldRot = transform.rotation;
        _newRot = Quaternion.Euler(0, pRot, 0);
    }

    private void UpdateHealth(float nHealth)
    {
        if(_health <= 0)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    public void UpdateEnemy(CustomCommands.Update.DroneUpdate package)
    {
        UpdateTransform(new Vector3(package.x, package.z, 0),package.rotation);
        UpdateHealth(package.hpPercent);
    }
}

