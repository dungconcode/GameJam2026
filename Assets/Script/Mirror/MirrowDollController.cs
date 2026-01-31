using UnityEngine;

public class MirrorDollController : MonoBehaviour
{
    [Header("Doll References")]
    [SerializeField] private GameObject dollObject; // Kéo object Doll vào đây
    [SerializeField] private Animator dollAnimator; // Kéo object Doll (nơi có Animator) vào đây

    public void ShowDoll(DollType type)
    {
        dollObject.SetActive(true);

        // Quan trọng: Tên trong chuỗi string ("Run", "Idle") phải khớp với tên State trong cửa sổ Animator
        switch (type)
        {
            case DollType.Normal:
                // Nếu bạn muốn Doll thường chạy animation Run
                dollAnimator.Play("RunAnim"); 
                break;

            case DollType.Abnormal:
                dollAnimator.Play("AbnormalReveal");
                break;

            case DollType.Vampire:
                dollAnimator.Play("VampireReveal");
                break;
        }
    }

    public void HideDoll()
    {
        dollObject.SetActive(false);
    }
}