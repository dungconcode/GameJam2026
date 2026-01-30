using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private Image loadingBar;
    [SerializeField] private Text loadingText;
    private bool isLoading = false;
    private Coroutine loadingCoroutine;
    private AudioManager audioManager;
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        startScreen.SetActive(false);
        loadingScreen.SetActive(true);
        loadingBar.fillAmount = 0f;
        loadingText.text = "Loading... 0%";
        if(loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
        }
        loadingCoroutine = StartCoroutine(LoadingBar(1f));
    }
    public void LoadLevelBtn(string levelToLoad)
    {
        ShowLoadingUI();
        if (loadingCoroutine != null)
            StopCoroutine(loadingCoroutine);

        loadingCoroutine = StartCoroutine(LoadSceneAsync(levelToLoad));
    }
    IEnumerator LoadingBar(float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / time);
            loadingBar.fillAmount = progress;
            
            UpdateUI(progress);
            yield return null;
            isLoading = false;
        }
        UpdateUI(1f);
        if (!isLoading)
        {
            startScreen.SetActive(true);
            loadingScreen.SetActive(false);
            audioManager.PlayMusicBG();
        }
    }
    private void ShowLoadingUI()
    {
        if (startScreen != null) startScreen.SetActive(false);
        if (loadingScreen != null) loadingScreen.SetActive(true);

        UpdateUI(0f);
    }
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float displayProgress = 0f;

        while (displayProgress < 1f)
        {
            float targetProgress = Mathf.Clamp01(op.progress / 0.9f);

            displayProgress = Mathf.MoveTowards(
                displayProgress,
                targetProgress,
                Time.deltaTime * 0.8f
            );

            UpdateUI(displayProgress);
            if (displayProgress >= 1f && op.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.2f);
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    private void UpdateUI(float progress01)
    {
        if (loadingBar != null) loadingBar.fillAmount = progress01;
        if (loadingText != null)
            loadingText.text = $"Loading... {Mathf.RoundToInt(progress01 * 100)}%";
    }
}
