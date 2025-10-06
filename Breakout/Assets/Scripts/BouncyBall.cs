using UnityEngine;
using TMPro;

public class BouncyBall : MonoBehaviour
{
    public float minY = -5.5f;
    public float maxVelocity = 15f;

    private Rigidbody2D rb;
    private int score = 0;
    private int lives = 5;

    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI powerUpText;
    public GameObject[] livesImage;
    public GameObject gameOverPanel;
    public GameObject youWinPanel;

    private int brickCount;
    public static BouncyBall Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        brickCount = FindFirstObjectByType<LevelGenerator>().transform.childCount;
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
                transform.position = Vector3.zero;
                rb.linearVelocity = new Vector2(Random.Range(-3f, 3f), 5f);
                lives--;
                livesImage[lives].SetActive(false);

                // Reset paddle size wanneer leven verliest
                PlayerMovement paddle = FindFirstObjectByType<PlayerMovement>();
                if (paddle != null)
                    paddle.ResetPaddleSize();
            }
        }

        if (rb.linearVelocity.magnitude > maxVelocity)
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxVelocity);
    }

    public void AddLife()
    {
        if (lives < livesImage.Length)
        {
            livesImage[lives].SetActive(true);
            lives++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Brick")) return;

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

    public void AddRandomScore()
    {
        score += Random.Range(1, 101);
        scoreTxt.text = score.ToString("00000");
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        Destroy(gameObject);
    }
}