using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 2f;

    Rigidbody2D rb;
    Animator animator;

    Vector2 input;    // キー入力（-1,0,1）
    Vector2 move;     // 斜めでも速さ一定にするための正規化結果
    Vector2 lastMove; // 最後に向いていた方向（idleの向き）

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.gravityScale = 0f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        // キーボード入力（矢印キー/WASD対応）
        input.x = Input.GetAxisRaw("Horizontal"); // -1 or 0 or 1
        input.y = Input.GetAxisRaw("Vertical");   // -1 or 0 or 1

        // 斜めで速くならないよう正規化
        move = input.normalized;

        // 移動中かどうか
        bool isMoving = input.sqrMagnitude > 0.0f;

        // 最後に向いていた方向を記録（idleで使う）
        if (isMoving)
        {
            // どっちの成分が大きいかで優先方向を決める（上下左右のブレ防止）
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                lastMove = new Vector2(Mathf.Sign(input.x), 0f);
            else
                lastMove = new Vector2(0f, Mathf.Sign(input.y));
        }

        // Animatorへ送る
        animator.SetFloat("moveX", input.x);
        animator.SetFloat("moveY", input.y);
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("lastMoveX", lastMove.x);
        animator.SetFloat("lastMoveY", lastMove.y);
    }

    void FixedUpdate()
    {
        // 実際の移動
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }
}
