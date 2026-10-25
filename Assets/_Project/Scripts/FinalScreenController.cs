using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Логика финального экрана и HR-конверсии по GDD
/// </summary>
public class FinalScreenController : MonoBehaviour
{
    [Header("UI Элементы Итогов")]
    [SerializeField] private TextMeshProUGUI totalXPText;
    [SerializeField] private TextMeshProUGUI careerStatusText;
    [SerializeField] private Button applyButton;
    [SerializeField] private GameObject backgroundFinal;

    [Header("Ссылки на Менеджеры")]
    [SerializeField] private MascotController mascotController;

    private void Start()
    {
        if (backgroundFinal != null)
        {
            backgroundFinal.SetActive(true);
        }

        if (applyButton != null)
        {
            applyButton.onClick.AddListener(OnApplyButtonClicked);
        }
    }

    private void OnEnable()
    {
        ShowFinalResults();
    }

    public void ShowFinalResults()
    {
        int finalXP = 0;
        if (GameManager.Instance != null)
        {
            finalXP = GameManager.Instance.progress.totalXP;
        }

        // Вывод итоговых очков
        if (totalXPText != null)
            totalXPText.text = $"Набрано очков: {finalXP} XP";

        // Расчет статуса по GDD
        string status = CalculateStatus(finalXP);
        if (careerStatusText != null)
            careerStatusText.text = status;

        // Финальное обращение Мастера Очакова по GDD
        string finalMessage = $"За каждой деятельностью есть человек! Позволь нам узнать какое направление тебя заинтересовало, твои ФИО и возможный рост, а я тебе сообщу, кем ты можешь стать в Очаково!";

        if (mascotController != null)
        {
            mascotController.DisplayDialog(finalMessage, MascotEmotion.Joyful);
        }
    }

    private string CalculateStatus(int xp)
    {
        if (xp >= 300) return "Главный инженер Очаково";
        if (xp >= 140) return "Ведущий специалист цеха";
        return "Перспективный стажёр";
    }

    private void OnApplyButtonClicked()
    {
        Application.Quit();
    }
}
