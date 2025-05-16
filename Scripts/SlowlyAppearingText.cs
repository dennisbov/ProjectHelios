using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SlowlyAppearingText : MonoBehaviour
{
    [TextArea][SerializeField] private string _targetText;
    [SerializeField] private float _symbolPlacementPeriod;
    [SerializeField] private float _timeToDisapear;
    
    private TextMeshProUGUI _textMeshProUGUI;
    private string _currentText;
    private int _indexOfLastSymbol;

    private void OnEnable()
    {
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private float _expiredTime = 0;
    private void Update()
    {
        _expiredTime += Time.deltaTime;
        if(_expiredTime > _symbolPlacementPeriod && _indexOfLastSymbol < _targetText.Length)
        {
            _expiredTime = 0;
            _currentText += _targetText[_indexOfLastSymbol];
            _textMeshProUGUI.text = _currentText+"_";
            _indexOfLastSymbol++;
        }
        if(_indexOfLastSymbol >= _targetText.Length && _expiredTime > _timeToDisapear)
        {
            _textMeshProUGUI.text = "";
            _currentText = "";
            _indexOfLastSymbol = 0;
            _targetText = "";
        }
    }

    public void SetTrgetText(string text)
    {
        _targetText = text;
        _currentText = "";
        _textMeshProUGUI.text = "";
        _expiredTime = 0;
        _indexOfLastSymbol = 0;
    }
}
