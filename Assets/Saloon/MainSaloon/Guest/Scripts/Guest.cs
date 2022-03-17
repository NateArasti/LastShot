using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(VerticalMovement))]
public class Guest : MonoBehaviour
{
    private static bool _dialogueStarted;

    private Character _character;
    private UnityAction _onClick;

    private bool _onChair;
    private SpriteRenderer _spriteRenderer;
    private VerticalMovement _movement;
    private Animator _animator;

    private static readonly int Sit = Animator.StringToHash("Sit");
    private static readonly int OutlineEnabledProperty = Shader.PropertyToID("_OutlineEnabled");

    public TextAsset CurrentDialogue => _character.Dialogue;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _movement = GetComponent<VerticalMovement>();
    }

    public void GuestStart(Character character, float newPosX, UnityEvent<Guest> onClick)
    {
        _character = character;
        StartCoroutine(Move(newPosX));
        _onClick += () => onClick.Invoke(this);
    }

    private IEnumerator Move(float newX, float awaitSeconds = 0)
    {
        yield return new WaitForSeconds(awaitSeconds);
        _spriteRenderer.flipX = awaitSeconds != 0;
        _movement.TryStartMovement(newX);
        yield return new WaitUntil(() => ! _movement.IsMoving);
        _animator.SetBool(Sit, true);
        yield return new WaitForSeconds(1f);
        _onChair = true;
    }

    private void OnMouseDown()
    {
        if (_dialogueStarted || ! _onChair) return;
        _onClick.Invoke();
        _dialogueStarted = true;
        _spriteRenderer.material.SetFloat(OutlineEnabledProperty, 0);
    }

    private void OnMouseEnter()
    {
        if (_onChair && !_dialogueStarted)
            _spriteRenderer.material.SetFloat(OutlineEnabledProperty, 1);
    }

    private void OnMouseExit()
    {
        if (_onChair && !_dialogueStarted)
            _spriteRenderer.material.SetFloat(OutlineEnabledProperty, 0);
    }

    public void EndVisit(float exitPosX)
    {
        _dialogueStarted = false;
        _animator.SetBool(Sit, false);
        StartCoroutine(Move(exitPosX, awaitSeconds: 1.1f));
    }

    public bool GetGuestChoice() => _character.GetGuestChoice();

    public CharacterGuestGrade GetCharacterGrade() => _character.GetCharacterGrade();
}