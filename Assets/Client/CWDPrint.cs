
using System;
using UnityEngine;

public class CWDPrint : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text text;
    void Start()
    {
        text.text = Environment.CurrentDirectory;
    }
}
