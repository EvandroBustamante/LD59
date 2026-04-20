using UnityEngine;
using DG.Tweening;

public class UIToggleTween : MonoBehaviour
{
    public GameObject target;
    public RectTransform rectTransform;

    public float duration = 0.3f;

    public void Toggle()
    {
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

        }
    }

}
