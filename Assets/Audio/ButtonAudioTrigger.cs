using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAudioTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip _clickSound;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => AudioManager.PlaySound(_clickSound, 0.1f));
    }
}
