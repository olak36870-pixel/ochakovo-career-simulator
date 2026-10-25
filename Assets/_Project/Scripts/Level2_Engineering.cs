using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Логика Уровня 2: «Инженерный Цех» (Механика «Быстрый ремонт» по GDD)
/// </summary>
public class Level2_Engineering : MonoBehaviour
{
    [Header("UI Элементы уровня")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject background2;
    [SerializeField] private Image timerProgressBar;
    [SerializeField] private List<Button> indicatorButtons; // Список всех кнопок-индикаторов
    [SerializeField] private Button furtherButton;          // Кнопка перехода к уровню 3

    [Header("Настройки мини-игры")]
    [SerializeField] private float gameDuration = 10f;      // Время на прохождение (в секундах)
    private float currentTime;
    private bool isGameActive = false;
    private int clickedIndicatorsCount = 0;

    [Header("Ссылки на маскота")]
    [SerializeField] private MascotController mascotController;
    string historicalFact = "В 1994 году инженеры «Очаково» первыми в мире запустили розлив напитков в большие пластиковые (ПЭТ) бутылки! Иностранцы не верили, что пластик сохранит вкус и качество. Однако наши инженеры модернизировали линии и доказали надежность технологии.";

    private void Start()
    {
        if (background2 != null)
        {
            background2.SetActive(true);
        }
        if (furtherButton != null)
        {
            furtherButton.onClick.AddListener(GoToNextLevel);
            furtherButton.gameObject.SetActive(false);
        }

        // Привязываем клики к каждой кнопке-индикатору
        for (int i = 0; i < indicatorButtons.Count; i++)
        {
            int index = i; // Замыкание для лямбда-выражения
            indicatorButtons[i].onClick.AddListener(() => OnIndicatorClicked(index));
        }
    }

    private void OnEnable()
    {
        StartMiniGame();
    }

    public void StartMiniGame()
    {
        isGameActive = true;
        currentTime = gameDuration;
        clickedIndicatorsCount = 0;
        furtherButton.gameObject.SetActive(false);

        if (timerProgressBar != null)
        {
            timerProgressBar.fillAmount = 0f;
        }

        string introText = "Инженерный цех на связи! Панель сбоит, датчики горят красным. Тапай по ним быстрее, чем обновляется лента в тиктоке, пока всё не задымилось!";
        if (mascotController != null)
        {
            mascotController.DisplayDialog(introText, MascotEmotion.Normal);
        }

        ResetAndColorAllIndicators();
    }

    private void Update()
    {
        if (!isGameActive) return;

        currentTime -= Time.deltaTime;

        float timePassedNormalized = 1f - (currentTime / gameDuration);

        if (timerProgressBar != null)
        {
            timerProgressBar.fillAmount = Mathf.Clamp01(timePassedNormalized);
        }

        if (timerText != null)
        {
            timerText.text = "Время: " + Mathf.Ceil(currentTime).ToString() + "с";
        }

        if (currentTime <= 0)
        {
            if (timerProgressBar != null) timerProgressBar.fillAmount = 1f;
            EndMiniGame(false);
        }
    }

    /// <summary>
    /// ИСПРАВЛЕНИЕ: Переводим тип перехода кнопок на None и красим Image напрямую, чтобы избежать зачернения
    /// </summary>
    private void ResetAndColorAllIndicators()
    {
        for (int i = 0; i < indicatorButtons.Count; i++)
        {
            indicatorButtons[i].interactable = true;

            // Отключаем встроенные цветовые тинты Unity Button, чтобы они не мешали
            indicatorButtons[i].transition = Selectable.Transition.None;

            // Красим саму картинку кнопки напрямую в красный
            Image buttonImage = indicatorButtons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = Color.red;
            }
        }
    }

    private void OnIndicatorClicked(int clickedIndex)
    {
        if (!isGameActive) return;

        Button clickedButton = indicatorButtons[clickedIndex];
        Image buttonImage = clickedButton.GetComponent<Image>();

        // Проверяем, что кнопка еще красная (не была нажата)
        if (buttonImage != null && buttonImage.color == Color.red)
        {
            // ЖЕСТКО КРАСИМ В ЗЕЛЕНЫЙ НАПРЯМУЮ
            buttonImage.color = Color.green;

            clickedButton.interactable = false; // Отключаем кликабельность

            clickedIndicatorsCount++;

            if (clickedIndicatorsCount >= indicatorButtons.Count)
            {
                EndMiniGame(true);
            }
        }
    }

    private void EndMiniGame(bool isSuccess)
    {
        isGameActive = false;

        for (int i = 0; i < indicatorButtons.Count; i++)
        {
            indicatorButtons[i].interactable = false;
        }

        if (isSuccess)
        {
            if (GameManager.Instance != null) GameManager.Instance.AddXP(100);
            if (mascotController != null)
                mascotController.DisplayDialog("Классно сработано! Кстати, ты только что защитил от сбоя легендарную линию, на которой наши парни устроили мировую технологическую революцию!", MascotEmotion.Joyful, historicalFact);
        }
        else
        {
            if (GameManager.Instance != null) GameManager.Instance.AddXP(20);
            if (mascotController != null)
                mascotController.DisplayDialog("Ой, пошёл дымок! Датчики победили, но спокойно: наши инженеры и не такое оживляли изолентой! Ты круто старался. Между прочим, именно на этой линии наши рабочие устроили мировую революцию в упаковке, утерев нос иностранцам.", MascotEmotion.Sad, historicalFact);
        }

        furtherButton.gameObject.SetActive(true);
    }

    private void GoToNextLevel()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameState.Level3_Logistics);
            Debug.Log("[Level2] Переход к Уровню 3 (Склад и логистика)");
        }
    }
}
