using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    public float moveSpeed = 2f;

    Rigidbody2D rb;
    Animator animator;

    Vector2 input;    // �L�[���́i-1,0,1�j
    Vector2 move;     // �΂߂ł��������ɂ��邽�߂̐��K������
    Vector2 lastMove; // �Ō�Ɍ����Ă��������iidle�̌����j

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.gravityScale = 0f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        // �L�[�{�[�h���́i���L�[/WASD�Ή��j
        input.x = Input.GetAxisRaw("Horizontal"); // -1 or 0 or 1
        input.y = Input.GetAxisRaw("Vertical");   // -1 or 0 or 1

        // �΂߂ő����Ȃ�Ȃ��悤���K��
        move = input.normalized;

        // �ړ������ǂ���
        bool isMoving = input.sqrMagnitude > 0.0f;

        // �Ō�Ɍ����Ă����������L�^�iidle�Ŏg���j
        if (isMoving)
        {
            // �ǂ����̐������傫�����ŗD����������߂�i�㉺���E�̃u���h�~�j
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                lastMove = new Vector2(Mathf.Sign(input.x), 0f);
            else
                lastMove = new Vector2(0f, Mathf.Sign(input.y));
        }

        // Animator�֑���
        animator.SetFloat("moveX", input.x);
        animator.SetFloat("moveY", input.y);
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("lastMoveX", lastMove.x);
        animator.SetFloat("lastMoveY", lastMove.y);
    }

    void FixedUpdate()
    {
        // ���ۂ̈ړ�
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }
}
