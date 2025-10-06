using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb;
    public float startingSpeed = 5f;
    public float speedIncreaseFactor = 1.1f;

    private float currentSpeed;

    void Start()
    {
        Invoke("ResetAndLaunch", 1f);
    }

    public void ResetAndLaunch()
    {
        transform.position = Vector2.zero;
        currentSpeed = startingSpeed;

        float x = Random.value < 0.5f ? -1f : 1f;

        float y = Random.value < 0.5f ? Random.Range(-1f, -0.2f) : Random.Range(0.2f, 1f);

        Vector2 direction = new Vector2(x, y).normalized;
        rb.linearVelocity = direction * currentSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            currentSpeed *= speedIncreaseFactor;
            rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ComputerGoal"))
        {
            GameManager.instance.PlayerScores();
        }
        else if (other.CompareTag("PlayerGoal"))
        {
            GameManager.instance.ComputerScores();
        }
    }
}