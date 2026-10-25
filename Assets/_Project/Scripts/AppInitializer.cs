using UnityEngine;

/// <summary>
/// —тартовый системный скрипт.
/// «апускаетс€ при старте игры и настраивает базовые параметры приложени€.
/// </summary>
public class AppInitializer : MonoBehaviour
{
    private void Awake()
    {
        // ќграничиваем частоту кадров до 60 FPS, чтобы не перегружать процессоры мобильных устройств
        Application.targetFrameRate = 60;

        // «апрещаем экрану устройства уходить в сп€щий режим / гаснуть во врем€ игры
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Debug.Log("[AppInitializer] ѕриложение успешно инициализировано под WebGL (60 FPS, SleepTimeout: Never).");
    }
}

