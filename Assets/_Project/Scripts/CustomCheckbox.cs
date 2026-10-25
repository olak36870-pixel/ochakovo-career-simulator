using UnityEngine;
using UnityEngine.UI;

public class CustomCheckbox : MonoBehaviour
{
    [SerializeField] private GameObject checkmarkImage;
    private bool isChecked = false;

    public bool IsChecked => isChecked;

    public void ToggleCheckbox()
    {
        isChecked = !isChecked;
        checkmarkImage.SetActive(isChecked);
    }
}