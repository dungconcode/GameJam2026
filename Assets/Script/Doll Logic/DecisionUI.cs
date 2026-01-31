using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DecisionUI : MonoBehaviour
{
    [SerializeField] private DollManager dollManager;
    [Header("Buttons pass")]
    [SerializeField] private Button passButton;
    //[SerializeField] private Sprite spriteUp;
    //[SerializeField] private Sprite spriteDown;

    [Header("Buttons cancel")]
    //[SerializeField] private Sprite cancelUp;
    //[SerializeField] private Sprite cancelPress;
    [SerializeField] private Button cancelButton;

    [Header("Buttons next")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Sprite nextUp;
    [SerializeField] private Sprite nextPress;

    private void OnEnable()
    {
        dollManager.OnDecisionLockChanged += SetInteractable;
        dollManager.OnNextLockChanged += SetNextInteractable;
    }

    private void OnDisable()
    {
        dollManager.OnDecisionLockChanged -= SetInteractable;
        dollManager.OnNextLockChanged -= SetNextInteractable;
    }

    private void Start()
    {
        SetInteractable(false);
        SetNextInteractable(true);
    }

    private void SetInteractable(bool locked)
    {
        bool canInteract = !locked;
        passButton.interactable = canInteract;
        cancelButton.interactable = canInteract;
    }

    public void OnPassClicked()
    {
        dollManager.Pass();
    }
    IEnumerator WaitForSpriteUp(Button btn, Sprite down, Sprite up)
    {
        btn.image.sprite = down;
        yield return new WaitForSeconds(2.2f);
        btn.image.sprite = up;
    }
    public void OnCancelClicked()
    {
        dollManager.Cancel();
    }
    public void OnNextClicked()
    {
        dollManager.Next();
        //WaitForSpriteUp(nextButton, nextPress, nextUp);
    }
    private void SetDecisionInteractable(bool locked)
    {
        bool canInteract = !locked;
        passButton.interactable = canInteract;
        cancelButton.interactable = canInteract;

    }


    private void SetNextInteractable(bool locked)
    {
        if (nextButton == null) return;

        bool canInteract = !locked;
        nextButton.interactable = canInteract;
        if (nextButton.image != null)
        {
            nextButton.image.sprite = canInteract ? nextUp : nextPress;
        }

    }

}
