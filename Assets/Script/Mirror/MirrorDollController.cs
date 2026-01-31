using UnityEngine;

public class MirrorDollController : MonoBehaviour
{
    [Header("Doll References")]
    [SerializeField] private GameObject dollObject; 
    [SerializeField] private Animator dollAnimator; 

    // Hàm này giờ sẽ nhận vào toàn bộ data của con búp bê
    public void ShowDoll(DollData data)
    {
        if (data == null) return;

        dollObject.SetActive(true);

        // 1. Nếu là búp bê thường
        if (data.dollType == DollType.Normal)
        {
            dollAnimator.Play("NormalIdle"); // Đổi tên này theo tên state trong Animator của bạn
            return;
        }

        // 2. Nếu là búp bê lỗi (Abnormal), check xem lỗi kiểu gì
        switch (data.abnormalTrait)
        {
            case AbnormalTrait.HeadMove:
                dollAnimator.Play("HeadMove"); // Đảm bảo trong Animator có state tên này
                break;

            case AbnormalTrait.WaveHand:
                dollAnimator.Play("WaveHand");
                break;

            case AbnormalTrait.BodyCrack:
                dollAnimator.Play("BodyCrack");
                break;

            case AbnormalTrait.BloodyBody:
                dollAnimator.Play("BloodyBody");
                break;

            case AbnormalTrait.JumpWhenNeedle:
                // Trường hợp này có thể chỉ hiện khi châm kim, 
                // hoặc hiện animation run rẩy sợ hãi
                dollAnimator.Play("Trembling"); 
                break;

            default:
                // Fallback nếu có lỗi lạ
                dollAnimator.Play("NormalIdle");
                break;
        }
    }

    public void HideDoll()
    {
        dollObject.SetActive(false);
    }
}