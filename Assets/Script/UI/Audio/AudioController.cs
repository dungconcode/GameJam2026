using UnityEngine;
using UnityEngine.UI;

public partial class AudioController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button musicOnButton;  // Nút hiển thị khi đang bật nhạc
    [SerializeField] private Button musicOffButton; // Nút hiển thị khi đang tắt nhạc

    [Header("Settings")]
    [SerializeField] private bool isMuted = false;

    void Start()
    {
        // Kiểm tra trạng thái ban đầu để hiển thị đúng button
        UpdateUI();
        
        // Gán sự kiện click cho các nút (nếu bạn chưa gán trong Inspector)
        if(musicOnButton != null) musicOnButton.onClick.AddListener(ToggleAudio);
        if(musicOffButton != null) musicOffButton.onClick.AddListener(ToggleAudio);
    }

    public void ToggleAudio()
    {
        // Đảo ngược trạng thái mute
        isMuted = !isMuted;

        // Tắt/Bật âm thanh tổng của hệ thống
        AudioListener.pause = isMuted;

        // Cập nhật giao diện ẩn/hiện nút
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (musicOnButton != null && musicOffButton != null)
        {
            // Nếu đang Mute thì hiện nút Off, ẩn nút On và ngược lại
            musicOnButton.gameObject.SetActive(!isMuted);
            musicOffButton.gameObject.SetActive(isMuted);
        }
    }
}