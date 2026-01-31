using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollMover : MonoBehaviour
{
    private float moveSpeed = 8f;
    private float arriverDistance = 0.1f;

    private Coroutine moveCo;


    public void MoveTo(Transform target, System.Action onArrived = null)
    {
        if (moveCo != null)
            StopCoroutine(moveCo);

        moveCo = StartCoroutine(MoveRoutine(target, onArrived));
    }
    private IEnumerator MoveRoutine(Transform target, System.Action onArrived)
    {
        while (target != null && Vector3.Distance(transform.position, target.position) > arriverDistance)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.identity;
            yield return null;
        }

        if (target != null) transform.position = target.position;

        onArrived?.Invoke();
    }


}
