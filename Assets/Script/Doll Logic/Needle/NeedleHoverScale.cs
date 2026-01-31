using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleHoverScale : MonoBehaviour
{
    [SerializeField] private Transform needle;
    [SerializeField] private float hoverScale = 1.2f;
    [SerializeField] private float scaleSpeed = 10f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    private void Start()
    {
        if (needle != null)
        {
            originalScale = needle.localScale;
            targetScale = originalScale;
        }
    }
    private void OnMouseEnter()
    {
        targetScale = originalScale * hoverScale;
    }
    private void OnMouseExit()
    {
        targetScale = originalScale;
    }
    private void Update()
    {
        needle.localScale = Vector3.Lerp(
           needle.localScale,
           targetScale,
           scaleSpeed * Time.deltaTime
       );
    }
}
