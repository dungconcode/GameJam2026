using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndBGSequence : MonoBehaviour
{
    [Header("Order: BGStartBlack -> BGEnd_1 -> BGEnd_2.1 -> BGEnd_2 -> ...")]
    [SerializeField] private List<Image> steps = new List<Image>();

    [Header("Timing")]
    [SerializeField] private float fadeDuration = 0.8f;
    [SerializeField] private float holdDuration = 1.2f;
    [SerializeField] private bool playOnEnable = true;

    [Header("Optional")]
    [SerializeField] private bool disablePreviousAfterFade = true;

    [Header("Animation Step (run trigger once, wait finish, then continue)")]
    [Tooltip("Step index trong list steps cần chạy anim (VD: BGEnd_2.1 là element 2 thì để 2). Để -1 nếu không dùng.")]
    [SerializeField] private int animStepIndex = -1;

    [Tooltip("Animator nằm trên BGEnd_2.1 (hoặc parent của nó). Nếu để trống sẽ auto GetComponent trên Image.")]
    [SerializeField] private Animator animAnimator;

    [Tooltip("Trigger name để kích hoạt anim (VD: isAnim / Run / Start...).")]
    [SerializeField] private string animTriggerName = "isAnim";

    [Tooltip("Tên state anim sau khi trigger (VD: JumpScare_Run). Phải đúng tên state trong Animator.")]
    [SerializeField] private string animStateToWait = "JumpScare_Run";

    [SerializeField] private int animLayer = 0;

    private Coroutine sequenceCo;
    private bool animTriggeredOnce = false;

    private void OnEnable()
    {
        if (playOnEnable) Play();
    }

    public void Play()
    {
        if (sequenceCo != null) StopCoroutine(sequenceCo);
        sequenceCo = StartCoroutine(SequenceRoutine());
    }

    private IEnumerator SequenceRoutine()
    {
        if (steps == null || steps.Count == 0) yield break;

        // Init: active tất cả + alpha = 0
        for (int i = 0; i < steps.Count; i++)
        {
            if (!steps[i]) continue;
            steps[i].gameObject.SetActive(true);
            SetAlpha(steps[i], 0f);
        }

        // Step 0 hiện
        Image current = steps[0];
        if (current) SetAlpha(current, 1f);

        yield return new WaitForSeconds(holdDuration);

        // Loop chuyển step
        for (int i = 0; i < steps.Count - 1; i++)
        {
            current = steps[i];
            Image next = steps[i + 1];

            if (next)
            {
                next.gameObject.SetActive(true);
                SetAlpha(next, 0f);
            }

            // Fade sang next
            yield return StartCoroutine(CrossFade(current, next, fadeDuration));

            if (disablePreviousAfterFade && current)
                current.gameObject.SetActive(false);

            // Nếu next chính là step anim -> kích trigger 1 lần + đợi chạy xong
            if (i + 1 == animStepIndex)
            {
                yield return StartCoroutine(PlayTriggerAndWait(next));
            }
            else
            {
                yield return new WaitForSeconds(holdDuration);
            }
        }
    }

    private IEnumerator PlayTriggerAndWait(Image stepImage)
    {
        // đảm bảo animator
        Animator a = animAnimator;
        if (a == null && stepImage != null)
            a = stepImage.GetComponent<Animator>(); // hoặc GetComponentInParent nếu animator đặt ở parent

        if (a == null || string.IsNullOrEmpty(animStateToWait))
        {
            // fallback nếu thiếu animator/state
            yield return new WaitForSeconds(holdDuration);
            yield break;
        }

        // Kích trigger đúng 1 lần
        if (!animTriggeredOnce && !string.IsNullOrEmpty(animTriggerName))
        {
            a.ResetTrigger(animTriggerName); // tránh kẹt do trigger cũ
            a.SetTrigger(animTriggerName);
            animTriggeredOnce = true;
        }

        // Đợi 1 frame cho animator kịp chuyển state
        yield return null;

        // Chờ đến khi vào đúng state (tránh trường hợp đang transition)
        while (true)
        {
            var st = a.GetCurrentAnimatorStateInfo(animLayer);
            if (st.IsName(animStateToWait) && !a.IsInTransition(animLayer))
                break;
            yield return null;
        }

        // Đợi state chạy xong (normalizedTime >= 1)
        while (true)
        {
            var st = a.GetCurrentAnimatorStateInfo(animLayer);

            // nếu đã out khỏi state -> coi như xong
            if (!st.IsName(animStateToWait)) break;

            if (!a.IsInTransition(animLayer) && st.normalizedTime >= 1f)
                break;

            yield return null;
        }

        // anim xong thì giữ thêm chút nếu muốn
        yield return new WaitForSeconds(holdDuration);
    }

    private IEnumerator CrossFade(Image from, Image to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duration);

            if (from) SetAlpha(from, 1f - k);
            if (to) SetAlpha(to, k);

            yield return null;
        }

        if (from) SetAlpha(from, 0f);
        if (to) SetAlpha(to, 1f);
    }

    private void SetAlpha(Image img, float a)
    {
        var c = img.color;
        c.a = a;
        img.color = c;
    }
}
