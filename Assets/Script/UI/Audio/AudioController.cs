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
        UpdateUI();
        
        if(musicOnButton != null) musicOnButton.onClick.AddListener(ToggleAudio);
        if(musicOffButton != null) musicOffButton.onClick.AddListener(ToggleAudio);
    }

    public void ToggleAudio()
    {
        isMuted = !isMuted;

        AudioListener.pause = isMuted;

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (musicOnButton != null && musicOffButton != null)
        {
            musicOnButton.gameObject.SetActive(!isMuted);
            musicOffButton.gameObject.SetActive(isMuted);
        }
    }
}