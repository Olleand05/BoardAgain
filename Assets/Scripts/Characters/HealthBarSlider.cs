using UnityEngine;
using UnityEngine.UI;

public class HealthBarSlider : MonoBehaviour
{
    public Slider slider;

    private void EnsureSlider()
    {
        if (slider == null) slider = GetComponent<Slider>();
    }

    public void SetMaxHealth(int health)
    {
        EnsureSlider();


        slider.minValue = 0;
        slider.maxValue = health;


        slider.value = health;


        if (slider.fillRect != null)
        {

            LayoutRebuilder.ForceRebuildLayoutImmediate(slider.GetComponent<RectTransform>());
        }
    }

    public void SetHealth(int health)
    {
        EnsureSlider();
        slider.value = Mathf.Clamp(health, 0, slider.maxValue);
    }
}