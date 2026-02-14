using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSimple : MonoBehaviour
{
    [Header("Danh sách các cảnh (Kéo theo thứ tự)")]
    // Element 0: GameOverGroup (Nền đen + Thư)
    // Element 1: Bangthuviec
    // Element 2: Bangthuviecroom (Ảnh cuối)
    [SerializeField] private List<GameObject> danhSachCanh; 

    private int buocHienTai = 0;
    private bool duocPhepBam = true;

    // Hàm này GameManager gọi lúc thua
    public void Show()
    {
        // 1. Tắt hết các cảnh phía sau để reset
        foreach (var canh in danhSachCanh)
        {
            if (canh != null) canh.SetActive(false);
        }

        // 2. Bật cảnh đầu tiên (GameOverGroup)
        buocHienTai = 0;
        if (danhSachCanh.Count > 0 && danhSachCanh[0] != null)
        {
            danhSachCanh[0].SetActive(true);
        }
        
        // Đợi 1 chút mới cho bấm để tránh click nhầm
        duocPhepBam = false;
        Invoke("MoKhoaBam", 1.0f); 
    }

    void MoKhoaBam() => duocPhepBam = true;

    // GÁN HÀM NÀY VÀO NÚT BẤM CỦA CẢ 3 CẢNH
    public void ChuyenCanhTiepTheo()
    {
        if (!duocPhepBam) return;

        // 1. Tắt cảnh hiện tại
        if (buocHienTai < danhSachCanh.Count)
        {
            if(danhSachCanh[buocHienTai] != null)
                danhSachCanh[buocHienTai].SetActive(false);
        }

        // 2. Tăng bước lên
        buocHienTai++;

        // 3. Bật cảnh tiếp theo (nếu còn)
        if (buocHienTai < danhSachCanh.Count)
        {
            if(danhSachCanh[buocHienTai] != null)
            {
                danhSachCanh[buocHienTai].SetActive(true);
                
                // Khóa click tạm thời để hiệu ứng Fade kịp chạy
                duocPhepBam = false;
                Invoke("MoKhoaBam", 1.0f);
            }
        }
        else
        {
            Debug.Log("HẾT GAME!");
            // Xử lý logic hết game ở đây (Ví dụ: LoadScene Menu)
        }
    }
}