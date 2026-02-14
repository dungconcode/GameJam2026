using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MirrorZoomController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject mirrorOverlay;     
    [SerializeField] private Button mirrorUIButton;        
    
    // --- THÊM DÒNG NÀY ---
    [Header("Effect References")]
    [SerializeField] private MirrorReflectionEffect reflectionEffect; // Kéo script MirrorReflectionEffect vào đây
    // ---------------------

    [Header("Zoom Settings")]
    [SerializeField] private float zoomScale = 2.0f;
    [SerializeField] private float animDuration = 0.3f;
    [SerializeField] private Vector3 targetPosition = Vector3.zero;

    private Vector3 originalPos;
    private Vector3 originalScale;
    private Quaternion originalRot;
    private SpriteRenderer sr;
    private bool isAnimating = false;

    void Start()
    {
        originalPos = transform.position;
        originalScale = transform.localScale;
        originalRot = transform.rotation;
        sr = GetComponent<SpriteRenderer>();

        if (mirrorOverlay != null) mirrorOverlay.SetActive(false);
        if (mirrorUIButton != null)
        {
            mirrorUIButton.gameObject.SetActive(false);
            mirrorUIButton.onClick.AddListener(CloseMirror);
        }
    }

    private void OnMouseDown()
    {
        if (isAnimating || (mirrorOverlay != null && mirrorOverlay.activeSelf)) return;
        StartCoroutine(ZoomInRoutine());
    }

    public void CloseMirror()
    {
        if (isAnimating) return;
        StartCoroutine(ZoomOutRoutine());
    }

    IEnumerator ZoomInRoutine()
    {
        isAnimating = true;

        // ... (Giữ nguyên đoạn code di chuyển gương) ...
        float time = 0;
        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(0, 0, 0);

        while (time < animDuration)
        {
            float t = time / animDuration;
            t = t * t * (3f - 2f * t);

            transform.position = Vector3.Lerp(startPos, targetPosition, t);
            transform.localScale = Vector3.Lerp(startScale, originalScale * zoomScale, t);
            transform.rotation = Quaternion.Lerp(startRot, targetRot, t);

            time += Time.deltaTime;
            yield return null;
        }
        
        transform.position = targetPosition;
        transform.localScale = originalScale * zoomScale;
        transform.rotation = targetRot;

        // --- ĐOẠN NÀY LÀ LÚC GƯƠNG ĐÃ PHÓNG TO XONG ---
        
        if (sr != null) sr.enabled = false; // Ẩn gương thật đi (bạn đã có)
        
        if (mirrorOverlay != null) mirrorOverlay.SetActive(true); // Hiện Overlay nền đen
        if (mirrorUIButton != null) mirrorUIButton.gameObject.SetActive(true); 

        // --- THÊM DÒNG NÀY: Kích hoạt hiệu ứng hình ảnh Doll ---
        if (reflectionEffect != null)
        {
            reflectionEffect.ShowReflection();
        }
        // -------------------------------------------------------

        isAnimating = false;
    }

    IEnumerator ZoomOutRoutine()
    {
        isAnimating = true;

        // --- THÊM DÒNG NÀY: Tắt hiệu ứng Doll ngay khi bắt đầu đóng ---
        if (reflectionEffect != null)
        {
            reflectionEffect.HideReflection();
        }
        // -------------------------------------------------------------

        if (mirrorOverlay != null) mirrorOverlay.SetActive(false); // Tắt nền đen
        if (mirrorUIButton != null) mirrorUIButton.gameObject.SetActive(false);
        
        if (sr != null) sr.enabled = true; // Hiện lại gương thật

        // ... (Giữ nguyên đoạn code di chuyển gương về cũ) ...
        float time = 0;
        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;
        Quaternion startRot = transform.rotation;

        while (time < animDuration)
        {
            float t = time / animDuration;
            t = t * t * (3f - 2f * t); 

            transform.position = Vector3.Lerp(startPos, originalPos, t);
            transform.localScale = Vector3.Lerp(startScale, originalScale, t);
            transform.rotation = Quaternion.Lerp(startRot, originalRot, t);

            time += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
        transform.localScale = originalScale;
        transform.rotation = originalRot;

        isAnimating = false;
    }
}