using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Tween_BlackTransition : MonoBehaviour
{
    public float transitionTime = 1f;
    public float waitTime = 0.5f;
    
    private Image image;

    [SerializeField] GameObject menu;

    void OnEnable()
    {
        menu.SetActive(false);
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); // 초기 알파 0

        // 먼저 페이드 인 (알파 1) → 완료되면 페이드 아웃 (알파 0)
        image.DOFade(1f, transitionTime).OnComplete(() =>
        {
            DOVirtual.DelayedCall(waitTime, () => image.DOFade(0f, transitionTime).OnComplete(() =>
            {
                image.gameObject.SetActive(false);
                menu.SetActive(true);
            }));
        });
    }
}
