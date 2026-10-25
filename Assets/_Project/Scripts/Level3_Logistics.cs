using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Логика Уровня 3: «Склад и Логистика» (Механика свайп-карточек по GDD)
/// </summary>
public class Level3_Logistics : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("UI Элементы уровня")]
    [SerializeField] private RectTransform cardRect;         // RectTransform карточки (SwipeCard)
    [SerializeField] private Image cardImage;               // Картинка товара на карточке
    [SerializeField] private TextMeshProUGUI cardTitleText; // Текст названия товара
    [SerializeField] private Button furtherButton;          // Кнопка перехода к Финалу
    [SerializeField] private GameObject background3;

    [Header("Визуальные индикаторы свайпа")]
    [SerializeField] private CanvasGroup approveOverlay;     // Индикатор "В фуру" (справа)
    [SerializeField] private CanvasGroup rejectOverlay;      // Индикатор "В утиль" (слева)

    [Header("Настройки свайпа")]
    [SerializeField] private float swipeThreshold = 150f;   // Дистанция свайпа для срабатывания
    [SerializeField] private float cardRotationAmount = 15f;// Угол наклона карточки при свайпе

    [Header("Список карточек")]
    [SerializeField] private List<LogisticsCard> cardsList = new List<LogisticsCard>();

    [Header("Ссылки на Маскота")]
    [SerializeField] private MascotController mascotController;
    private string historicalFact = "С марта по сентябрь комбинат переходит в режим максимальной интенсивности. Объём отгрузки продукции исчисляется миллионами литров. Управление такими потоками полностью автоматизировано через ИТ-системы и АСУ ТП.\r\n";

    private Vector2 initialCardPosition;
    private int currentCardIndex = 0;
    private int correctSwipesCount = 0;
    private bool isGameActive = false;

    private void Start()
    {
        if (background3 != null)
        {
            background3.SetActive(true);
        }

        if (cardRect != null) initialCardPosition = cardRect.anchoredPosition;

        if (furtherButton != null)
        {
            furtherButton.onClick.AddListener(GoToFinalScreen);
            furtherButton.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (cardRect != null)
        {
            initialCardPosition = cardRect.anchoredPosition;
            cardRect.gameObject.SetActive(true);
        }

        StartMiniGame();
    }

    public void StartMiniGame()
    {
        isGameActive = true;
        currentCardIndex = 0;
        correctSwipesCount = 0;
        furtherButton.gameObject.SetActive(false);

        // Включаем объекты обратно при старте новой игры
        if (cardRect != null) cardRect.gameObject.SetActive(true);
        if (approveOverlay != null) approveOverlay.gameObject.SetActive(true);
        if (rejectOverlay != null) rejectOverlay.gameObject.SetActive(true);

        string introText = "Добро пожаловать в ИТ-логистику! Заказы летят быстрее, чем сторис в соцсетях. Перед тобой карточки: смахивай продукцию в фуру (вправо), а всякий мусор — в утиль (влево).";
        if (mascotController != null)
        {
            mascotController.DisplayDialog(introText, MascotEmotion.Normal);
        }

        DisplayCurrentCard();
    }

    private void DisplayCurrentCard()
    {
        if (currentCardIndex >= cardsList.Count)
        {
            EndMiniGame();
            return;
        }

        if (cardRect != null)
        {
            cardRect.anchoredPosition = initialCardPosition;
            cardRect.localRotation = Quaternion.identity;
        }

        ResetOverlays();

        LogisticsCard currentCard = cardsList[currentCardIndex];
        if (cardImage != null) cardImage.sprite = currentCard.cardSprite;
        if (cardTitleText != null) cardTitleText.text = currentCard.cardName;
    }

    // --- Обработка Drag & Drop (Свайпа) ---

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isGameActive || currentCardIndex >= cardsList.Count || cardRect == null) return;

        cardRect.anchoredPosition += new Vector2(eventData.delta.x, 0);

        float offset = cardRect.anchoredPosition.x - initialCardPosition.x;
        float normalizedOffset = Mathf.Clamp(offset / swipeThreshold, -1f, 1f);
        cardRect.localRotation = Quaternion.Euler(0, 0, -normalizedOffset * cardRotationAmount);

        if (offset > 0)
        {
            if (approveOverlay != null) approveOverlay.alpha = normalizedOffset;
            if (rejectOverlay != null) rejectOverlay.alpha = 0;
        }
        else
        {
            if (rejectOverlay != null) rejectOverlay.alpha = -normalizedOffset;
            if (approveOverlay != null) approveOverlay.alpha = 0;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isGameActive || cardRect == null) return;

        float offset = cardRect.anchoredPosition.x - initialCardPosition.x;

        if (Mathf.Abs(offset) >= swipeThreshold)
        {
            bool swipedRight = offset > 0;
            ProcessSwipe(swipedRight);
        }
        else
        {
            cardRect.anchoredPosition = initialCardPosition;
            cardRect.localRotation = Quaternion.identity;
            ResetOverlays();
        }
    }

    private void ProcessSwipe(bool swipedRight)
    {
        LogisticsCard currentCard = cardsList[currentCardIndex];

        if (swipedRight == currentCard.isCorrectProduct)
        {
            correctSwipesCount++;
        }

        currentCardIndex++;
        DisplayCurrentCard();
    }

    private void ResetOverlays()
    {
        if (approveOverlay != null) approveOverlay.alpha = 0;
        if (rejectOverlay != null) rejectOverlay.alpha = 0;
    }

    private void EndMiniGame()
    {
        isGameActive = false;

        // 1. Полностью скрываем и деактивируем саму карточку
        if (cardRect != null)
        {
            cardRect.gameObject.SetActive(false);
        }

        // 2. Жёстко обнуляем прозрачность индикаторов и выключаем их GameObject'ы
        if (approveOverlay != null)
        {
            approveOverlay.alpha = 0f;
            approveOverlay.gameObject.SetActive(false);
        }
        if (rejectOverlay != null)
        {
            rejectOverlay.alpha = 0f;
            rejectOverlay.gameObject.SetActive(false);
        }

        // Условие ЖЕСТКОЕ: успех ТОЛЬКО если ВСЕ карточки распределены верно
        bool isSuccess = (correctSwipesCount == cardsList.Count);

        if (isSuccess)
        {
            if (GameManager.Instance != null) GameManager.Instance.AddXP(100);
            if (mascotController != null)
                mascotController.DisplayDialog("Шедеврально! Фуры улетели вовремя. С марта по сентябрь у нас пик сезона и жара, но с тобой логистика работает как швейцарские часы! ", MascotEmotion.Joyful, historicalFact);
        }
        else
        {
            if (GameManager.Instance != null) GameManager.Instance.AddXP(20);
            if (mascotController != null)
                mascotController.DisplayDialog("Ой, кажется, коробка уехала не туда, а кота чуть не отправили в командировку! С марта по сентябрь объёмы колоссальные, глаза разбегаются, с опытом станет легче!", MascotEmotion.Sad, historicalFact);
        }

        furtherButton.gameObject.SetActive(true);
    }

    private void GoToFinalScreen()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameState.Final_HR);
            Debug.Log("[Level3] Переход к Финалу и HR-конверсии");
        }
    }
}
