using UnityEngine;
using TMPro;

public class HudController : MonoBehaviour
{
    public TextMeshProUGUI bubblesText;
    public TextMeshProUGUI livesText;


    private GameManager game;
    private int bubbles = 0;
    private int lives = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        game = FindFirstObjectByType<GameManager>();
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
    }
}
