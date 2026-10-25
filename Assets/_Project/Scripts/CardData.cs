using UnityEngine;

[System.Serializable]
public class LogisticsCard
{
    public string cardName;          // Название объекта/продукции
    public Sprite cardSprite;        // Картинка на карточке
    public bool isCorrectProduct;    // true = отправляем в фуру (вправо), false = в утиль (влево)
}
