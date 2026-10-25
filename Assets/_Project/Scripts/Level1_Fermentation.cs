using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Логика Уровня 1: «Цех Натурального Брожения» (Механика Тайминг-клика по GDD)
/// </summary>
public class Level1_Fermentation : MonoBehaviour
{
    [Header("UI Элементы уровня")]
    [SerializeField] private RectTransform flaskRect;      // Прямоугольник колбы
    [SerializeField] private GameObject background1;
    [SerializeField] private RectTransform markerRect;     // Маркер (ВАЖНО: Pivot Y в Инспекторе должен быть 0!)
    [SerializeField] private RectTransform greenZoneRect;  // Зеленая зона
    [SerializeField] private Button stopButton;            // Кнопка "Стоп"
    [SerializeField] private Button futherButton;          // Кнопка "Далее"
    string historicalFact = "В 1980 году во время Олимпиады в Москве «Очаково» стал главным поставщиком освежающих напитков для спортсменов и гостей со всего мира! Завод был единственным местом на планете, где одновременно разливали напитки вечных конкурентов — Pepsi и Fanta. Наши технологи до сих пор сохраняют стандарты самого легендарного качества!";

    [Header("Настройки движения")]
    [SerializeField] private float moveSpeed = 300f;       // Скорость изменения высоты маркера
    [SerializeField] private float maxHeight = 800f;       // НАСТРОЙКА: Максимальная высота подъема маркера
    private bool isGameActive = false;
    private float initialTime;                             // Фиксация времени старта

    [Header("Ссылки на менеджеры")]
    [SerializeField] private MascotController mascotController;

    private void Start()
    {
        if (background1 != null)
        {
            background1.SetActive(true);
        }

        if (stopButton != null)
        {
            stopButton.onClick.AddListener(OnStopButtonClicked);
        }
   
    }

    private void OnEnable()
    {
        StartMiniGame();
    }

    public void StartMiniGame()
    {
        isGameActive = true;
        initialTime = Time.time;

        string introText = "Первая остановка — цех брожения! Жми „Стоп“, когда маркер окажется в зелёной зоне. Не попадёшь — устроим внеплановый запуск кваса в космос!";

        if (mascotController != null)
        {
            mascotController.DisplayDialog(introText, MascotEmotion.Normal);
        }
    }

    private void Update()
    {
        if (!isGameActive) return;

        // Изменение значения от 0 до настроенной maxHeight по PingPong
        float currentHeight = Mathf.PingPong((Time.time - initialTime) * moveSpeed, maxHeight);

        // ЖЕСТКО: Меняем ТОЛЬКО вертикальную ось (высоту). Ширина берется текущая.
        markerRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currentHeight);
    }

    private void OnStopButtonClicked()
    {
        if (!isGameActive) return;
        isGameActive = false; // Останавливаем игру

        // ВАЖНО: Настоящий верхний край маркера на панели — это его Pos Y + его текущая высота
        float currentMarkerTopEdge = markerRect.anchoredPosition.y + markerRect.rect.height;

        // Границы зеленой зоны относительно центра панели (Pos Y: 235.42)
        float greenZoneMinY = greenZoneRect.anchoredPosition.y - (greenZoneRect.rect.height / 2f);
        float greenZoneMaxY = greenZoneRect.anchoredPosition.y + (greenZoneRect.rect.height / 2f);

        // Проверяем попадание
        bool isSuccess = (currentMarkerTopEdge >= greenZoneMinY && currentMarkerTopEdge <= greenZoneMaxY);

        if (isSuccess)
        {
            if (GameManager.Instance != null) GameManager.Instance.AddXP(100);
            mascotController.DisplayDialog("Святые дрожжи, ну ты и машина! Сработал просто космически круто! Твой глазомер настолько точен, что если бы тебя занесло на Олимпиаду-80, бренд «Очаково» доверил бы тебе самый важный конвейер страны!", MascotEmotion.Joyful, historicalFact);
            Debug.Log("[Level1] Успех! Маркер дотянулся до зеленой зоны.");
        }
        else
        {
            if (GameManager.Instance != null) GameManager.Instance.AddXP(20);
            mascotController.DisplayDialog("Ой! Пенный фонтан улетел выше ракеты Илона Маска. Спортсмены с Олимпиады-1980 были бы в шоке. Но на практике научим варить чётко по ГОСТу. Едем дальше!", MascotEmotion.Sad, historicalFact);
            Debug.Log($"[Level1] Промах! Верх маркера: {currentMarkerTopEdge}, а зона: от {greenZoneMinY} до {greenZoneMaxY}");
        }

        stopButton.gameObject.SetActive(false);
        futherButton.gameObject.SetActive(true);
    }

    private void GoToNextLevel()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameState.Level2_Engineering);
        }
    }

    public void OnFurtherButtonClicked()
    {
        GoToNextLevel();
    }
}
