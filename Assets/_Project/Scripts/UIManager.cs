using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Ссылки на панели")]
    [SerializeField] private GameObject startScreenPanel;
    [SerializeField] private GameObject level1Panel;
    [SerializeField] private GameObject level2Panel;
    [SerializeField] private GameObject level3Panel;
    [SerializeField] private GameObject finalScreenPanel;

    [Header("Кнопка старта")]
    [SerializeField] private Button startButton; // Кнопка "ПОГНАЛИ!"

    [Header("Ссылка на Маскота")]
    [SerializeField] private MascotController mascotController;

    private void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        ShowStartScreen();
    }

    public void ShowStartScreen()
    {
        HideAllScreens();
        if (startScreenPanel != null) startScreenPanel.SetActive(true);

        string welcomeText = "Привет! На связи Мастер Очаково. Оставь свой номер телефона, чтобы сохранить результаты, и мы отправляемся!";
        if (mascotController != null)
        {
            mascotController.DisplayDialog(welcomeText, MascotEmotion.Normal);
        }
    }

    private void OnStartButtonClicked()
    {
        Debug.Log("[UIManager] Нажата кнопка ПОГНАЛИ! Переход на Уровень 1.");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameState.Level1_Quality);
        }
        ShowLevel1();
    }

    public void ShowLevel1()
    {
        HideAllScreens();
        if (level1Panel != null) level1Panel.SetActive(true);
    }

    public void ShowLevel2()
    {
        HideAllScreens();
        if (level2Panel != null) level2Panel.SetActive(true);
    }

    public void ShowLevel3()
    {
        HideAllScreens();
        if (level3Panel != null) level3Panel.SetActive(true);
    }

    public void ShowFinalScreen()
    {
        HideAllScreens();
        if (finalScreenPanel != null) finalScreenPanel.SetActive(true);
    }

    private void HideAllScreens()
    {
        if (startScreenPanel != null) startScreenPanel.SetActive(false);
        if (level1Panel != null) level1Panel.SetActive(false);
        if (level2Panel != null) level2Panel.SetActive(false);
        if (level3Panel != null) level3Panel.SetActive(false);
        if (finalScreenPanel != null) finalScreenPanel.SetActive(false);
    }
}

