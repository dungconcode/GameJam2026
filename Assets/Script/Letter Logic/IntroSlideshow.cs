using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSlideshow : MonoBehaviour
{
    [Header("Cài đặt")]
    [SerializeField] private GameManager gameManager; // Kéo GameManager vào đây
    [SerializeField] private List<GameObject> danhSachCanh; 

    private int buocHienTai = 0;
    private bool duocPhepBam = true;

    // Tự động chạy ngay khi object được bật (Start Scene)
    private void Start()
    {
        Show();
    }

    public void Show()
    {
        // Ẩn hết, chỉ hiện cái đầu tiên
        foreach (var canh in danhSachCanh)
        {
            if (canh != null) canh.SetActive(false);
        }

        buocHienTai = 0;
        if (danhSachCanh.Count > 0 && danhSachCanh[0] != null)
        {
            danhSachCanh[0].SetActive(true);
        }
        
        // Khóa click 1 giây đầu
        duocPhepBam = false;
        Invoke("MoKhoaBam", 1.0f); 
    }

    void MoKhoaBam() => duocPhepBam = true;

    public void ChuyenCanhTiepTheo()
    {
        if (!duocPhepBam) return;

        // Tắt cảnh cũ
        if (buocHienTai < danhSachCanh.Count && danhSachCanh[buocHienTai] != null)
        {
            danhSachCanh[buocHienTai].SetActive(false);
        }

        buocHienTai++;

        // Kiểm tra xem còn ảnh tiếp theo không?
        if (buocHienTai < danhSachCanh.Count)
        {
            // CÒN ẢNH -> Hiện ảnh tiếp theo
            if(danhSachCanh[buocHienTai] != null)
            {
                danhSachCanh[buocHienTai].SetActive(true);
                duocPhepBam = false;
                Invoke("MoKhoaBam", 1.0f);
            }
        }
        else
        {
            // HẾT ẢNH -> VÀO GAME (Thay vì LoadScene)
            Debug.Log("Intro kết thúc -> Vào Game");
            
            // Tắt chính object Intro này đi
            gameObject.SetActive(false);
            
            // Gọi hàm bắt đầu game
            if (gameManager != null)
            {
                gameManager.StartGameplay();
            }
        }
    }
}