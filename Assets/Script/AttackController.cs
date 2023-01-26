using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    // 내 컴포넌트
    private Rigidbody2D rigid;
    private MoveController moveController;
    private Animator animator;

    // 키
    private KeyCode meleeKey;
    private KeyCode shootKey;

    // 쿨다운
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

        Debug.Log($"근접공격 : {meleeKey}, 원거리공격 : {shootKey}");
    }

    private void Update()
    {
        if(meleeCurrentCooldown > 0)
        meleeCurrentCooldown -= Time.deltaTime;
        if(shootCurrentCooldown > 0)
        shootCurrentCooldown -= Time.deltaTime;

        // 근거리공격
        if (Input.GetKeyDown(meleeKey))
        {
            if (meleeCurrentCooldown > 0) return;
            if (isDash) return;
            if (moveController.runFast) return;
            if (moveController.invinsible) return;

            animator.SetTrigger("meleeAttack");
            meleeCurrentCooldown = meleeCooldown;
        }

        // 원거리 공격
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

    // 급접공격 애니메이션에서 호출하는 함수
    public void MeleeAttackDash()
    {
        isDash = true;

        // 카메라 상 캐릭터 위치 기억
        tempMeleeAttackStartPositionInViewport = Camera.main.WorldToViewportPoint(transform.position);
        // 카메라 워킹 "따라감->분리"로 변경
        Camera.main.GetComponent<CameraMove>().SeparateMove();

        tempSpeedValue = moveController.speed;
        moveController.speed = moveController.RunSpeed * 3.2f;

        rigid.velocity = new Vector2(rigid.velocity.x, 0);
        tempGravity = rigid.gravityScale;
        rigid.gravityScale = 0;
    }

    // 급접공격 애니메이션에서 호출하는 함수
    public void MeleeAttackDone()
    {
        isDash = false;

        moveController.speed = moveController.RunSpeed * 0.1f;
        rigid.gravityScale = tempGravity;

        StartCoroutine("MeleeAttack_RecoverySpeed");
    }

    // 근접공격 후 원래자리까지 돌아오기 위해 위치 체크 후 카메라 워킹 "분리->따라감"으로 변경
    private IEnumerator MeleeAttack_RecoverySpeed()
    {
        recoveringSpeed = true;

        float startX = tempMeleeAttackStartPositionInViewport.x; 

        while (true)
        {
            Vector3 tempVector3 = Camera.main.WorldToViewportPoint(transform.position);

            if (tempVector3.x <= startX) // 현재 플레이어의 카메라상 x위치 <= 대쉬 시작한 플레이어의 카메라상 x위치
            {
                transform.position = new Vector2(Camera.main.ViewportToWorldPoint(tempMeleeAttackStartPositionInViewport).x, transform.position.y);

                moveController.speed = moveController.RunSpeed;

                recoveringSpeed = false;

                Camera.main.GetComponent<CameraMove>().FollowingTarget();

                moveController.accelerateSpeed = 0f; // 대쉬 가속스피드 초기화

                yield break;
            }

            yield return null;
        }
    }
}
