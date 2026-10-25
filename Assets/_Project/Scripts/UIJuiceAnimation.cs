using UnityEngine;

/// <summary>
/// Простой скрипт для эффекта дыхания/покачивания UI-элемента.
/// </summary>
public class UIJuiceAnimation : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 2f;    // Скорость покачивания
    [SerializeField] private float floatAmount = 10f;  // Амплитуда движения

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        // Вычисляем новое положение по оси Y через синусоиду
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.localPosition = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}
