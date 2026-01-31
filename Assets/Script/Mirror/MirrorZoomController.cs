using System.Collections;
using UnityEngine;

public class MirrorZoomController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform mirror;      // Kéo OpenMirror vào đây
    [SerializeField] private CanvasGroup mirrorOverlay; // Kéo MirrorOverlay vào đây
    [SerializeField] private MirrorDollController dollController; // Kéo object chứa script MirrorDollController vào đây
    [SerializeField] private DollType currentDollType;

    [Header("Animation Settings")]
    [SerializeField] private float durationIn = 0.4f;  // Thời gian phóng to
    [SerializeField] private float durationOut = 0.4f; // Thời gian thu nhỏ
    [SerializeField] private Vector3 zoomScale = new Vector3(2.5f, 2.5f, 1f); // Độ phóng đại

    [Header("Timing")]
    [Tooltip("Thời gian chờ sau khi zoom xong mới hiện animation")]
    [SerializeField] private float animationStartDelay = 0.5f; // Chỉnh số này để animation hiện nhanh hay chậm

    private Vector3 startPos;
    private Vector3 startScale;
    private bool isZoomed;
    private Coroutine currentRoutine;

    void Start()
    {
        if (mirror == null) 
        {
            Debug.LogError("Chưa gán Reference cho MirrorZoomController!");
            return;
        }

        startPos = mirror.position;
        startScale = mirror.localScale;

        if(mirrorOverlay != null)
        {
            mirrorOverlay.gameObject.SetActive(false);
            mirrorOverlay.alpha = 0f;
        }
        
        // Luôn ẩn doll khi bắt đầu game
        if(dollController != null) dollController.HideDoll();
    }

    public void ToggleMirror()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        if (!isZoomed)
            currentRoutine = StartCoroutine(ZoomIn());
        else
            currentRoutine = StartCoroutine(ZoomOut());
    }

    private IEnumerator ZoomIn()
    {
        isZoomed = true;

        if(mirrorOverlay != null) mirrorOverlay.gameObject.SetActive(true);

        Vector3 targetPos = new Vector3(Screen.width / 2f, Screen.height / 2f, mirror.position.z);

        float time = 0f;

        // 1. Thực hiện Zoom
        while (time < durationIn)
        {
            time += Time.unscaledDeltaTime;
            float t = time / durationIn;

            mirror.position = Vector3.Lerp(startPos, targetPos, t);
            mirror.localScale = Vector3.Lerp(startScale, zoomScale, t);
            
            if(mirrorOverlay != null) mirrorOverlay.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        // Đảm bảo vị trí chính xác khi kết thúc
        mirror.position = targetPos;
        mirror.localScale = zoomScale;
        if(mirrorOverlay != null) mirrorOverlay.alpha = 1f;

        // 2. Dừng thời gian (Game Pause)
        Time.timeScale = 0f;

        // 3. Đợi một chút (Delay) trước khi hiện Doll
        // Dùng Realtime vì TimeScale đang bằng 0
        yield return new WaitForSecondsRealtime(animationStartDelay);

        // 4. Hiện Doll và chạy Animation
        dollController.ShowDoll(currentDollType);
    }

    public IEnumerator ZoomOut()
    {
        // 1. Ẩn Doll ngay lập tức khi bắt đầu thu nhỏ
        dollController.HideDoll();
        
        // 2. Trả lại thời gian bình thường
        Time.timeScale = 1f;

        Vector3 currentPos = mirror.position;
        Vector3 currentScale = mirror.localScale;

        float time = 0f;

        while (time < durationOut)
        {
            time += Time.unscaledDeltaTime;
            float t = time / durationOut;

            mirror.position = Vector3.Lerp(currentPos, startPos, t);
            mirror.localScale = Vector3.Lerp(currentScale, startScale, t);
            
            if(mirrorOverlay != null) mirrorOverlay.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        mirror.position = startPos;
        mirror.localScale = startScale;
        
        if(mirrorOverlay != null)
        {
            mirrorOverlay.alpha = 0f;
            mirrorOverlay.gameObject.SetActive(false);
        }

        isZoomed = false;
    }

    public void CloseMirror()
    {
        if (isZoomed)
            ToggleMirror();
    }
}