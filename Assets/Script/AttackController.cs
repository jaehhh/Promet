using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    // �� ������Ʈ
    private Rigidbody2D rigid;
    private MoveController moveController;
    private Animator animator;

    // Ű
    private KeyCode meleeKey;
    private KeyCode shootKey;

    // ��ٿ�
    private float meleeCooldown = 2f;
    private float shootCooldown = 1f;
    private float meleeCurrentCooldown;
    private float shootCurrentCooldown;

    private Vector2 tempMeleeAttackStartPositionInViewport;
    private float tempSpeedValue;
    private float tempGravity;

    [HideInInspector]
    public bool isDash;
    [HideInInspector]
    public bool recoveringSpeed;

    [SerializeField]
    private GameObject projectile;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        moveController = GetComponent<MoveController>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        meleeKey = OptionSetting.meleeKey;
        shootKey = OptionSetting.shootKey;

        Debug.Log($"�������� : {meleeKey}, ���Ÿ����� : {shootKey}");
    }

    private void Update()
    {
        if(meleeCurrentCooldown > 0)
        meleeCurrentCooldown -= Time.deltaTime;
        if(shootCurrentCooldown > 0)
        shootCurrentCooldown -= Time.deltaTime;

        // �ٰŸ�����
        if (Input.GetKeyDown(meleeKey))
        {
            if (meleeCurrentCooldown > 0) return;
            if (isDash) return;
            if (moveController.runFast) return;
            if (moveController.invinsible) return;

            animator.SetTrigger("meleeAttack");
            meleeCurrentCooldown = meleeCooldown;
        }

        // ���Ÿ� ����
        if (Input.GetKeyDown(shootKey))
        {
            if (shootCurrentCooldown > 0) return;
            if (isDash) return;
            if (moveController.invinsible) return;

            animator.SetTrigger("shootAttack");
            GameObject clone = Instantiate(projectile, transform.position + Vector3.right * 2f + Vector3.up * -0.35f, Quaternion.identity);
            clone.GetComponent<Projectile>().speed = moveController.RunSpeed * 2;

            shootCurrentCooldown = shootCooldown;
        }
    }

    // �������� �ִϸ��̼ǿ��� ȣ���ϴ� �Լ�
    public void MeleeAttackDash()
    {
        isDash = true;

        // ī�޶� �� ĳ���� ��ġ ���
        tempMeleeAttackStartPositionInViewport = Camera.main.WorldToViewportPoint(transform.position);
        // ī�޶� ��ŷ "����->�и�"�� ����
        Camera.main.GetComponent<CameraMove>().SeparateMove();

        tempSpeedValue = moveController.speed;
        moveController.speed = moveController.RunSpeed * 3.2f;

        rigid.velocity = new Vector2(rigid.velocity.x, 0);
        tempGravity = rigid.gravityScale;
        rigid.gravityScale = 0;
    }

    // �������� �ִϸ��̼ǿ��� ȣ���ϴ� �Լ�
    public void MeleeAttackDone()
    {
        isDash = false;

        moveController.speed = moveController.RunSpeed * 0.1f;
        rigid.gravityScale = tempGravity;

        StartCoroutine("MeleeAttack_RecoverySpeed");
    }

    // �������� �� �����ڸ����� ���ƿ��� ���� ��ġ üũ �� ī�޶� ��ŷ "�и�->����"���� ����
    private IEnumerator MeleeAttack_RecoverySpeed()
    {
        recoveringSpeed = true;

        float startX = tempMeleeAttackStartPositionInViewport.x; 

        while (true)
        {
            Vector3 tempVector3 = Camera.main.WorldToViewportPoint(transform.position);

            if (tempVector3.x <= startX) // ���� �÷��̾��� ī�޶�� x��ġ <= �뽬 ������ �÷��̾��� ī�޶�� x��ġ
            {
                transform.position = new Vector2(Camera.main.ViewportToWorldPoint(tempMeleeAttackStartPositionInViewport).x, transform.position.y);

                moveController.speed = moveController.RunSpeed;

                recoveringSpeed = false;

                Camera.main.GetComponent<CameraMove>().FollowingTarget();

                moveController.accelerateSpeed = 0f; // �뽬 ���ӽ��ǵ� �ʱ�ȭ

                yield break;
            }

            yield return null;
        }
    }
}
