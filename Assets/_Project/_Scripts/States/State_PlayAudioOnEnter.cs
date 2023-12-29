using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_PlayAudioOnEnter : MonoState
{
    [SerializeField] private AudioClip _clip;
    [SerializeField] private bool _randomizePitch;
    [SerializeField] private float _volume = 1f;
    protected override void OnEnter()
    {
        base.OnEnter();
        AudioCenter.Instance.Play(_clip,_volume,_randomizePitch);
    }
}
