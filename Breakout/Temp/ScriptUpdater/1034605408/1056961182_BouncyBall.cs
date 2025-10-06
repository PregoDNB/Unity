using UnityEngine;

using TMPro;
using System.Collections;

public class BouncyBall : MonoBehaviour

{
    public float minY = -5.5f;
    public float maxVelocity = 15f;
    Rigidbody2D rb;
    int score = 0;
    int lives = 5;
    public TextMeshProUGUI scoreTxt;
    public GameObject[] livesImage;
    public GameObject gameOverPanel;
    public GameObject youWinPanel;
    public float paddleHitSpeedIncrease = 1.05f; // 5% sneller per paddle-tik

    public TextMeshProUGUI powerUpText;
    int brickCount;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.y < minY)
        {
            if (lives <= 0)
            {
                GameOver();
            }
            else
            {
                rb.linearVelocity = Vector3.zero;
                lives--;
                livesImage[lives].SetActive(false);
            }
        }

        if (rb.linearVelocity.magnitude > maxVelocity)
        {
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxVelocity);
        }
    }

    public void InitializeBrickCount()
    {
        brickCount = FindObjectsByType<Brick>(FindObjectsSortMode.None).Length;
    }

    public void SpawnExtraBall()
    {
        Instantiate(gameObject, transform.position, Quaternion.identity);
    }

    public void AddLife()
    {
        if (lives < livesImage.Length) // Zorg dat we niet meer levens hebben dan plaatjes
        {
            livesImage[lives].SetActive(true); // Zet een hartje AAN
            lives++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            collision.gameObject.GetComponent<Brick>().GetHit();
            score += 10;
            scoreTxt.text = score.ToString("00000");
            brickCount--;
            if (brickCount <= 0)
            {
                youWinPanel.SetActive(true);
                Time.timeScale = 0;
            }
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            // Verhoog de snelheid een klein beetje bij het raken van de paddle
            rb.linearVelocity *= paddleHitSpeedIncrease;
        }
    }

    public void AddRandomScore()
    {
        score += Random.Range(1, 101); // Voeg een willekeurige score toe
        scoreTxt.text = score.ToString("00000");
    }

    void GameOver()

    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        Destroy(gameObject);
    }

    public void ShowPowerUpMessage(string message)
    {
        StartCoroutine(ShowMessageRoutine(message));
    }

    // Deze coroutine regelt de timing
    private IEnumerator ShowMessageRoutine(string message)
    {
        powerUpText.text = message;
        powerUpText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f); // Toon de tekst voor 2 seconden

        powerUpText.gameObject.SetActive(false);
    }
}