using TMPro;
using UnityEngine;

public class GameStatsController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private TMP_Text comboText;
    private int latestCombo = 0;

    private void Start()
    {
        pointsText.text = $"Puntos: 0";
        comboText.text = "";
    }

    private void Update()
    {
        pointsText.text = $"Puntos: {gameManager.score}";
        if (latestCombo != gameManager.combo)
        {
            latestCombo = gameManager.combo;
            comboText.text = gameManager.combo == 0 ? "" : $"Combo x{gameManager.combo}";
        }
    }
}
