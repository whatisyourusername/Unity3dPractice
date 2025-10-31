using UnityEngine;
using UnityEngine.UI;

public class Popup_moveBack : MonoBehaviour
{
    [SerializeField] TweenAnimation[] popupPanels;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(moveBack);
    }

    // Update is called once per frame
    void moveBack()
    {
        for (int i = 0; i < popupPanels.Length; i++)
        {
            popupPanels[i].moveBack();
        }
    }
}
