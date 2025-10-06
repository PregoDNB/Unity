using UnityEngine;
using TMPro;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { BiggerPaddle, AddLife, AddScore }

    public TextMeshProUGUI powerUpText; // Wordt opgehaald uit BouncyBall
    private PowerUpType type;

    void Start()
    {
        // Kies willekeurig type
        type = (PowerUpType)Random.Range(0, 3);

        // Koppel PowerUpText automatisch
        if (powerUpText == null && BouncyBall.Instance != null)
            powerUpText = BouncyBall.Instance.powerUpText;

        // Geef de PowerUp een kleur op basis van type
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            switch (type)
            {
                case PowerUpType.BiggerPaddle: sr.color = Color.blue; break;
                case PowerUpType.AddLife: sr.color = Color.green; break;
                case PowerUpType.AddScore: sr.color = Color.yellow; break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        BouncyBall ball = BouncyBall.Instance;
        PlayerMovement paddle = FindFirstObjectByType<PlayerMovement>();

        // Toon tekst in UI
        StartCoroutine(ShowPowerUpText());

        switch (type)
        {
            case PowerUpType.BiggerPaddle:
                paddle.ActivateBiggerPaddle();
                break;

            case PowerUpType.AddLife:
                ball.AddLife();
                break;

            case PowerUpType.AddScore:
                ball.AddRandomScore();
                break;
        }

        Destroy(gameObject);
    }

    private IEnumerator ShowPowerUpText()
    {
        if (powerUpText == null) yield break;

        switch (type)
        {
            case PowerUpType.BiggerPaddle: powerUpText.text = "Bigger Paddle!"; break;
            case PowerUpType.AddLife: powerUpText.text = "Extra Life!"; break;
            case PowerUpType.AddScore: powerUpText.text = "Bonus Score!"; break;
        }

        yield return new WaitForSeconds(3f);
        powerUpText.text = "";
    }
}