using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class TweenTextAlpha : MonoBehaviour
{
    [SerializeField] float fadeTime = 0.6f;   // 페이드 인/아웃 시간
    [SerializeField] float showDuration = 1f; // 보여지는 시간
    [SerializeField] Image panel;             // 같이 페이드 처리할 패널

    private TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        if (textMeshPro == null)
        {
            Debug.LogWarning("TextMeshProUGUI 컴포넌트가 없습니다!");
        }
    }

    private void OnEnable()
    {
        if (textMeshPro == null) return;

        // 초기 알파 0으로 세팅
        textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 0f);
        if (panel != null)
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0f);

        // DOTween 시퀀스
        Sequence seq = DOTween.Sequence();

        // 1) 페이드 인
        seq.Append(textMeshPro.DOFade(1f, fadeTime).SetEase(Ease.OutQuad));
        if (panel != null)
            seq.Join(panel.DOFade(0.4f, fadeTime).SetEase(Ease.OutQuad));

        // 2) 보여지는 시간 유지
        seq.AppendInterval(showDuration);

        // 3) 페이드 아웃
        seq.Append(textMeshPro.DOFade(0f, fadeTime).SetEase(Ease.InQuad));
        if (panel != null)
            seq.Join(panel.DOFade(0f, fadeTime).SetEase(Ease.InQuad));

        // 4) 비활성화
        seq.OnComplete(() =>
        {
            panel.gameObject.SetActive(false);
        });

    }
}
