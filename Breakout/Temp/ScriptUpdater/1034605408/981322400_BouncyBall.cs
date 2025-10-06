using System.Collections;
using UnityEngine;
using TMPro;

public class BouncyBall : MonoBehaviour
{
    public float minY = -5.5f;
    public float maxVelocity = 15f;
    public bool isMainBall = false; // Belangrijk: zet dit AAN voor je bal-prefab

    Rigidbody2D rb;

    // --- STATIC VARIABELEN: Gedeeld door ALLE ballen ---
    public static int score;

    public static int lives;
    public static int brickCount;

    // --- STATIC REFERENTIES: Gedeeld door ALLE ballen ---
    public static TextMeshProUGUI scoreTxt_s;

    public static GameObject[] livesImage_s;
    public static GameObject gameOverPanel_s;
    public static GameObject youWinPanel_s;
    public static TextMeshProUGUI powerUpText_s;
    public static PlayerMovement paddle_s;

    // --- Lokale referenties ---
    public TextMeshProUGUI scoreTxt;

    public GameObject[] livesImage;
    public GameObject gameOverPanel;
    public GameObject youWinPanel;
    public TextMeshProUGUI powerUpText;

    void Awake()
    {
        // De eerste bal die wordt gemaakt, stelt alle static referenties in.
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

        // Alleen de allereerste bal telt de bricks en reset de game state.
        if (isMainBall)
        {
            brickCount = FindObjectsOfType<Brick>().Length;
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
            // Alleen de main bal kan een leven kosten
            if (isMainBall && FindObjectsOfType<BouncyBall>().Length == 1)
            {
                LoseLife();
            }
            else
            {
                // Extra ballen verdwijnen gewoon
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
                youWinPanel_s.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    void LoseLife()
    {
        lives--;
        UpdateLivesUI();

        // Reset power-ups
        paddle_s.ResetSize();

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            // Respawn de bal
            transform.position = Vector3.zero;
            rb.linearVelocity = Vector2.zero;
            // Geef een nieuwe start-impuls
            rb.AddForce(new Vector2(Random.Range(-3f, 3f), 5f), ForceMode2D.Impulse);
        }
    }

    // --- Functies voor Power-ups ---
    public void SpawnExtraBall()
    {
        BouncyBall newBall = Instantiate(gameObject, transform.position, Quaternion.identity).GetComponent<BouncyBall>();
        newBall.isMainBall = false; // Belangrijk! De kloon is geen main bal.
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

    // --- Functies voor UI updates ---
    void UpdateScoreText() { scoreTxt_s.text = score.ToString("00000"); }

    void UpdateLivesUI()
    {
        for (int i = 0; i < livesImage_s.Length; i++)
        {
            livesImage_s[i].SetActive(i < lives);
        }
    }

    void GameOver()
    {
        gameOverPanel_s.SetActive(true);
        Time.timeScale = 0;
        Destroy(gameObject);
    }

    // --- Functies voor Power-up boodschap ---
    public void ShowPowerUpMessage(string message) { StartCoroutine(ShowMessageRoutine(message)); }

    private IEnumerator ShowMessageRoutine(string message)
    {
        powerUpText_s.text = message;
        powerUpText_s.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        powerUpText_s.gameObject.SetActive(false);
    }
}