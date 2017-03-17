using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaspipeMapIcon : AbstractMapIcon {
    public static void CreateInstance(Vector2 pPos, float pRot, bool pOpen, bool pUsed)
    {
        GameObject go = (GameObject)Instantiate(HackerReferenceManager.Instance.GasPipeIcon, new Vector3(pPos.x / MinimapManager.scale, pPos.y / MinimapManager.scale, 0), Quaternion.Euler(0, pRot, 0));
        go.GetComponent<GaspipeMapIcon>()._used = pUsed;
        go.GetComponent<GaspipeMapIcon>().StateChanged();
    }

    private bool _used = false;

    #region Sprites
    [Header("Sprites")]
    [SerializeField]
    private Sprite _normalSprite;
    [SerializeField]
    private Sprite _usedSprite;
    #endregion

    public void NormalExplosion()
    {
        if (!_used)
        {
            _used = true;
        }

        StateChanged();
    }

    public void BigExplosion()
    {
        if (!_used)
        {
            _used = true;
        }

        StateChanged();
    }

    private void StateChanged()
    {
        if (!_used)
        {
            changeSprite(_normalSprite);
        }else
        {
            changeSprite(_usedSprite);
        }
    }
}
