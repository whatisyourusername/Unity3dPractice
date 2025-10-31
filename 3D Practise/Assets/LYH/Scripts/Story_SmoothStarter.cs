using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Story_SmoothStarter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] Image bg;

    private Image self;

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Image>();

        DOVirtual.DelayedCall(3f, () => self.DOFade(1f, 2f).OnComplete(() => { bg.gameObject.SetActive(false); }));
        DOVirtual.DelayedCall(3f, () => title.DOFade(1f, 2f).OnComplete(() =>
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                self.DOFade(0f, 2f);
                title.DOFade(0f, 2f).OnComplete(() =>
                {
                    self.gameObject.SetActive(false);
                    title.gameObject.SetActive(false);
                });
            });
        }));
    }
}
