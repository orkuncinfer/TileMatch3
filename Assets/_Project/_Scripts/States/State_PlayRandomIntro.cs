using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class State_PlayRandomIntro : MonoState
{
    [SerializeField] private Animator _animator;

    protected override void OnEnter()
    {
        base.OnEnter();

        int randomIndex = Random.Range(1, 5);
        string triggerIndex = randomIndex.ToString();

        float randomSpeed = Random.Range(1, 1.6f);
        
        _animator.SetTrigger(triggerIndex);
        _animator.speed = randomSpeed;
    }
}
