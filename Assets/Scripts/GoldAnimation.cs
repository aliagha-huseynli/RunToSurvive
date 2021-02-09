using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldAnimation : MonoBehaviour
{
    public Sprite[] AnimationFrames;
    private SpriteRenderer _spriteRenderer;
    private float _time;
    private int _animationFrameCounter;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _time += Time.deltaTime;
        if (_time > 0.04f)
        {
            _spriteRenderer.sprite = AnimationFrames[_animationFrameCounter++];
            if (AnimationFrames.Length == _animationFrameCounter)
            {
                _animationFrameCounter = 0;
            }

            _time = 0;
        }
    }
}
