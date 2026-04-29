using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    [Header("UI & Audio Settings")]
    public GameObject gameOverPanel; 
    public AudioSource bgmSource;    
    public AudioClip deathSound; 

    private Rigidbody2D rb;
    private Animator pAni;
    private bool isGrounded;
    private float moveInput;
    private bool isDead = false;
    private bool isGod = false;

    float score;

    private float defaultMoveSpeed;
    private float defaultJumpForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
        score = 0f;
        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
    }

    void Update()
    {
        if (isDead) return; 

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);


        isGrounded = Physics2D.OverlapCircle(groundCheck.position, .2f, groundLayer);

        if (isGod)
        {
            if (moveInput < 0)
                transform.localScale = new Vector3(2, 2, 2);
            else if (moveInput > 0)
                transform.localScale = new Vector3(-2, 2, 2);
        }
        else
        {
            if (moveInput < 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (moveInput > 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void OnMove(InputValue value)
    {
        if (isDead) return;
        Vector2 input = value.Get<Vector2>();
        moveInput = input.x;
    }

    public void OnJump(InputValue value)
    {
        if (isDead || !isGrounded) return;

        if (value.isPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            pAni.SetTrigger("Jump");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("Respawn") || collision.CompareTag("Enemy"))
        {
            if (!isGod)
            {
                Die();
            }
        }

        if (collision.CompareTag("Finish"))
        {
            LeaderBoard.TrySet(SceneManager.GetActiveScene().buildIndex, (int)score);

            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        if (collision.CompareTag("Item"))
        {
            isGod = true;
            Invoke(nameof(ResetGiant), 3f); // 3초 뒤 초기화
            Destroy(collision.gameObject);
            score += 10f;
        }

        if (collision.CompareTag("SpeedItem"))
        {
            moveSpeed = defaultMoveSpeed * 1.5f; // 1.5배 빨라짐
            Invoke(nameof(ResetSpeed), 3f);      // 3초 뒤 초기화
            Destroy(collision.gameObject);
            score += 10f;
        }

        if (collision.CompareTag("JumpItem"))
        {
            jumpForce = defaultJumpForce * 1.3f; // 1.3배 높아짐
            Invoke(nameof(ResetJump), 3f);       // 3초 뒤 초기화
            Destroy(collision.gameObject);
            score += 10f;
        }
    }

    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static; // 물리 연산 정지

        if (bgmSource != null) bgmSource.Stop();
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetGiant()
    {
        isGod = false;
    }

    private void ResetSpeed()
    {
        moveSpeed = defaultMoveSpeed;
    }

    private void ResetJump()
    {
        jumpForce = defaultJumpForce;
    }
}