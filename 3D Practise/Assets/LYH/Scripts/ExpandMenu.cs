using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ExpandMenu : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] float tweenTime;
    [SerializeField] float panelHeight = 0f;
    [SerializeField] GameObject panel;
    [SerializeField] Image triangle;

    private RectTransform rectTransform;
    private float originalHeight;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // 원래 높이 기억
        originalHeight = rectTransform.sizeDelta.y;

        if (button != null) button.onClick.AddListener(ToggleExpand);
    }

    public bool isExpanded = false;

    public void ToggleExpand()
    {
        if (panel != null)
        {
            panelHeight = ((RectTransform)panel.transform).rect.height;
        }

        if (isExpanded)
        {
            // 원래 높이로 줄이기
            rectTransform.DOSizeDelta(new Vector2(rectTransform.sizeDelta.x, originalHeight), tweenTime);
            if (triangle != null) triangle.rectTransform.DOScale(new Vector3(1, -1, 1),tweenTime);
        }
        else
        {
            // panel의 현재 높이만큼 확장
            rectTransform.DOSizeDelta(new Vector2(rectTransform.sizeDelta.x, panelHeight), tweenTime);
            if (triangle != null) triangle.rectTransform.DOScale(new Vector3(1, 1, 1), tweenTime);
        }

        isExpanded = !isExpanded;
    }

    public void ToggleShrink()
    {
        if (!isExpanded) return;

        rectTransform.DOSizeDelta(new Vector2(rectTransform.sizeDelta.x, originalHeight), tweenTime);
        if (triangle != null) triangle.rectTransform.DOScale(new Vector3(1, -1, 1), tweenTime);
        isExpanded = false;
    }
}
