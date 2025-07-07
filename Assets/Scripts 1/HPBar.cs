using UnityEngine;
using UnityEngine.UI;

public class HP_Bar : MonoBehaviour
{
    public Player player;
    public Image healthBarBG;

    [SerializeField] private RectTransform healthBarFill;

    private float height;
    private float fullWidth;

    void Start()
    {
        height = healthBarFill.rect.height;
        fullWidth = healthBarFill.rect.width;

        UpdateHealthBar(player.g_playerHP);
    }

    public void UpdateHealthBar(int hp)
    {
        // FIXED: Cast to float to prevent integer division
        float fill = Mathf.Clamp01((float)hp / player.g_playerMaxHP);

        // Adjust only the width of the fill bar, preserve full height
        healthBarFill.sizeDelta = new Vector2(fullWidth * fill, height);
    }
}


