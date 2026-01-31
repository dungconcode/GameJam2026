using System.Collections;
using UnityEngine;

public class MirrorZoomController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform mirror;      
    [SerializeField] private CanvasGroup mirrorOverlay; 
    [SerializeField] private MirrorDollController dollController; 

    private DollData currentTargetDoll; 

    [Header("Animation Settings")]
    [SerializeField] private float durationIn = 0.4f;  
    [SerializeField] private float durationOut = 0.4f; 
    
    // Bạn có thể chỉnh scale to hơn tại đây (ví dụ: 3, 3, 1)
    [SerializeField] private Vector3 zoomScale = new Vector3(2.5f, 2.5f, 1f); 
    
    // Đặt trục Z là 45 để khi phóng to xong gương sẽ ở góc này
    [SerializeField] private Vector3 targetRotation = new Vector3(0, 0, 45f);

    [Header("Timing")]
    [SerializeField] private float animationStartDelay = 0.5f; 

    private Vector3 startPos;
    private Vector3 startScale;
    private Quaternion startRotation; 
    private bool isZoomed;
    private Coroutine currentRoutine;

    void Start()
    {
        if (mirror == null) 
        {
            Debug.LogError("Chưa gán Reference cho MirrorZoomController!");
            return;
        }

        // Lưu thông số ban đầu (bao gồm Pos, Scale và Rotation 0,0,20 của bạn)
        startPos = mirror.position;
        startScale = mirror.localScale;
        startRotation = mirror.rotation; 

        if(mirrorOverlay != null)
        {
            mirrorOverlay.gameObject.SetActive(false);
            mirrorOverlay.alpha = 0f;
        }
        
        if(dollController != null) dollController.HideDoll();
    }

    public void SetTargetDoll(DollData dollData)
    {
        currentTargetDoll = dollData;
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
        if (currentTargetDoll == null)
        {
            Debug.LogWarning("Chưa gán Target Doll cho gương! Gọi SetTargetDoll trước.");
        }

        isZoomed = true;

        if(mirrorOverlay != null) mirrorOverlay.gameObject.SetActive(true);

        Vector3 targetPos = new Vector3(Screen.width / 2f, Screen.height / 2f, mirror.position.z);
        Quaternion targetRot = Quaternion.Euler(targetRotation);

        float time = 0f;

        while (time < durationIn)
        {
            time += Time.unscaledDeltaTime;
            float t = time / durationIn;

            // Di chuyển, phóng to và xoay cùng lúc
            mirror.position = Vector3.Lerp(startPos, targetPos, t);
            mirror.localScale = Vector3.Lerp(startScale, zoomScale, t);
            mirror.rotation = Quaternion.Lerp(startRotation, targetRot, t);

            if(mirrorOverlay != null) mirrorOverlay.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        mirror.position = targetPos;
        mirror.localScale = zoomScale;
        mirror.rotation = targetRot;

        if(mirrorOverlay != null) mirrorOverlay.alpha = 1f;

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(animationStartDelay);

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
        Quaternion currentRot = mirror.rotation; 

        float time = 0f;

        while (time < durationOut)
        {
            time += Time.unscaledDeltaTime;
            float t = time / durationOut;

            // Trả mọi thứ về trạng thái ban đầu (bao gồm góc 0,0,20)
            mirror.position = Vector3.Lerp(currentPos, startPos, t);
            mirror.localScale = Vector3.Lerp(currentScale, startScale, t);
            mirror.rotation = Quaternion.Lerp(currentRot, startRotation, t);

            if(mirrorOverlay != null) mirrorOverlay.alpha = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        mirror.position = startPos;
        mirror.localScale = startScale;
        mirror.rotation = startRotation; 
        
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