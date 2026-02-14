using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DollManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> dollPrefabs;
    private List<GameObject> pool = new();

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform targetPoint;
    [SerializeField] private Transform cancelPoint;
    [SerializeField] private Transform passPoint;

    private GameObject currentDoll;
    public int countDolls = 5;
    private bool waitingForDecision;
    [SerializeField] private Text countDolls_txt;

    [Header("Checking Dolls")]
    private int correctCount = 0;
    private int wrongCount = 0;

    public event Action<bool> OnDecisionLockChanged;
    public event Action<bool> OnNextLockChanged;
    private bool waitingForNext;

    private void Awake()
    {
        pool = new List<GameObject>(dollPrefabs);
    }

    public void BeginGame()
    {
        SpawnNext();
    }

    private void SpawnNext()
    {
        if (currentDoll != null)
            Destroy(currentDoll);

        // Quay lại logic cũ: Khi hết búp bê thì chỉ log ra console, không kích hoạt sự kiện kết thúc
        if (countDolls == 0) 
        { 
            Debug.Log("No more dolls to spawn."); 
            return; 
        }

        if (pool.Count == 0) { Debug.LogError("Pool empty!"); return; }

        int tmp = countDolls - 1;
        countDolls_txt.text = "Dolls: " + tmp + " / 5";

        int index = UnityEngine.Random.Range(0, pool.Count);
        GameObject prefab = pool[index];
        pool.RemoveAt(index);

        currentDoll = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        countDolls--;
        
        waitingForDecision = false;
        waitingForNext = false;
        OnDecisionLockChanged?.Invoke(true);
        OnNextLockChanged?.Invoke(true);

        var mover = currentDoll.GetComponent<DollMover>();
        if (mover != null && targetPoint != null)
        {
            mover.MoveTo(targetPoint, () =>
            {
                waitingForDecision = true;
                OnDecisionLockChanged?.Invoke(false);
            });
        }
        else
        {
            waitingForDecision = true;
            OnDecisionLockChanged?.Invoke(false);
        }
    }

    IEnumerator WaitAndSpawnNext(float delay, Transform pos)
    {
        yield return new WaitForSeconds(delay);

        if (currentDoll == null) yield break;

        var mover = currentDoll.GetComponent<DollMover>();
        if (mover != null && pos != null)
            mover.MoveTo(pos);

        while (currentDoll != null &&
               pos != null &&
               Vector2.Distance(currentDoll.transform.position, pos.position) > 0.05f)
        {
            yield return null;
        }

        waitingForDecision = false;           
        OnDecisionLockChanged?.Invoke(true);  

        // Chỉ cho phép hiện nút Next nếu người chơi chưa làm sai quá 4 lần
        if (wrongCount < 4)
        {
            waitingForNext = true;               
            OnNextLockChanged?.Invoke(false);    
        }
    }

    private void EvaluateDecision(bool playerPass)
    {
        if (currentDoll == null) return;
        
        DollData data = currentDoll.GetComponent<DollData>();
        if (data == null)
        {
            Debug.LogError("Doll has no DollData!");
            return;
        }

        bool isCorrect;
        if (data.IsAbnormal())
            isCorrect = !playerPass;
        else
            isCorrect = playerPass; 

        if (isCorrect)
        {
            correctCount++;
        }
        else
        {
            wrongCount++;
        }

        Debug.Log($"Correct: {correctCount} | Wrong: {wrongCount}");

        // CHỈ THÊM LOGIC NÀY: Khi sai đúng 4 lần mới kích hoạt sự kiện "Thua"
        if (wrongCount >= 4)
        {
            Debug.Log("Sự kiện thua: Bạn đã làm sai 4 lần!");
            StopAllCoroutines(); // Dừng búp bê đang chạy
            // Gọi hàm xử lý hiện bức thư trượt ra ở GameManager
            FindObjectOfType<GameManager>().SendMessage("ShowGameOverEvent", SendMessageOptions.DontRequireReceiver);
        }
    }

    #region Button Actions
    public void Pass()
    {
        if (!waitingForDecision) return;
        waitingForDecision = false;
        OnDecisionLockChanged?.Invoke(true);

        EvaluateDecision(true);
        
        // Chỉ tiếp tục luồng búp bê nếu chưa bị thua
        if(wrongCount < 4)
            StartCoroutine(WaitAndSpawnNext(0.5f, passPoint));
    }

    public void Cancel()
    {
        if (!waitingForDecision) return;
        waitingForDecision = false;
        OnDecisionLockChanged?.Invoke(true); 

        EvaluateDecision(false);
        
        if(wrongCount < 4)
            StartCoroutine(WaitAndSpawnNext(0.5f, cancelPoint));
    }

    public void Next()
    {
        if (!waitingForNext) return;
        waitingForNext = false;
        OnNextLockChanged?.Invoke(true);
        SpawnNext();
    }
    #endregion

    #region Interaction
    public void PokeNeedle()
    {
        if (currentDoll == null) return;
        var react = currentDoll.GetComponent<DollNeedleReaction>();
        if (react != null) react.OnNeedlePoke();
    }

    public void UseFlashlight()
    {
        if (currentDoll == null) return;
        DollData data = currentDoll.GetComponent<DollData>();
        if (data == null || !waitingForDecision) return;

        if (data.abnormalTrait == AbnormalTrait.BloodyBody)
        {
            var reveal = currentDoll.GetComponent<DollBloodReveal>();
            if (reveal != null) reveal.RevealOnce();
        }
    }
    #endregion

    public void SetUIState(bool lockDecision, bool lockNext)
    {
        OnDecisionLockChanged?.Invoke(lockDecision);
        OnNextLockChanged?.Invoke(lockNext);
    }

    public DollData GetCurrentDollData()
    {
        if (currentDoll == null) return null;
        return currentDoll.GetComponent<DollData>();
    }
}