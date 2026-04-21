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
    public GameObject gameOverPanel; // 게임오버 UI 패널
    public AudioSource bgmSource;    // 현재 스테이지 BGM 소스
    public AudioClip deathSound;     // 죽었을 때 재생할 음악/효과음

    private Rigidbody2D rb;
    private Animator pAni;
    private bool isGrounded;
    private float moveInput;
    private bool isDead = false;
    private bool isGod = false; // 무적 상태 확인용

    // 아이템 효과가 끝났을 때 되돌아갈 원래 수치
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();

        // 게임 시작 시 인스펙터에 설정된 기본 스탯을 저장해 둡니다.
        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
    }

    void Update()
    {
        if (isDead) return; // 죽었을 때는 아무 행동도 하지 않음

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // 바닥 체크
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, .2f, groundLayer);

        // 거대화(isGod) 상태에 따른 크기 조절 및 좌우 방향 뒤집기
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
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        if (collision.CompareTag("Item"))
        {
            isGod = true;
            Invoke(nameof(ResetGiant), 3f); // 3초 뒤 초기화
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("SpeedItem"))
        {
            moveSpeed = defaultMoveSpeed * 1.5f; // 1.5배 빨라짐
            Invoke(nameof(ResetSpeed), 3f);      // 3초 뒤 초기화
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("JumpItem"))
        {
            jumpForce = defaultJumpForce * 1.3f; // 1.3배 높아짐
            Invoke(nameof(ResetJump), 3f);       // 3초 뒤 초기화
            Destroy(collision.gameObject);
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