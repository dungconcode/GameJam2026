using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class ButtonUIManager : MonoBehaviour
{
    [SerializeField] private GameObject panel_Menu;
    [SerializeField] private GameObject setting_Panel;
    [SerializeField] private Transform start_Pos;
    [SerializeField] private Transform end_Pos;

    private Coroutine slideCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        if(start_Pos != null)
        {
            panel_Menu.transform.position = start_Pos.position;
            setting_Panel.transform.position = start_Pos.position;
        }
        panel_Menu.SetActive(false);
        setting_Panel.SetActive(false);
    }
    #region Menu Panel
    public void OpenMenu()
    {
        panel_Menu.SetActive(true);
        if(slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
        }
        slideCoroutine = StartCoroutine(Slide(panel_Menu.transform, start_Pos.position, end_Pos.position));
    }    
    public void CloseMenu()
    {
        if (slideCoroutine != null)
            StopCoroutine(slideCoroutine);

        slideCoroutine = StartCoroutine(CloseSlide(panel_Menu.transform));
    }
    #endregion

    #region Setting Panel
    public void OpenSetting()
    {
        setting_Panel.SetActive(true);
        if (slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
        }
        slideCoroutine = StartCoroutine(Slide(setting_Panel.transform, start_Pos.position, end_Pos.position));
    }
    public void CloseSetting()
    {
        if (slideCoroutine != null)
            StopCoroutine(slideCoroutine);

        slideCoroutine = StartCoroutine(CloseSlide(setting_Panel.transform));
    }
    #endregion
    IEnumerator CloseSlide(Transform panel)
    {
        yield return Slide(panel, end_Pos.position, start_Pos.position);
        panel.gameObject.SetActive(false);
    }
    IEnumerator Slide(Transform target, Vector3 from, Vector3 to)
    {
        float elapsedTime = 0f;
        float duration = 0.1f;
        while (elapsedTime < duration)
        {
            target.position = Vector3.Lerp(from, to, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        target.position = to;
    }
    
}
