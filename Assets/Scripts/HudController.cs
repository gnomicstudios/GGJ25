using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public TextMeshProUGUI bubblesText;
    public TextMeshProUGUI levelText;
    public RectTransform progressBar;
    public bool progressBarVertical = true;
    public Color progressColorNormal;
    public Color progressColorHit;
    public Color progressColorGrow;

    public SlideInUI gameStartScreen;
    public SlideInUI levelCompleteScreen;
    public SlideInUI gameOverScreen;

    private Image progressBarImage;

    private GameManager game;
    private int bubbles = 0;
    private int level = 0;
    private float coverage = 0.0f;

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
        if (game.level != level) {
            level = game.level;
            levelText.text = level.ToString();
        }

        if (progressBarVertical) {
            progressBar.localScale = new Vector3(1.0f, game.CoverageProportion, 1.0f);
        } else {
            progressBar.localScale = new Vector3(game.CoverageProportion, 1.0f, 1.0f);
        }

        if (game.CoverageProportion != coverage) {
            LeanTween.value(coverage, game.CoverageProportion, 1f).setOnUpdate(UpdateProgressBar).setEase(LeanTweenType.easeOutCubic);
            coverage = game.CoverageProportion;

            LeanTween.value(0f, 1f, 1f).setOnUpdate(UpdateProgressColorGrow).setEase(LeanTweenType.easeInOutBounce);

            // LeanTween.color(progressBar.gameObject, progressColorGrow, 0.5f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() => {
            //     LeanTween.color(progressBar.gameObject, progressColorNormal, 0.5f).setEase(LeanTweenType.easeInOutCubic);
            // });
        }

        // if (game.coverageExtra > 0.0f) {
        //     progressBarImage.color = Color.Lerp(progressColorNormal, progressColorGrow, game.coverageExtra / 10f);
        // } else {
        //     progressBarImage.color = progressColorNormal;
        // }
    }

    void UpdateProgressBar(float value) {
        if (progressBarVertical) {
            progressBar.localScale = new Vector3(1.0f, value, 1.0f);
        } else {
            progressBar.localScale = new Vector3(value, 1.0f, 1.0f);
        }
    }

    void UpdateProgressColorGrow(float value) {
        progressBarImage.color = Color.Lerp(progressColorNormal, progressColorGrow, value);
    }

    void UpdateProgressColorHit(float value) {
        progressBarImage.color = Color.Lerp(progressColorNormal, progressColorHit, value);
    }
}
