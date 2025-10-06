using System.Collections;
using UnityEngine;
using TMPro;

public class BouncyBall : MonoBehaviour
{
    public float minY = -5.5f;
    public float maxVelocity = 15f;

    Rigidbody2D rb;

    // Static variabelen die als centraal brein fungeren
    public static int score;

    public static int lives;
    public static int brickCount;

    // Static referenties naar UI en andere objecten
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
        // De eerste bal stelt alle gedeelde (static) referenties in.
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
        // Start is nu veel simpeler. We initialiseren de game state hier niet meer.
        rb = GetComponent<Rigidbody2D>();

        if (rb.linearVelocity.magnitude < 0.1f)
        {
            rb.AddForce(new Vector2(Random.Range(-3f, 3f), 5f), ForceMode2D.Impulse);
        }
    }

    void Update()
    {
        // Als een bal van het scherm valt, vernietig hem. De OnDestroy functie regelt de rest.
        if (transform.position.y < minY)
        {
            Destroy(gameObject);
        }

        // Snelheidslimiet.
        if (rb.linearVelocity.magnitude > maxVelocity)
        {
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxVelocity);
        }
    }

    // Deze functie wordt AANgeroepen net voordat de bal wordt vernietigd.
    void OnDestroy()
    {
        // Zoek hoeveel ballen er nog over zijn.
        BouncyBall[] ballsInScene = FindObjectsByType<BouncyBall>(FindObjectsSortMode.None);

        // Als dit de laatste bal was (of op het punt staat te zijn)...
        if (ballsInScene.Length <= 1)
        {
            // ...en het spel is nog bezig...
            if (Time.timeScale > 0 && lives > 0)
            {
                // ...verlies dan een leven.
                LoseLife();
            }
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
            // Maak een nieuwe bal aan in het midden van het veld.
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