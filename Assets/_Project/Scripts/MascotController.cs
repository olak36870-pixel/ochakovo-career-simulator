using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Эмоции Мастера Очакова по GDD
/// </summary>
public enum MascotEmotion
{
    Normal,   // Обычное / Разговор
    Joyful,   // Радостное (успех)
    Sad,      // Печальное (ошибка)
    Think     // Думает
}

public class MascotController : MonoBehaviour
{
    [Header("UI Элементы Маскота")]
    [SerializeField] private Image mascotImage;
    [SerializeField] private TextMeshProUGUI speechBubbleText;

    [Header("Спрайты Эмоций")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite joyfulSprite;
    [SerializeField] private Sprite sadSprite;
    [SerializeField] private Sprite thinkSprite;

    [Header("Поп-ап Исторического Факта")]
    [SerializeField] private GameObject factPopupPanel;
    [SerializeField] private TextMeshProUGUI factText;
    [SerializeField] private GameObject factButton; // лампочка

    private string currentFactText = "";
    private MascotEmotion lastEmotion = MascotEmotion.Normal; // Переменная для сохранения предыдущей эмоции

    private void Start()
    {
        // По умолчанию скрываем окно исторического факта
        if (factPopupPanel != null)
            factPopupPanel.SetActive(false);
    }

    /// <summary>
    /// Обновить отображение реплики и эмоции Мастера Очакова
    /// </summary>
    public void DisplayDialog(string message, MascotEmotion emotion, string historicalFact = "")
    {
        speechBubbleText.text = message;
        currentFactText = historicalFact;
        lastEmotion = emotion; // Запоминаем текущую эмоцию основного диалога

        if (historicalFact == "") factButton.SetActive(false);
        else factButton.SetActive(true);

        ApplySpriteByEmotion(emotion);
    }

    /// <summary>
    /// Вызывается при нажатии на иконку вопроса / лампочки (?)
    /// </summary>
    public void OnFactButtonClicked()
    {
        if (string.IsNullOrEmpty(currentFactText)) return;

        factText.text = currentFactText;
        factPopupPanel.SetActive(true);

        // Меняем эмоцию на Think при открытии факта
        ApplySpriteByEmotion(MascotEmotion.Think);
    }

    /// <summary>
    /// Закрыть окно исторического факта
    /// </summary>
    public void OnCloseFactClicked()
    {
        factPopupPanel.SetActive(false);

        // Возвращаем прошлую эмоцию, которая была до открытия факта
        ApplySpriteByEmotion(lastEmotion);
    }

    /// <summary>
    /// Вспомогательный метод для смены спрайта маскота по типу эмоции
    /// </summary>
    private void ApplySpriteByEmotion(MascotEmotion emotion)
    {
        if (mascotImage == null) return;

        switch (emotion)
        {
            case MascotEmotion.Normal:
                if (normalSprite != null) mascotImage.sprite = normalSprite;
                break;
            case MascotEmotion.Joyful:
                if (joyfulSprite != null) mascotImage.sprite = joyfulSprite;
                break;
            case MascotEmotion.Sad:
                if (sadSprite != null) mascotImage.sprite = sadSprite;
                break;
            case MascotEmotion.Think:
                if (thinkSprite != null) mascotImage.sprite = thinkSprite;
                break;
        }
    }
}
