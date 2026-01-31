using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightObject : MonoBehaviour
{
    [SerializeField] private DollManager dollManager;
    private void Start()
    {
        dollManager = FindObjectOfType<DollManager>();
    }
    private void OnMouseDown()
    {
        if (dollManager == null) return;
        dollManager.UseFlashlight();
    }
}
