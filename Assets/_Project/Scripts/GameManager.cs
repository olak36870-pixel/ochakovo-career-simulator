using UnityEngine;

/// <summary>
/// Все возможные состояния игровой сессии по GDD "Очаково"
/// </summary>
public enum GameState
{
    StartScreen,       // Экран 0: Приветствие
    Level1_Quality,    // Уровень 1: Брожение (Качество)
    Level2_Engineering,// Уровень 2: Инженерный цех
    Level3_Logistics,  // Уровень 3: Склад и логистика
    Form,
    Final_HR           // Финал: Итоги и HR-форма
}

/// <summary>
/// Класс для хранения данных и очков игрока
/// </summary>
[System.Serializable]
public class PlayerProgress
{
    public int totalXP = 0;
    public string selectedDirection = "";
}

/// <summary>
/// Главный управляющий скрипт игры (GameManager).
/// </summary>
public class GameManager : MonoBehaviour
{
    // Публичный доступ к синглтону из любого другого скрипта
    public static GameManager Instance { get; private set; }

    [Header("Текущее состояние игры")]
    public GameState currentState = GameState.StartScreen;

    [Header("Прогресс игрока")]
    public PlayerProgress progress = new PlayerProgress();

    // ИСПРАВЛЕНИЕ: Добавляем ссылки на UI панели каждого экрана/уровня
    [Header("UI Панели экранов")]
    [SerializeField] private GameObject startScreenPanel;
    [SerializeField] private GameObject level1Panel;
    [SerializeField] private GameObject level2Panel;
    [SerializeField] private GameObject level3Panel;
    [SerializeField] private GameObject form;
    [SerializeField] private GameObject finalHRPanel;

    private void Awake()
    {
        // Реализация паттерна Singleton (Единственный экземпляр)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняем объект между возможностью смены сцен
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // При старте принудительно включаем стартовый экран
        UpdateVisualStates();
    }

    /// <summary>
    /// Метод для начисления очков XP (по GDD: +100 за успех, +20 за ошибку)
    /// </summary>
    public void AddXP(int amount)
    {
        progress.totalXP += amount;
        Debug.Log($"[GameManager] Добавлено XP: {amount}. Всего XP: {progress.totalXP}");
    }

    /// <summary>
    /// Переключение текущего состояния игры
    /// </summary>
    public void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log($"[GameManager] Состояние игры изменено на: {newState}");

        // ИСПРАВЛЕНИЕ: Физически переключаем экраны при смене состояния
        UpdateVisualStates();
    }

    /// <summary>
    /// Включает нужную панель и выключает все остальные в зависимости от currentState
    /// </summary>
    private void UpdateVisualStates()
    {
        if (startScreenPanel != null) startScreenPanel.SetActive(currentState == GameState.StartScreen);
        if (level1Panel != null) level1Panel.SetActive(currentState == GameState.Level1_Quality);
        if (level2Panel != null) level2Panel.SetActive(currentState == GameState.Level2_Engineering);
        if (level3Panel != null) level3Panel.SetActive(currentState == GameState.Level3_Logistics);
        if (form != null) form.SetActive(currentState == GameState.Form);
        if (finalHRPanel != null) finalHRPanel.SetActive(currentState == GameState.Final_HR);
    }
}
