using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleObject : MonoBehaviour
{
    [SerializeField] private DollManager dollManager;
    [SerializeField] private GameObject needleAnim;
    [SerializeField] private GameObject needleItem;
    [SerializeField] private Animator needleAnimator;
    [SerializeField] private string pokeTrigger = "Poke";
    [SerializeField] private float hitDelay = 0.12f;

    private bool isPlaying;
    private void Start()
    {
        needleAnim.SetActive(false);
        needleItem.SetActive(true);
    }
    private void OnMouseDown()
    {
        Debug.Log("Needle Poked");
        if (isPlaying) return;
        isPlaying = true;
        StartCoroutine(NeedleRunning());
    }

    IEnumerator NeedleRunning()
    {
        needleAnim.SetActive(true);
        needleItem.SetActive(false);
        if (needleAnimator != null)
            needleAnimator.SetTrigger(pokeTrigger);
        yield return new WaitForSeconds(0.1f);
        Invoke(nameof(FireNeedle), hitDelay);
        Invoke(nameof(Unlock), 1f);
        yield return new WaitForSeconds(1f);
        needleAnim.SetActive(false);
        needleItem.SetActive(true);
    }
    private void FireNeedle()
    {
        dollManager.PokeNeedle();
    }

    private void Unlock()
    {
        isPlaying = false;
    }
}
