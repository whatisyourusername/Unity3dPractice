using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition_Scene : MonoBehaviour
{
    [SerializeField] private Image whitePanel;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float waitSeconds;
    private void Start()
    {
        whiteTransition();
    }
    public void whiteTransition()
    {
        StartCoroutine(transition());
    }

    private IEnumerator transition()
    {
        yield return new WaitForSeconds(waitSeconds);

        whitePanel.gameObject.SetActive(true); // 패널 켜기
        float elapsed = 0f;
        Color startColor = whitePanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            whitePanel.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        whitePanel.color = new Color(startColor.r, startColor.g, startColor.b, 1f);

        // 씬 전환
        SceneManager.LoadScene("ChosunScene");
    }
}
