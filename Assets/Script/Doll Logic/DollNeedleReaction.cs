using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollNeedleReaction : MonoBehaviour
{
    private Animator animator;
    private DollData dollData;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        dollData = GetComponent<DollData>();
    }

    public void OnNeedlePoke()
    {
        if (dollData == null)
        {
            return;
        }
        StartCoroutine(NeedleActive());
    }  
    IEnumerator NeedleActive()
    {
        
        if (dollData.abnormalTrait == AbnormalTrait.JumpWhenNeedle)
        {
            if (animator != null)
            {
                animator.SetBool("IsScare", true);
            }
        }
        yield return new WaitForSeconds(1f);
        animator.SetBool("IsScare", false);
    }
}
