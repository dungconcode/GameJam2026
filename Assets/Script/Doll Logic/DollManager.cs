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
    private void Awake()
    {
        pool = new List<GameObject>(dollPrefabs);
    }
    private void Start()
    {
        SpawnNext();
    }
    private void SpawnNext()
    {
        if (currentDoll != null)
            Destroy(currentDoll);

        if (countDolls == 0) { Debug.Log("No more dolls to spawn."); return; }

        if (pool.Count == 0) { Debug.LogError("Pool empty!"); return; }

        int tmp = countDolls - 1;
        countDolls_txt.text = "Dolls Left: " 
            + tmp + " / 5";

        int index = UnityEngine.Random.Range(0, pool.Count);
        GameObject prefab = pool[index];
        pool.RemoveAt(index);

        currentDoll = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        countDolls--;
        waitingForDecision = false;
        OnDecisionLockChanged?.Invoke(true);

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

    public void PokeNeedle()
    {
        if (currentDoll == null) return;

        var react = currentDoll.GetComponent<DollNeedleReaction>();
        if (react != null)
            react.OnNeedlePoke();
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

        SpawnNext();
        OnDecisionLockChanged?.Invoke(false); 
    }
    private void EvaluateDecision(bool playerPass)
    {
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
    }
    public void Pass()
    {
        if (!waitingForDecision) return;
        waitingForDecision = false;

        OnDecisionLockChanged?.Invoke(true); // khóa nút

        EvaluateDecision(true);
        StartCoroutine(WaitAndSpawnNext(0.5f, passPoint));
    }
    public void Cancel()
    {
        if (!waitingForDecision) return;
        waitingForDecision = false;

        OnDecisionLockChanged?.Invoke(true); // khóa nút

        EvaluateDecision(false);
        StartCoroutine(WaitAndSpawnNext(0.5f, cancelPoint));
    }
}
