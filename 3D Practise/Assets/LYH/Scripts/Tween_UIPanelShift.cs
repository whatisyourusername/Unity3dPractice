using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Tween_UIPanelShift : MonoBehaviour
{
    [SerializeField] Button charButton;
    [SerializeField] Button lobbyButton;
    [SerializeField] Button dailyButton;
    [SerializeField] Button collectButton;
    [SerializeField] Button gatchaButton;

    [SerializeField] TweenAnimation charUI;
    [SerializeField] TweenAnimation lobbyUI;

    [SerializeField] TweenAlpha bgVideo;

    [SerializeField] ExpandMenu charPanel;
    [SerializeField] ExpandMenu charBGPanel;

    [SerializeField] float delayTime = 1.35f;

    private bool isBusy = false; // 입력 잠금 상태
    private bool isHome = true;

    void Start()
    {
        charButton.onClick.AddListener(CharMove);
        lobbyButton.onClick.AddListener(LobbyMove);
    }

    void CharMove()
    {
        if (isBusy) return;
        isBusy = true;
        // Home UI 상태라면 트윈동안 버튼 전부 비활성화
        if(isHome) SetButtonsInteractable(false);

        charUI.moveAway();
        lobbyUI.moveAway();
        bgVideo.FadeOut();

        DOVirtual.DelayedCall(delayTime + 0.4f, () =>
        {
            if (charPanel.isExpanded) charPanel.isExpanded = false;
            charPanel.ToggleExpand();

            if (charBGPanel.isExpanded) charBGPanel.isExpanded = false;
            charBGPanel.ToggleExpand();

            // 버튼 다시 활성화
            SetButtonsInteractable(true);
            isHome = false;
            isBusy = false;
        });
    }

    void LobbyMove()
    {
        if (isBusy) return;
        isBusy = true;
        // Char UI 상태라면 트윈동안 버튼 전부 비활성화
        if (!isHome) SetButtonsInteractable(false);

        if (!charPanel.isExpanded) charPanel.isExpanded = true;
        charPanel.ToggleExpand();

        if (!charBGPanel.isExpanded) charBGPanel.isExpanded = true;
        charBGPanel.ToggleExpand();

        DOVirtual.DelayedCall(delayTime, () =>
        {
            charUI.moveBack();
            lobbyUI.moveBack();
            bgVideo.FadeIn();

            // 버튼 다시 활성화
            SetButtonsInteractable(true);
            isHome = true;
            isBusy = false;
        });
    }

    // 버튼 활성/비활성 일괄 제어 함수
    private void SetButtonsInteractable(bool value)
    {
        charButton.interactable = value;
        lobbyButton.interactable = value;
        dailyButton.interactable = value;
        collectButton.interactable = value;
        gatchaButton.interactable = value;
    }
}
