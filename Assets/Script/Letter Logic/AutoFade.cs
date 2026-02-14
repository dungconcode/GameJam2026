using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class AutoFade : MonoBehaviour
{
    [SerializeField] private float thoiGianHien = 1.5f; // Thời gian fade
    private CanvasGroup group;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    // Mỗi khi object được bật lên, hàm này sẽ chạy
    private void OnEnable()
    {
        if (group != null)
        {
            group.alpha = 0f; // Đặt về trong suốt
            StartCoroutine(FadeProcess());
        }
    }

    private IEnumerator FadeProcess()
    {
        float timer = 0f;
        while (timer < thoiGianHien)
        {
            timer += Time.deltaTime;
            group.alpha = timer / thoiGianHien;
            yield return null;
        }
        group.alpha = 1f; // Hiện rõ 100%
    }
}