using UnityEngine;

public class Computer : MonoBehaviour
{
    public float moveSpeed;

    public Transform ballTransform;

    public Rigidbody2D rb;

    public float minY;
    public float maxY;

    private Vector2 moveDirection;

    void Update()
    {
        if (ballTransform == null)
        {
            moveDirection = Vector2.zero;
            return;
        }

        if (ballTransform.position.y > transform.position.y + 0.2f)
        {
            moveDirection = Vector2.up;
        }
        else if (ballTransform.position.y < transform.position.y - 0.2f)
        {
            moveDirection = Vector2.down;
        }
        else
        {
            moveDirection = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        rb.MovePosition(newPosition);
    }
}