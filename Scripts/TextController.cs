using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour
{
    [SerializeField] private SlowlyAppearingText textField;

    public void SetText(string text)
    {
        textField.SetTrgetText(text);
    }
}
