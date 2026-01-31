using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IntroLetterButton : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform letter;
    [SerializeField] private RectTransform startPoint;
    [SerializeField] private RectTransform targetPoint;

    [Header("Timing")]
    [SerializeField] private float slideTime = 0.6f;

    [Header("Flow")]
    [SerializeField] private GameManager gameManager;

    public Button button;

    private void Awake()
    {
        if (letter == null) letter = transform as RectTransform;
    }

    private void OnEnable()
    {
        // khóa click khi đang trượt
        button.interactable = false;

        if (startPoint != null)
            letter.anchoredPosition = startPoint.anchoredPosition;

        StartCoroutine(SlideIn());
    }

    private IEnumerator SlideIn()
    {
        if (targetPoint == null)
        {
            button.interactable = true;
            yield break;
        }

        Vector2 from = letter.anchoredPosition;
        Vector2 to = targetPoint.anchoredPosition;

        float t = 0f;
        while (t < slideTime)
        {
            t += Time.deltaTime;
            float k = Mathf.SmoothStep(0f, 1f, t / slideTime);
            letter.anchoredPosition = Vector2.Lerp(from, to, k);
            yield return null;
        }

        letter.anchoredPosition = to;

        button.interactable = true;
    }
    public void OnLetterClicked()
    {
        Debug.Log("IntroLetterButton: Letter Clicked");
        gameManager.OnLetterPressed();
    }
}
