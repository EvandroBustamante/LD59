using UnityEngine;
using DG.Tweening;

public class UIToggleTween : MonoBehaviour
{
    public GameObject target;
    public GameObject timerTarget;
    public RectTransform rectTransform;

    public float duration = 0.3f;

    public void Toggle()
    {
        AudioManager.Instance.PlayClickUI();
        if (target.activeSelf)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    void Show()
    {
        target.SetActive(true);

        rectTransform.localScale = Vector3.zero;
        

        rectTransform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
        
    }

    void Hide()
    {
        rectTransform.DOScale(Vector3.zero, duration).SetEase(Ease.InBack);
        
        {
            Sequence seq = DOTween.Sequence();

            seq.Append(rectTransform.DOScale(Vector3.zero, duration).SetEase(Ease.InBack));

            seq.OnComplete(() =>
            {
                target.SetActive(false);
            });

            /*if (timerTarget) timerTarget.GetComponent<Timer>().elapsedTime = 0;*/

        }

    }

}
