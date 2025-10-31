using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TweenAlpha : MonoBehaviour
{
    private RawImage image;
    [SerializeField] float duration = 1.35f;

    void Start()
    {
        image = GetComponent<RawImage>();
    }

    public void FadeOut()
    {
        // 알파값 0까지 duration 시간 동안 페이드아웃
        image.DOFade(0f, duration);
    }

    public void FadeIn()
    {
        // 알파값 1까지 duration 시간 동안 페이드인
        image.DOFade(1f, duration);
    }
}
