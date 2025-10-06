using System.Collections;
using UnityEngine;
using TMPro;

public class BouncyBall : MonoBehaviour
{
    public float minY = -5.5f;
    public float maxVelocity = 15f;
    public float paddleHitSpeedIncrease = 1.05f; // 5% sneller per paddle-tik

    private Rigidbody2D rb;
    private static bool isShuttingDown = false; // Noodstop-variabele

    // Static variabelen die als centraal brein fungeren
    public static int score;

    public static int lives;
    public static int brickCount;
    public static TextMeshProUGUI scoreTxt_s;
    public static GameObject[] livesImage_s;
    public static GameObject gameOverPanel_s;
    public static GameObject youWinPanel_s;
    public static TextMeshProUGUI powerUpText_s;
    public static PlayerMovement paddle_s;
    public static GameObject ballPrefab_s;

    // Lokale referenties om de static variabelen in te stellen
    public TextMeshProUGUI scoreTxt;

    public GameObject[] livesImage;
    public GameObject gameOverPanel;
    public GameObject youWinPanel;
    public TextMeshProUGUI powerUpText;
    public GameObject ballPrefab;

    void Awake()
    {
        if (scoreTxt_s == null)
        {
            scoreTxt_s = scoreTxt;
            livesImage_s = livesImage;
            gameOverPanel_s = gameOverPanel;
            youWinPanel_s = youWinPanel;
            powerUpText_s = powerUpText;
            paddle_s = FindFirstObjectByType<PlayerMovement>();
            ballPrefab_s = ballPrefab;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (FindObjectsByType<BouncyBall>(FindObjectsSortMode.None).Length == 1)
        {
            isShuttingDown = false; // Reset de noodstop bij een nieuwe start
            brickCount = FindObjectsByType<Brick>(FindObjectsSortMode.None).Length;
            lives = 5;
            score = 0;
            UpdateScoreText();
            UpdateLivesUI();
        }

        if (rb.linearVelocity.magnitude < 0.1f)
        {
            rb.AddForce(new Vector2(Random.Range(-3f, 3f), 5f), ForceMode2D.Impulse);
        }
    }

    void Update()
    {
        if (transform.position.y < minY)
        {
            Destroy(gameObject);
        }

        if (rb.linearVelocity.magnitude > maxVelocity)
        {
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxVelocity);
        }
    }

    public void InitializeState()
    {
        brickCount = FindObjectsByType<Brick>(FindObjectsSortMode.None).Length;
        lives = 5;
        score = 0;
        UpdateScoreText();
        UpdateLivesUI();
    }

    void OnApplicationQuit()
    {
        isShuttingDown = true;
    }

    // Deze functie wordt AANgeroepen net voordat de bal wordt vernietigd.
    void OnDestroy()
    {
        // Als het spel afsluit, doe dan niets.
        if (isShuttingDown) return;

        if (FindObjectsByType<BouncyBall>(FindObjectsSortMode.None).Length <= 1)
        {
            if (Time.timeScale > 0 && lives > 0)
            {
                LoseLife();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Logica voor het raken van een brick
        if (collision.gameObject.CompareTag("Brick"))
        {
            collision.gameObject.GetComponent<Brick>().GetHit();
            score += 10;
            UpdateScoreText();
            brickCount--;
            if (brickCount <= 0)
            {
                if (youWinPanel_s != null) youWinPanel_s.SetActive(true);
                Time.timeScale = 0;
            }
        }
        // HERSTELD: Logica voor het raken van de paddle
        else if (collision.gameObject.CompareTag("Player"))
        {
            rb.linearVelocity *= paddleHitSpeedIncrease;
        }
    }

    void LoseLife()
    {
        lives--;
        UpdateLivesUI();

        if (paddle_s != null) paddle_s.ResetSize();

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            if (ballPrefab_s != null)
            {
                Instantiate(ballPrefab_s, Vector3.zero, Quaternion.identity);
            }
        }
    }

    public void SpawnExtraBall()
    {
        if (ballPrefab_s != null)
        {
            Instantiate(ballPrefab_s, transform.position, Quaternion.identity);
        }
    }

    public void AddLife()
    {
        if (lives < livesImage_s.Length)
        {
            lives++;
            UpdateLivesUI();
        }
    }

    public void AddRandomScore()
    {
        score += Random.Range(1, 101);
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreTxt_s != null) scoreTxt_s.text = score.ToString("00000");
    }

    void UpdateLivesUI()
    {
        if (livesImage_s == null) return;
        for (int i = 0; i < livesImage_s.Length; i++)
        {
            livesImage_s[i].SetActive(i < lives);
        }
    }

    void GameOver()
    {
        if (gameOverPanel_s != null) gameOverPanel_s.SetActive(true);
        Time.timeScale = 0;
    }

    public void ShowPowerUpMessage(string message)
    {
        if (this != null && gameObject.activeInHierarchy) StartCoroutine(ShowMessageRoutine(message));
    }

    private IEnumerator ShowMessageRoutine(string message)
    {
        if (powerUpText_s != null)
        {
            powerUpText_s.text = message;
            powerUpText_s.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            powerUpText_s.gameObject.SetActive(false);
        }
    }
}