using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCounterText;
    [SerializeField] private Character character;
    [SerializeField] private Image healthBar;
    [SerializeField] private CanvasGroup gameOverScreen;
    [SerializeField] private CanvasGroup winScreen;
    [SerializeField] private CanvasGroup hudCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private Transform respawnPoint;

    private bool isFadingGameOver = false;
    private static UIManager instance = null;

    public static UIManager Instance => instance;

    private class PlayerStatistics
    {
        public int coinCounter = 0;
    }

    private PlayerStatistics playerStatistics;

    private void Awake()
    {
        instance = this;

        this.playerStatistics = new PlayerStatistics();
    }

    public void CollectCoin()
    {
        this.playerStatistics.coinCounter++;

        string coinText = $"{this.playerStatistics.coinCounter}";
        this.coinCounterText.text = coinText;
    }

    public void Respawn()
    {
        this.character.transform.position = this.respawnPoint.position;
        this.ResetCoinCounter();
        this.ResetHealthBar();
        this.isFadingGameOver = false;

        this.hudCanvasGroup.alpha = 1f;
        this.gameOverScreen.alpha = 0f;

        this.character.SetPlayerControllerState(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Win()
    {
        // Implement win condition logic here, such as displaying a win screen or transitioning to the next level.
        this.character.SetPlayerControllerState(false);
        StartCoroutine(FadeInWin());
    }

    private void ResetCoinCounter()
    {
        this.playerStatistics.coinCounter = 0;
        this.coinCounterText.text = "0";
    }

    private void ResetHealthBar()
    {
        this.character.ResetHealth();
    }

    private IEnumerator FadeInGameOver()
    {
        this.isFadingGameOver = true;
        

        float timer = 0f;
        while (timer < fadeDuration)
        {
            float percent = Mathf.Clamp01(timer / fadeDuration);
            hudCanvasGroup.alpha = 1f - percent;
            gameOverScreen.alpha = percent;
            yield return null;
            timer += Time.deltaTime;
        }

        this.hudCanvasGroup.alpha = 0f;
        this.gameOverScreen.alpha = 1f;
    }

    private IEnumerator FadeInWin()
    {    
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float percent = Mathf.Clamp01(timer / fadeDuration);
            hudCanvasGroup.alpha = 1f - percent;
            winScreen.alpha = percent;
            yield return null;
            timer += Time.deltaTime;
        }

        this.hudCanvasGroup.alpha = 0f;
        this.winScreen.alpha = 1f;
    }

    private void Update()
    {
        float healthPercentage = this.character.GetCurrentHealth() / this.character.GetMaxHealth();
        this.healthBar.fillAmount = healthPercentage;
        if (healthPercentage <= 0f && !isFadingGameOver)
        {
            // Disable player input
            this.character.SetPlayerControllerState(false);
            StartCoroutine(FadeInGameOver());
        }
    }
}
