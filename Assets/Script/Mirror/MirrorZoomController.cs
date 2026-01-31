using System.Collections;
using UnityEngine;

public class MirrorZoomController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform mirror;      
    [SerializeField] private CanvasGroup mirrorOverlay; 
    [SerializeField] private MirrorDollController dollController; 

    // Bỏ dòng [SerializeField] DollType cũ đi
    // Thêm biến lưu trữ data của con búp bê đang soi
    private DollData currentTargetDoll; 

    [Header("Animation Settings")]
    [SerializeField] private float durationIn = 0.4f;  
    [SerializeField] private float durationOut = 0.4f; 
    [SerializeField] private Vector3 zoomScale = new Vector3(2.5f, 2.5f, 1f); 

    [Header("Timing")]
    [SerializeField] private float animationStartDelay = 0.5f; 

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
        
        if(dollController != null) dollController.HideDoll();
    }

    // --- HÀM MỚI QUAN TRỌNG ---
    // Gọi hàm này TRƯỚC khi bật gương để set búp bê mục tiêu
    public void SetTargetDoll(DollData dollData)
    {
        currentTargetDoll = dollData;
    }
    // ---------------------------

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
        // Kiểm tra an toàn: Nếu chưa set data thì không cho zoom hoặc báo lỗi
        if (currentTargetDoll == null)
        {
            Debug.LogWarning("Chưa gán Target Doll cho gương! Gọi SetTargetDoll trước.");
            // Vẫn cho zoom nhưng có thể không hiện doll, hoặc return tùy bạn
        }

        isZoomed = true;

        if(mirrorOverlay != null) mirrorOverlay.gameObject.SetActive(true);

        Vector3 targetPos = new Vector3(Screen.width / 2f, Screen.height / 2f, mirror.position.z);
        float time = 0f;

        while (time < durationIn)
        {
            time += Time.unscaledDeltaTime;
            float t = time / durationIn;

            mirror.position = Vector3.Lerp(startPos, targetPos, t);
            mirror.localScale = Vector3.Lerp(startScale, zoomScale, t);
            if(mirrorOverlay != null) mirrorOverlay.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        mirror.position = targetPos;
        mirror.localScale = zoomScale;
        if(mirrorOverlay != null) mirrorOverlay.alpha = 1f;

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(animationStartDelay);

        // TRUYỀN DATA VÀO CONTROLLER
        if (currentTargetDoll != null)
        {
            dollController.ShowDoll(currentTargetDoll);
        }
    }

    public IEnumerator ZoomOut()
    {
        dollController.HideDoll();
        
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