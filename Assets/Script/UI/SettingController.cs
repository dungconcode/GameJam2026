using UnityEngine;
using UnityEngine.SceneManagement; // QUAN TRỌNG: Phải có dòng này mới load scene được

public class SettingController : MonoBehaviour
{
    [Header("Cấu hình UI")]
    [SerializeField] private GameObject settingPanel;

    [Header("Cấu hình Scene")]
    [Tooltip("Nhập chính xác tên Scene mà bạn muốn quay lại (VD: Menu, Home...)")]
    [SerializeField] private string sceneToReturn = "Menu"; 

    // --- CÁC HÀM CŨ (ĐÓNG/MỞ UI) ---
    public void OpenSetting()
    {
        if (settingPanel != null) settingPanel.SetActive(true);
    }

    public void CloseSetting()
    {
        if (settingPanel != null) settingPanel.SetActive(false);
    }

    public void ToggleSetting()
    {
        if (settingPanel != null)
        {
            if (settingPanel.activeSelf) CloseSetting();
            else OpenSetting();
        }
    }

    // --- CÁC HÀM MỚI (SCENE) ---

    /// <summary>
    /// Logic Restart: Load lại chính Scene hiện tại
    /// </summary>
    public void RestartScene()
    {
        // Lấy tên của Scene đang chạy và load lại nó
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    /// <summary>
    /// Logic Exit: Quay về Scene trước đó (thường là Menu)
    /// </summary>
    public void ExitScene()
    {
        // Load Scene theo tên bạn đã nhập ở ô 'Scene To Return'
        SceneManager.LoadScene(sceneToReturn);
    }
}