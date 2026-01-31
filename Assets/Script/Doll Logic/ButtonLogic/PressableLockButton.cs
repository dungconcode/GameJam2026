using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PressableSpriteButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Visual")]
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteDown;

    [Header("Scale")]
    [SerializeField] private RectTransform visual;
    [SerializeField] private float hoverScale = 1.05f;
    [SerializeField] private float normalScale = 1f;
    [SerializeField] private float speed = 18f;

    [Header("Action")]
    [SerializeField] private DecisionUI decisionUI;
    [SerializeField] private bool isPassButton = true;

    private bool locked;          // interactable = false
    private bool pressedVisual;   // đang hiển thị thụt
    private Vector3 targetScale;

    private void Awake()
    {
        if (targetImage == null) targetImage = GetComponent<Image>();
        if (visual == null) visual = transform as RectTransform;

        // Nếu bạn quên gán spriteUp, lấy sprite hiện tại làm spriteUp
        if (spriteUp == null && targetImage != null)
            spriteUp = targetImage.sprite;

        ApplyVisual(isPressed: false);
        targetScale = Vector3.one * normalScale;
    }

    private void Update()
    {
        if (visual == null) return;
        visual.localScale = Vector3.Lerp(visual.localScale, targetScale, speed * Time.deltaTime);
    }

    // Gọi từ DecisionUI khi cần lock/unlock
    public void SetLocked(bool value)
    {
        locked = value;

        // Khi unlock => nút phải "bật lên" (không press) theo đúng yêu cầu bạn
        if (!locked)
        {
            pressedVisual = false;
            ApplyVisual(isPressed: false);
        }
        else
        {
            // Khi lock thì giữ nguyên pressed/unpressed tùy bạn,
            // nhưng mình để nếu đang pressed thì giữ pressed.
            ApplyVisual(isPressed: pressedVisual);
        }
    }

    private void ApplyVisual(bool isPressed)
    {
        pressedVisual = isPressed;

        if (targetImage != null)
        {
            if (isPressed && spriteDown != null) targetImage.sprite = spriteDown;
            else if (spriteUp != null) targetImage.sprite = spriteUp;
        }

        // pressed thì không hover
        targetScale = Vector3.one * normalScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (locked) return;
        if (pressedVisual) return;
        targetScale = Vector3.one * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (locked) return;
        if (pressedVisual) return;
        targetScale = Vector3.one * normalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (locked) return;

        // Press xuống + khóa tương tác ngay
        ApplyVisual(isPressed: true);
        locked = true;

        if (decisionUI == null) return;
        if (isPassButton) decisionUI.OnPassClicked();
        else decisionUI.OnCancelClicked();
    }
}
