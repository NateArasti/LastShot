using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RandomAnimations : MonoBehaviour
{
    [SerializeField] private int _numberOfRandomAnimations;
    [SerializeField] private string _animationPrefix = "Random";
    [SerializeField] private float _delayBeforeRandomAnimation = 15f;
    [Range(0, 1)] [SerializeField] private float _chanceOfRandomAnimation = 0.3f;
    private float _lastRandomAnimationTime;
    private int _lastAnimationIndex;
    private Animator _animator;

    private readonly List<int> _animationIndexes = new();
    private readonly HashSet<int> _currentPossibleAnimations = new();

    private void Start()
    {
        _animator = GetComponent<Animator>();
        for (var i = 1; i <= _numberOfRandomAnimations; i++)
        {
            _animationIndexes.Add(Animator.StringToHash($"{_animationPrefix}{i}"));
            _currentPossibleAnimations.Add(i - 1);
        }
    }

    private void Update()
    {
        if (!(Time.time - _lastRandomAnimationTime > _delayBeforeRandomAnimation)) return;
        _lastRandomAnimationTime = Time.time;
        if(Random.value <= _chanceOfRandomAnimation)
        {
            if (_currentPossibleAnimations.Contains(_lastAnimationIndex))
                _currentPossibleAnimations.Remove(_lastAnimationIndex);
            var newAnimation = _currentPossibleAnimations.GetRandomObject();
            _animator.SetTrigger(_animationIndexes[newAnimation]);
            _currentPossibleAnimations.Add(_lastAnimationIndex);
            _lastAnimationIndex = newAnimation;
        }
    }
}
