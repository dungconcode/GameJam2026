using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlackScreenClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameManager gameManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        gameManager.SendMessage("StartGameplay", SendMessageOptions.DontRequireReceiver);
    }
}
