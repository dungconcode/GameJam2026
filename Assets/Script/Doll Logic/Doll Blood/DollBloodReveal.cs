using System.Collections;
using UnityEngine;

public class DollBloodReveal : MonoBehaviour
{
    [Header("Renderers")]
    [SerializeField] private SpriteRenderer baseRenderer; 
    [SerializeField] private SpriteRenderer bloodRenderer; 

    [Header("Timing")]
    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float visibleTime = 0.1f;
    [SerializeField] private float fadeOutTime = 1f;

    [Header("Base fade (dim while blood shows)")]
    [Range(0f, 1f)]
    [SerializeField] private float baseDimAlpha = 0.35f;  
    [SerializeField] private float baseDimTime = 0.15f;   
    [SerializeField] private float baseRestoreTime = 0.2f;

    private Coroutine co;
    private bool isRevealing = false;
    private void Awake()
    {
        if (bloodRenderer == null)
        {
            Debug.LogError($"{name}: Missing bloodRenderer reference!");
            return;
        }
        if (baseRenderer == null)
        {
            baseRenderer = GetComponent<SpriteRenderer>();
        }
        bloodRenderer.enabled = false;
        SetAlpha(bloodRenderer, 0f);

        if (baseRenderer != null)
            SetAlpha(baseRenderer, 1f);
    }

    public void RevealOnce()
    {
        if (bloodRenderer == null) return;
        if (isRevealing) return;

        if (co != null) StopCoroutine(co);
        co = StartCoroutine(RevealRoutine());
    }

    private IEnumerator RevealRoutine()
    {
        isRevealing = true;
        if (baseRenderer != null)
            yield return Fade(baseRenderer, 1f, baseDimAlpha, baseDimTime);
        bloodRenderer.enabled = true;
        yield return Fade(bloodRenderer, 0f, 1f, fadeInTime);
        if (visibleTime > 0f)
            yield return new WaitForSeconds(visibleTime);
        yield return Fade(bloodRenderer, 1f, 0f, fadeOutTime);
        bloodRenderer.enabled = false;
        if (baseRenderer != null)
            yield return Fade(baseRenderer, baseDimAlpha, 1f, baseRestoreTime);
        yield return new WaitForSeconds(1f);
        isRevealing = false;
        co = null;
    }
    private IEnumerator Fade(SpriteRenderer r, float from, float to, float duration)
    {
        if (r == null) yield break;

        if (duration <= 0f)
        {
            SetAlpha(r, to);
            yield break;
        }
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / duration);
            SetAlpha(r, a);
            yield return null;
        }
        SetAlpha(r, to);
    }
    private void SetAlpha(SpriteRenderer r, float a)
    {
        if (r == null) return;

        Color c = r.color;
        c.a = a;
        r.color = c;
    }
}
