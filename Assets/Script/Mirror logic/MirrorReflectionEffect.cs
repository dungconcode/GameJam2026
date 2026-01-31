using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MirrorReflectionEffect : MonoBehaviour
{
    [Header("References")]
    public DollManager dollManager;       // Kéo DollManager vào đây
    public Image reflectionImage;         // Kéo cái Image (MirrorUI) ở giữa gương vào đây

    [Header("Assets")]
    public Sprite normalSprite;           // Asset 1: Ảnh cho Doll Normal
    public Sprite abnormalGlitchSprite;   // Asset 2: Ảnh Abnormal (Giật giật)
    public Sprite abnormalJumpSprite;     // Asset 3: Ảnh Abnormal (Mờ -> Hù)

    [Header("Settings")]
    public float glitchDuration = 2.0f;   // Thời gian giật
    public float jumpDelay = 1.5f;        // Thời gian chờ trước khi phóng to (Asset 3)

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private Vector3 originalScale;

    private void Awake()
    {
        if (reflectionImage != null)
        {
            rectTransform = reflectionImage.GetComponent<RectTransform>();
            originalPosition = rectTransform.anchoredPosition;
            originalScale = rectTransform.localScale;
            reflectionImage.gameObject.SetActive(false); // Ẩn mặc định
        }
    }

    // Hàm này sẽ được gọi khi BẬT Overlay
    public void ShowReflection()
    {
        // 1. Lấy dữ liệu con doll hiện tại
        DollData data = dollManager.GetCurrentDollData();
        
        if (data == null || reflectionImage == null) return;

        reflectionImage.gameObject.SetActive(true);
        ResetTransform(); // Reset vị trí/scale cũ

        // 2. Kiểm tra loại Doll
        if (data.dollType == DollType.Normal)
        {
            // --- TRƯỜNG HỢP NORMAL ---
            reflectionImage.sprite = normalSprite;
            SetAlpha(1f); // Hiện rõ luôn
        }
        else // DollType.Abnormal
        {
            // --- TRƯỜNG HỢP ABNORMAL (Random 1 trong 2) ---
            int randomChoice = Random.Range(0, 2); // Trả về 0 hoặc 1

            if (randomChoice == 0)
            {
                // Asset 2: Giật giật
                reflectionImage.sprite = abnormalGlitchSprite;
                SetAlpha(1f);
                StartCoroutine(PlayGlitchEffect());
            }
            else
            {
                // Asset 3: Mờ -> Đứng im -> Phóng to (Jumpscare)
                reflectionImage.sprite = abnormalJumpSprite;
                StartCoroutine(PlayJumpScareEffect());
            }
        }
    }

    // Hàm gọi khi TẮT Overlay
    public void HideReflection()
    {
        StopAllCoroutines(); // Dừng mọi hiệu ứng đang chạy
        if (reflectionImage != null)
        {
            reflectionImage.gameObject.SetActive(false);
            ResetTransform();
        }
    }

    // --- CÁC HIỆU ỨNG (COROUTINES) ---

    // 1. Hiệu ứng Giật giật (Glitch)
    IEnumerator PlayGlitchEffect()
    {
        float timer = 0;
        while (timer < glitchDuration)
        {
            // Rung vị trí ngẫu nhiên
            float x = Random.Range(-10f, 10f);
            float y = Random.Range(-10f, 10f);
            rectTransform.anchoredPosition = originalPosition + new Vector2(x, y);

            // Chớp tắt (đổi màu hoặc alpha)
            reflectionImage.color = new Color(1, 1, 1, Random.Range(0.5f, 1f));

            yield return new WaitForSeconds(0.05f); // Tốc độ giật
            timer += 0.05f;
        }
        // Kết thúc giật thì về bình thường
        ResetTransform();
    }

    // 2. Hiệu ứng Mờ -> Phóng to (Jump Scare)
    IEnumerator PlayJumpScareEffect()
    {
        // Giai đoạn 1: Hiện không rõ (Mờ)
        SetAlpha(0.2f); // Rất mờ
        rectTransform.localScale = originalScale; // Kích thước chuẩn

        // Đứng im 1 lúc
        yield return new WaitForSeconds(jumpDelay);

        // Giai đoạn 2: Đột nhiên hiện rõ và phóng to
        SetAlpha(1f); // Rõ nét
        rectTransform.localScale = originalScale * 1.5f; // Phóng to 1.5 lần
        
        // (Tuỳ chọn) Có thể thêm âm thanh hù doạ ở đây
        // AudioSource.PlayClipAtPoint(screamSound, transform.position);
    }

    // --- CÁC HÀM PHỤ TRỢ ---
    void ResetTransform()
    {
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = originalPosition;
            rectTransform.localScale = originalScale;
            SetAlpha(1f);
        }
    }

    void SetAlpha(float alpha)
    {
        if (reflectionImage != null)
        {
            Color c = reflectionImage.color;
            c.a = alpha;
            reflectionImage.color = c;
        }
    }
}