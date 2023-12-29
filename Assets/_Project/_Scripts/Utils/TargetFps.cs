using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFps : MonoBehaviour
{
    [SerializeField] private int _targetFrameRate;
    private void Awake()
    {
        Application.targetFrameRate = _targetFrameRate;
    }
}
