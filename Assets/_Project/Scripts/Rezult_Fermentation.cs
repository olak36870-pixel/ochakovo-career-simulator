using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Логика уровня: Обработка нажатия кнопки и переход на следующий этап/плейн
/// </summary>
public class Rezult_Fermentation : MonoBehaviour
{
    [Header("UI Элементы уровня")]
    [SerializeField] private GameObject currentBackground; // Текущий фон/плейн уровня
    [SerializeField] private Button actionButton;            // Кнопка для совершения действия/перехода

    [Header("Ссылки на менеджеры")]
    [SerializeField] private MascotController mascotController;

    private void Start()
    {
        // Включаем текущий фон при старте, если он задан
        if (currentBackground != null)
        {
            currentBackground.SetActive(true);
        }

        // Подписываемся на событие клика по кнопке
        if (actionButton != null)
        {
            actionButton.onClick.AddListener(OnActionButtonClicked);
        }
    }

    private void OnEnable()
    {
        InitLevel();
    }

    /// <summary>
    /// Инициализация уровня при его включении
    /// </summary>
    public void InitLevel()
    {
        string welcomeText = "Ты на новом этапе! Внимательно осмотрись и нажми на кнопку, чтобы продолжить наше путешествие.";

        if (mascotController != null)
        {
            mascotController.DisplayDialog(welcomeText, MascotEmotion.Normal);
        }
    }

    /// <summary>
    /// Метод, вызываемый при нажатии на кнопку
    /// </summary>
    private void OnActionButtonClicked()
    {
        Debug.Log("[LevelTransition] Кнопка нажата. Выключаем текущий плейн и переходим далее.");

        // Выключаем текущий фон/экран
        if (currentBackground != null)
        {
            currentBackground.SetActive(false);
        }

        // Начисляем очки опыта, если нужно (по аналогии с вашим кодом)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddXP(50);
        }

        // Переключаем стейт и экран
        SwitchToNextState();
    }

    /// <summary>
    /// Логика переключения на следующий уровень/плейн
    /// </summary>
    private void SwitchToNextState()
    {
        // Переключаем стейт в GameManager на следующий (например, Level3 или другой нужный)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameState.Form);
        }
    }
}
