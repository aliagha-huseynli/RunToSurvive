using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthChestAnimation : MonoBehaviour
{
    public Sprite[] AnimationFrames;
    private SpriteRenderer _spriteRenderer;
    private float _time = 0;
    private int _animationFrameCounter;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time > 0.01f)
        {
            _spriteRenderer.sprite = AnimationFrames[_animationFrameCounter++];
            if (AnimationFrames.Length == _animationFrameCounter)
            {
                _animationFrameCounter = AnimationFrames.Length - 1;
                //_animationFrameCounter = 0;
            }

            //_time = 0;
        }
    }
}
