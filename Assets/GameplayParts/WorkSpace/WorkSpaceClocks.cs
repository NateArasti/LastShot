using UnityEngine;
using UnityEngine.UI;

public class WorkSpaceClocks : MonoBehaviour
{
    [SerializeField] private Image _clockMaskImage;
    [SerializeField] private Gradient _stagesGradient;
    private float _fillSpeed;
    private bool _clockIsGoing;

    public void StartClock(float totalTime)
    {
        _clockMaskImage.fillAmount = 0;
        _clockIsGoing = true;
        _fillSpeed = 1 / totalTime;
    }

    private void Update()
    {
        if(!_clockIsGoing) return;
        _clockMaskImage.fillAmount = Mathf.MoveTowards(_clockMaskImage.fillAmount, 1, _fillSpeed * Time.deltaTime);
        _clockMaskImage.color = _stagesGradient.Evaluate(_clockMaskImage.fillAmount);
        if (Mathf.Approximately(_clockMaskImage.fillAmount, 1))
        {
            _clockIsGoing = false;
        }
    }
}
