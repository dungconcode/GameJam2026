using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DecisionUI : MonoBehaviour
{
    [SerializeField] private DollManager gameManager;
    [Header("Buttons pass")]
    [SerializeField] private Button passButton;
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteDown;

    [Header("Buttons cancel")]
    //[SerializeField] private Sprite cancelUp;
    //[SerializeField] private Sprite cancelPress;
    [SerializeField] private Button cancelButton;

    private void OnEnable()
    {
        gameManager.OnDecisionLockChanged += SetInteractable;
    }

    private void OnDisable()
    {
        gameManager.OnDecisionLockChanged -= SetInteractable;
    }

    private void Start()
    {
        SetInteractable(false);
    }

    private void SetInteractable(bool locked)
    {
        bool canInteract = !locked;
        passButton.interactable = canInteract;
        cancelButton.interactable = canInteract;
    }

    public void OnPassClicked()
    {
        gameManager.Pass();
        StartCoroutine(WaitForSpriteUp());
    }
    IEnumerator WaitForSpriteUp()
    {
        passButton.image.sprite = spriteDown;
        yield return new WaitForSeconds(2.2f);
        passButton.image.sprite = spriteUp;
    }
    public void OnCancelClicked()
    {
        gameManager.Cancel();
    }
}
