using System.Collections;
using UnityEngine;
using TMPro;

public class BouncyBall : MonoBehaviour
{
    public float minY = -5.5f;
    public float maxVelocity = 15f;
    public bool isMainBall = false;

    Rigidbody2D rb;

    public static int score;
    public static int lives;
    public static int brickCount;

    public static TextMeshProUGUI scoreTxt_s;
    public static GameObject[] livesImage_s;
    public static GameObject gameOverPanel_s;
    public static GameObject youWinPanel_s;
    public static TextMeshProUGUI powerUpText_s;
    public static PlayerMovement paddle_s;

    public TextMeshProUGUI scoreTxt;
    public GameObject[] livesImage;
    public GameObject gameOverPanel;
    public GameObject youWinPanel;
    public TextMeshProUGUI powerUpText;

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
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (isMainBall)
        {
            // --- GECORRIGEERD ---
            brickCount = FindAnyObjectByType<Brick>().Length; // Zoek ALLE bricks
            lives = 5;
            score = 0;
            UpdateScoreText();
            UpdateLivesUI();
        }
    }

    void Update()
    {
        if (transform.position.y < minY)
        {
            // --- GECORRIGEERD ---
            // Controleer of dit de laatste bal in het spel is
            if (isMainBall && FindAnyObjectByType<BouncyBall>().Length == 1)
            {
                LoseLife();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        if (rb.linearVelocity.magnitude > maxVelocity)
        {
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxVelocity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
            transform.position = Vector3.zero;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(Random.Range(-3f, 3f), 5f), ForceMode2D.Impulse);
        }
    }

    public void SpawnExtraBall()
    {
        BouncyBall newBall = Instantiate(gameObject, transform.position, Quaternion.identity).GetComponent<BouncyBall>();
        newBall.isMainBall = false;
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
        Destroy(gameObject);
    }

    public void ShowPowerUpMessage(string message)
    {
        if (this != null) StartCoroutine(ShowMessageRoutine(message));
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