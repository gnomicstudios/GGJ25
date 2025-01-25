using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public TextMeshProUGUI bubblesText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI levelText;
    public RectTransform progressBar;
    public Color progressColorNormal;
    public Color progressColorHit;
    public Color progressColorGrow;


    private Image progressBarImage;

    private GameManager game;
    private int bubbles = 0;
    private int lives = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        game = FindFirstObjectByType<GameManager>();
        levelText.text = game.level.ToString();
        progressBarImage = progressBar.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (game.bubbles != bubbles) {
            bubbles = game.bubbles;
            bubblesText.text = bubbles.ToString();
        }
        if (game.lives != lives) {
            lives = game.lives;
            livesText.text = lives.ToString();
        }
        progressBar.localScale = new Vector3(game.CoverageProportion, 1.0f, 1.0f);

        if (game.coverageExtra > 0.0f) {
            progressBarImage.color = Color.Lerp(progressColorNormal, progressColorGrow, game.coverageExtra / 10f);
        } else {
            progressBarImage.color = progressColorNormal;
        }
    }
}
