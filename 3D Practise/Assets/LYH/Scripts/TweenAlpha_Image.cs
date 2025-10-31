using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TweenAlpha_Image : MonoBehaviour
{
    private Image image;

    [SerializeField] Button[] pushButtons;
    [SerializeField] float tweenTime = 1f;

    void OnEnable()
    {
        image = GetComponent<Image>();
        
        SetButtonsInteractable(false);

        image.DOFade(0.95f, tweenTime).OnComplete(() => SetButtonsInteractable(true));
    }
    public void FadeOut()
    {
        SetButtonsInteractable(false);

        image.DOFade(0f, tweenTime).OnComplete(() =>
        {
            gameObject.SetActive(false); // 페이드 끝나면 비활성화
            SetButtonsInteractable(true);
        });
    }
    private void SetButtonsInteractable(bool value)
    {
        for (int i = 0; i < pushButtons.Length; i++)
        {
            pushButtons[i].interactable = value;
        }
    }
}
