using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float normalJumpForce = 10f;
    public float highJumpForce = 18f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private bool isGrounded;
    private bool isCrouching;
    private float crouchStartTime;

    private Vector3 originalScale;
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;

    // �ݒ肵������{�^��
    private KeyCode moveLeftKey = KeyCode.A;
    private KeyCode moveRightKey = KeyCode.F;
    private KeyCode jumpKey = KeyCode.J;
    private KeyCode crouchKey = KeyCode.L;

    // ����{�^���̃��X�g
    private List<KeyCode> actionKeys;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        originalScale = transform.localScale;

        if (boxCollider != null)
        {
            originalColliderSize = boxCollider.size;
            originalColliderOffset = boxCollider.offset;
        }

        // �����ݒ�̑���{�^�������X�g�Ɋi�[
        actionKeys = new List<KeyCode> { moveLeftKey, moveRightKey, jumpKey, crouchKey };
    }

    void Update()
    {
        // �X�y�[�X�L�[�������ꂽ�Ƃ��ɑ���{�^����ύX
       // if (Input.GetKeyDown(KeyCode.Space))
       // {
       //     ChangeControlButtons();
       // }

        // �n�ʃ`�F�b�N
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // ���E�ړ�
        float move = 0f;
        if (Input.GetKey(moveLeftKey)) move = -1f;
        if (Input.GetKey(moveRightKey)) move = 1f;

        if (!isCrouching)
        {
            rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
        }

        // �W�����v����
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            float jumpForce = normalJumpForce;

            if (isCrouching)
            {
                float crouchDuration = Time.time - crouchStartTime;
                if (crouchDuration >= 1f)
                {
                    jumpForce = highJumpForce;
                }
                isCrouching = false;
                ResetCrouch(); // �k���߂�
            }

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // ���Ⴊ�݊J�n
        if (Input.GetKeyDown(crouchKey))
        {
            isCrouching = true;
            crouchStartTime = Time.time;
            StartCrouch();
        }

        // ���Ⴊ�݉���
        if (Input.GetKeyUp(crouchKey))
        {
            isCrouching = false;
            ResetCrouch();
        }
    }

    void StartCrouch()
    {
        transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);

        if (boxCollider != null)
        {
            boxCollider.size = new Vector2(originalColliderSize.x, originalColliderSize.y * 0.5f);
            boxCollider.offset = new Vector2(originalColliderOffset.x, originalColliderOffset.y - originalColliderSize.y * 0.25f);
        }
    }

    void ResetCrouch()
    {
        transform.localScale = originalScale;

        if (boxCollider != null)
        {
            boxCollider.size = originalColliderSize;
            boxCollider.offset = originalColliderOffset;
        }
    }

    // ����{�^���������_���ɓ���ւ��郁�\�b�h
    public void ChangeControlButtons()
    {
        // �{�^�����X�g�̏��Ԃ��V���b�t��
        Shuffle(actionKeys);

        // ���X�g���烉���_���ɏ��Ԃ�ς����{�^����ݒ�
        moveLeftKey = actionKeys[0];
        moveRightKey = actionKeys[1];
        jumpKey = actionKeys[2];
        crouchKey = actionKeys[3];

        // �V��������{�^�������O�ɕ\��
        Debug.Log($"New Controls: Move Left - {moveLeftKey}, Move Right - {moveRightKey}, Jump - {jumpKey}, Crouch - {crouchKey}");
    }

    // ���X�g���V���b�t�����郁�\�b�h
    private void Shuffle(List<KeyCode> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            KeyCode temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
