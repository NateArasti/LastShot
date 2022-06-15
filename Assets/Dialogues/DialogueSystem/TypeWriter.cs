using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
    [SerializeField] private float _typingSpeed = 20f;
    [SerializeField] private AudioClip _typingClip;

    public bool IsTypingEnded { get; private set; } = true;

    private TMP_Text _currentTextLabel;
    private string _currentPhrase;

    public void StartTypingPhrase(string phrase, TMP_Text textLabel, string coloredPhrase)
    {
        _currentPhrase = coloredPhrase;
        _currentTextLabel = textLabel;
        _currentTextLabel.text = string.Empty;
        IsTypingEnded = false;
        StartCoroutine(Write(phrase));
    }

    private IEnumerator Write(string phrase)
    {
        var t = 0f;
        var letterIndex = 0;
        var textLength = phrase.Length;
        var lastIndex = 0;
        while (letterIndex < textLength)
        {
            t += Time.deltaTime * _typingSpeed;
            letterIndex = Mathf.FloorToInt(t);
            letterIndex = Mathf.Clamp(letterIndex, 0, textLength);
            if (letterIndex > lastIndex)
            {
                AudioManager.PlaySound(_typingClip, 0.1f, true, 0.5f);
                lastIndex = letterIndex;
            }
            _currentTextLabel.text = phrase[..letterIndex];

            yield return null;
        }
        EndTyping();
    }

    public void EndTyping()
    {
        if (_currentTextLabel == null || _currentPhrase == null) 
            return;
        StopAllCoroutines();
        _currentTextLabel.text = _currentPhrase;
        _currentTextLabel = null;
        _currentPhrase = null;
        IsTypingEnded = true;
    }
}
