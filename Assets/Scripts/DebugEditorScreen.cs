using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugEditorScreen : MonoBehaviour
{
    public static DebugEditorScreen Instance;
    [SerializeField]
    private TextMeshProUGUI _text;

    private void Awake()
    {
        Instance = this;
    }

    public void DisplayValue(string value)
    {
        _text.text = value;
    }
}
