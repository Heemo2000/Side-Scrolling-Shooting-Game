using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class HealthLostVisual : GenericSingleton<HealthLostVisual>
{
    [Range(0.1f,1f)]
    [SerializeField]private float alphaUpdateTime = 0.2f;
    private Image _healthLostImage;

    private Coroutine _showCoroutine;
    protected override void Awake() {
        base.Awake();
        _healthLostImage = GetComponent<Image>();
    }

    private void Start() {
        _healthLostImage.color = new Color(_healthLostImage.color.r,_healthLostImage.color.g,_healthLostImage.color.b,0f);
    }

    public void ShowVisual()
    {
        if(_showCoroutine != null)
        {
            StopCoroutine(_showCoroutine);
        }

        _showCoroutine = StartCoroutine(ShowVisual(alphaUpdateTime));
    }
    
    private IEnumerator ShowVisual(float alphaUpdateTime)
    {
        float currentAlpha = 1f;
        Color currentColor = _healthLostImage.color;

        WaitForSeconds wait = new WaitForSeconds(alphaUpdateTime);
        while(currentAlpha >= 0)
        {
            currentColor.a = currentAlpha;
            _healthLostImage.color = currentColor;
            currentAlpha -= 0.1f;
            yield return wait;
        }
        
        currentColor.a = 0f;
        _healthLostImage.color = currentColor;
        yield return null;
    }
}
