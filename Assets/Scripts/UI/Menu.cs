using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class Menu : MonoBehaviour
{
    private CanvasGroup canvas;
    [SerializeField] private TextMeshProUGUI infoTxt;

    public void SetText(string message) => infoTxt.SetText(message);

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    public void Show(float fadeDuration = 0.5f)
    {
        gameObject.SetActive(true);
        canvas.DOFade(endValue: 1.0f, duration: fadeDuration).SetEase(Ease.OutSine);
        canvas.interactable = true;
        canvas.blocksRaycasts = true;
    }

    public void Hide(float fadeDuration = 0.5f)
    {
        canvas.DOFade(endValue: 0.0f, duration: fadeDuration)
            .SetEase(Ease.InSine)
            .OnComplete(() => gameObject.SetActive(false));
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }

    public static void StartGame() => OnStartClicked.Invoke();
    public static UnityEvent OnStartClicked = new UnityEvent();

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}

