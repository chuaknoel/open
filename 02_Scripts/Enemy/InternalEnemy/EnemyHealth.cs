using UnityEngine;
using UnityEngine.UI;


public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100.0f;
    private float currentHealth;

    private Slider healthSlider;
    [SerializeField] private Image fillImage;

   
    public void Init()
    {
        currentHealth = maxHealth;

        healthSlider = GetComponent<Slider>();
        UpdateHealthBar();
    }

    public void ResetHealthBar()
    {
        healthSlider.value = 1;
        Color newColor = Color.Lerp(Color.red, Color.green, 1);
        fillImage.color = newColor;
    }

    public void DamagedEffect(float currentHP, float maxHP)
    {
        currentHealth = Mathf.Clamp(currentHP / (maxHP / maxHealth), 0, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthSlider.value = (int)currentHealth / maxHealth;

        Color newColor = Color.Lerp(Color.red, Color.green, (int)currentHealth / maxHealth);
        fillImage.color = newColor;
    }
}
