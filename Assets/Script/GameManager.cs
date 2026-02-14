using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Core")]
    [SerializeField] private DollManager dollManager;

    [Header("Intro UI")]
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject letterButton;
    [SerializeField] private GameObject buttonsRoot;

    // --- SỬA Ở ĐÂY: Đổi sang kiểu script mới là GameOverSimple ---
    [SerializeField] private GameOverSimple gameOverScript; 
    // -----------------------------------------------------------

    // Đổi tên biến audio thành audioManager để tránh Warning màu vàng
    public AudioManager audioManager; 

    private bool openedLetter;

    // Hàm này được DollManager gọi khi sai 4 lần
    public void ShowGameOverEvent()
    {
        // 1. Ẩn các nút bấm chơi game
        if (buttonsRoot != null) buttonsRoot.SetActive(false);

        // 2. Gọi script Fade In của màn hình thua
        if (gameOverScript != null)
        {
            gameOverScript.Show();
        }
    }

    private void Awake()
    {
        if(audioManager != null){
            audioManager.PlayMusicBG();
        }
        
        openedLetter = false;

        // Setup trạng thái đầu game
        if (blackScreen != null) blackScreen.SetActive(false);
        if (letterButton != null) letterButton.SetActive(true);
        if (buttonsRoot != null) buttonsRoot.SetActive(false);
        
        if (dollManager != null)
            dollManager.SetUIState(true, true);
    }

    public void OnLetterPressed()
    {
        if (openedLetter) return;
        openedLetter = true;
        
        if (blackScreen != null) blackScreen.SetActive(true);
        letterButton.SetActive(false);
    }

    public void StartGameplay()
    {
        if (blackScreen != null) blackScreen.SetActive(false);
        if (letterButton != null) letterButton.SetActive(false);
        if (buttonsRoot != null) buttonsRoot.SetActive(true);

        if (dollManager != null)
            dollManager.BeginGame();
    }
}