using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
    [SerializeField] private float typingSpeed = 20f;

    public bool IsTypingEnded { get; private set; } = true;

    private TMP_Text currentTextLabel;
    private string currentPhrase;
    private Coroutine currentTypeCoroutine;

    public void StartTypingPhrase(string phrase, TMP_Text textLabel, string coloredPhrase)
    {
        currentPhrase = coloredPhrase;
        currentTextLabel = textLabel;
        currentTextLabel.text = string.Empty;
        IsTypingEnded = false;
        currentTypeCoroutine = StartCoroutine(Write(phrase));
    }

    private IEnumerator Write(string phrase)
    {
        var t = 0f;
        var letterIndex = 0;
        var textLength = phrase.Length;
        while (letterIndex < textLength)
        {
            t += Time.deltaTime * typingSpeed;
            letterIndex = Mathf.FloorToInt(t);
            letterIndex = Mathf.Clamp(letterIndex, 0, textLength);

            currentTextLabel.text = phrase.Substring(0, letterIndex);

            yield return null;
        }
        EndTyping();
    }

    public void EndTyping()
    {
        if (currentTextLabel == null || currentPhrase == null) 
            return;
        StopCoroutine(currentTypeCoroutine);
        currentTextLabel.text = currentPhrase;
        currentTextLabel = null;
        currentPhrase = null;
        IsTypingEnded = true;
    }
}
