using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PhoneInputHandler : MonoBehaviour
{
    [Header("UI Элементы")]
    [SerializeField] private InputField phoneInputField;
    [SerializeField] private Button submitPhoneButton; // Кнопка "ГОТОВО"
    [SerializeField] private Button startButton;       // Кнопка "ПОГНАЛИ!"
    [SerializeField] private GameObject background0;
    [SerializeField] private GameObject Form;

    [Header("Ссылки")]
    [SerializeField] private MascotController mascotController;
    [SerializeField] private UIManager uiManager;

    [Header("Google Таблица")]
    [Tooltip("URL веб-приложения (/exec) из Google Apps Script")]
    [SerializeField] private string webAppUrl = "https://script.google.com/macros/s/AKfycbzkr3QLC_nl-4nav5gjleIna2GAECkdDc9_1A_DliMNLxblwoSkt7gx0aduznQIntgQ/exec";

    private string savedPhoneNumber = "";

    private void Awake()
    {
        if (background0 != null)
        {
            background0.SetActive(true);
        }
        // Вешаем слушатель на кнопку "ГОТОВО"
        if (submitPhoneButton != null)
        {
            submitPhoneButton.onClick.AddListener(OnSubmitClicked);
        }

        // Вешаем слушатель на кнопку "ПОГНАЛИ!" и изначально скрываем её
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            startButton.gameObject.SetActive(false);
        }
    }

    private void OnSubmitClicked()
    {
        if (phoneInputField != null)
        {
            savedPhoneNumber = phoneInputField.text.Trim();

            // Проверка на пустое поле
            if (string.IsNullOrEmpty(savedPhoneNumber))
            {
                if (mascotController != null)
                {
                    mascotController.DisplayDialog("Пожалуйста, введи номер телефона перед продолжением!", MascotEmotion.Sad);
                }
                return;
            }
        }

        Debug.Log($"[PhoneInputHandler] Номер принят: {savedPhoneNumber}");

        // Отправляем данные в Google Таблицу без корутин
        SendToGoogleSheets(savedPhoneNumber);

        // Речь маскота после успешного ввода
        if (mascotController != null)
        {
            mascotController.DisplayDialog("Отлично! Теперь жми «ПОГНАЛИ!» и начинаем экскурсию!", MascotEmotion.Joyful);
        }

        // Выключаем объект Form
        if (Form != null) Form.SetActive(false);

        // Скрываем форму ввода и кнопку "ГОТОВО", открываем кнопку "ПОГНАЛИ!"
        if (phoneInputField != null) phoneInputField.gameObject.SetActive(false);
        if (submitPhoneButton != null) submitPhoneButton.gameObject.SetActive(false);
        if (startButton != null) startButton.gameObject.SetActive(true);
    }

    private void SendToGoogleSheets(string phone)
    {
        if (string.IsNullOrEmpty(webAppUrl) || webAppUrl.Contains(""))
        {
            Debug.LogWarning("[GoogleSheets] URL веб-приложения не настроен в инспекторе!");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("phone", phone);

        UnityWebRequest www = UnityWebRequest.Post(webAppUrl, form);
        var operation = www.SendWebRequest();

        // Обработка ответа сервера без корутин (через событие)
        operation.completed += (asyncOp) =>
        {
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("[GoogleSheets] Ошибка отправки: " + www.error);
            }
            else
            {
                Debug.Log("[GoogleSheets] Номер успешно сохранен в таблице!");
            }
            www.Dispose();
        };
    }

    private void OnStartButtonClicked()
    {
        background0.gameObject.SetActive(false);
        Debug.Log("[PhoneInputHandler] Нажата кнопка ПОГНАЛИ! Переход на Уровень 1.");

        // Переключаем стейт в GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameState.Level1_Quality);
        }

        // Переключаем экран через UIManager
        if (uiManager != null)
        {
            uiManager.ShowLevel1();
        }
    }

    public string GetPhoneNumber() => savedPhoneNumber;
}
