using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBar : MonoBehaviour
{

    private PlayerBehaviour _PB;
    private RectTransform _Blackbar;

    [SerializeField]
    private int _playerNumber;

    [SerializeField]
    private RectTransform[] ChargeIndicators;

    void Start()
    {
        //safeties
        if (_playerNumber == 0)
        {
            Debug.LogError("a health bar has no target player number assigned! Set it to either 1 or 2!");
        }
        if (_PB == null)
        {
            _PB = GameObject.Find("Player " + _playerNumber).GetComponent<PlayerBehaviour>();
        }
        //transform.find looks in children, in contrast to gameobject.find, who looks in everything
        _Blackbar = transform.Find("Charge").GetComponent<RectTransform>();
        if (_playerNumber == 1)
        {
            _Blackbar.pivot = new Vector2(0, 0.5f);
        }
        else
        {
            _Blackbar.pivot = new Vector2(1, 0.5f);
        }

        for (int i = 0; i < _PB.ChargeValues.Length; i++)
        {
            float x = Mathf.Lerp(0.01f, 0.99f, _PB.ChargeValues[i] / _PB.ChargeValues[_PB.ChargeValues.Length - 1]);
            x = _playerNumber == 1 ? x : 1-x;
            ChargeIndicators[i].anchorMin = new Vector2(x, 0.0f);
            ChargeIndicators[i].anchorMax = new Vector2(x, 0.9f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerNumber == 1)
        {
            //_Blackbar.localScale = new Vector3(Mathf.Clamp(_PB._charge / 3.5f,0,1), _Blackbar.localScale.y, _Blackbar.localScale.z);
            //_Blackbar.anchorMin = new Vector2(1, 0);
            float relativecharge = Mathf.Clamp(_PB._charge / _PB.ChargeValues[_PB.ChargeValues.Length - 1], 0, 1);
            Debug.Log(relativecharge);
            _Blackbar.anchorMax = new Vector2(Mathf.Lerp(0.01f, 0.99f, relativecharge), _Blackbar.anchorMax.y);
            _Blackbar.sizeDelta = Vector2.one;
        }
        else
        {
            float relativecharge = Mathf.Clamp(_PB._charge / _PB.ChargeValues[_PB.ChargeValues.Length - 1], 0, 1);
            _Blackbar.anchorMin = new Vector2(Mathf.Lerp(0.99f, 0.01f, relativecharge), _Blackbar.anchorMin.y);
            _Blackbar.sizeDelta = Vector2.one;
        }
    }

}