using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveController : MonoBehaviour
{
    // 내 컴포넌트
    private Rigidbody2D myRigid;
    private Animator anim;
    [SerializeField]
    private Animator effectAnim;
    private AttackController attackController;
    private PlayerFoot playerFoot;

    private KeyCode jumpKey;
    private KeyCode walkKey;

    // 타 오브젝트 참조
    [SerializeField]
    private Slider sliderWalk;
    [HideInInspector]
    public PlayerHealthUI playerHealthUI; // 타 오브젝트에서 초기화
    private CameraMove cameraMove;
    [SerializeField]
    private Background[] backgrounds; // 죽을 때 배경 멈추도록

    // 이동
    private float runSpeed = 15f;
    public float RunSpeed
    {
        get { return runSpeed; }
        set { runSpeed = value; }
    }
    [HideInInspector]
    public float speed;
    private float addSpeed = 1; // 스피드 가산치
    public float AddSpeed
    {
        get { return addSpeed; }
    } 
    private bool isRunning = true; // 뛰는지 걷는지
    public bool runFast; // 전력질주
    private float walkMaxTime = 2f; // 최대 걷기 가능 시간
    private float walkRemainTime; // 걷기 남은 시간(남은 게이지)
    private float walkCurrentTime; // 걷기 조작 유지 시간
    private float walkCoolTime = 1f;
    private IEnumerator walkCor;
    [HideInInspector]
    public float accelerateSpeed;
    private float dashAccelerate = 5f;
    [HideInInspector]
    public bool hitPillar; // 기둥에 닿았는가
    private float pillarCameraleaveSpeed = 0.75f; // 기둥에 닿을 때 카메라 떠나는 초당 속도;

    // 점프
    private int jumpMaxTime = 2;
    private int jumpCurrentTime;
    [HideInInspector]
    public bool isJump = false; // 점프중 상태 체크
    [HideInInspector] 
    public bool doJump = false; // 점프 버튼을 통해 점프했는가 아니면 땅 종료로 떨어지는 상태인가
    [HideInInspector] 
    public int grounds; // 접촉중인 땅 개수, 맵스폰에 의해 연결된 땅의 연결부위에서 exit, enter 중첩발생

    // 체력
    [HideInInspector]
    public int maxHealth = 3;
    private int currentHealth;
    [HideInInspector]
    public bool invinsible;
    private float invinsibleTime = 0.8f;
    private bool hitting; // 피격시 이동속도 감소
    private float hittingTime = 0.2f;
    public bool isDie;
    public bool isGameRunning;

    private void Awake()
    {
        myRigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        attackController = GetComponent<AttackController>();
        playerFoot = GetComponentInChildren<PlayerFoot>();
        cameraMove = Camera.main.GetComponent<CameraMove>();

        speed = runSpeed;
        walkRemainTime = walkMaxTime;
        currentHealth = maxHealth;
    }

    private void Start()
    {
        jumpKey = OptionSetting.jumpKey;
        walkKey = OptionSetting.walkKey;

        Debug.Log($"점프버튼 : {jumpKey}, 걷기버튼 : {walkKey}");
    }

    private void Update()
    {
        if (isDie) return;
        if (!isGameRunning)
        {
            // return;
        }
        

        // 스피드값 조정
        addSpeed = 1f;
        if (!isRunning) addSpeed *= 0.6f; // 걷기 속도
        if (hitting) addSpeed *= 0.5f; // 피격시 이동 속도
        if (attackController.isDash)
        {
            if(accelerateSpeed < 1f)
            {
                accelerateSpeed += Time.deltaTime * dashAccelerate;
            }
            else
            {
                accelerateSpeed = 1f;
            }
            addSpeed = accelerateSpeed; // 대쉬면 걷기 및 피격 속도 영향 X
        }    
        if (hitPillar)
        {
            addSpeed = 0f; // 기둥접촉시 이동 불가

            if(!attackController.recoveringSpeed && !attackController.isDash) // 대쉬나 대쉬리커버리중에는 카메라 addX변경 없음
            cameraMove.distanceX += Time.deltaTime * pillarCameraleaveSpeed;
        }


        // 전진
        float moveDisX = transform.position.x + speed * addSpeed * Time.deltaTime;
        transform.position = new Vector2(moveDisX, transform.position.y);


        // 점프
        if (Input.GetKeyDown(jumpKey))
        {
            if (jumpCurrentTime >= jumpMaxTime) return;
            if (attackController.isDash) return;

           if(isJump) playerFoot.Jump(); // 아래에서 위쪽 플랫폼으로 점프하기 위한 코드

            ++jumpCurrentTime;
            anim.SetInteger("jumpTime", jumpCurrentTime);

            //anim.SetBool("doJump", true);
            anim.SetTrigger("jump");

            myRigid.velocity = Vector2.zero;
            myRigid.AddForce(Vector2.up * 730f);
        }

        // 걷기
        if (Input.GetKeyDown(walkKey))
        {
            if (isRunning == false) return;
            if (runFast) return;
            if (invinsible) return;

            walkCurrentTime = Time.time;

            isRunning = false;
            anim.SetFloat("runSpeed", 0.6f);

            effectAnim.SetTrigger("walk");

            // 걷기 게이지 코루틴
            if (walkCor != null)
                StopCoroutine(walkCor);
            walkCor = ProgressWalkTime(-1);
            StartCoroutine(walkCor);
        }
        // 걷기 종료 = 달리기 or 전력질주
        else if (Input.GetKeyUp(walkKey))
        {
            if (isRunning) return;

            isRunning = true;
            anim.SetFloat("runSpeed", 1f);

            // 전력질주
            if (walkCurrentTime != 0 && !attackController.isDash && !attackController.recoveringSpeed)
            {
                walkCurrentTime = Time.time - walkCurrentTime;

                if (walkCurrentTime >= walkMaxTime / 3)
                {
                    anim.SetFloat("runSpeed", 1.5f);
                    speed = runSpeed * 1.8f;

                    runFast = true;
                    effectAnim.SetTrigger("runFast");

                    walkCurrentTime = 0;

                    StartCoroutine("RunFast");
                }
                else effectAnim.SetTrigger("default");
            }
            else effectAnim.SetTrigger("default");

            // 걷기 게이지 코루틴
            if (walkCor != null)
                StopCoroutine(walkCor);
            walkCor = ProgressWalkTime(1);
            StartCoroutine(walkCor);
        }
    }

    private IEnumerator ProgressWalkTime(int value)
    {
        if(value >= 1)
        yield return new WaitForSeconds(walkCoolTime);

        while(true)
        {
            walkRemainTime += value * Time.deltaTime;

            sliderWalk.value = walkRemainTime / walkMaxTime;

            yield return new WaitForSeconds(Time.deltaTime);

            if (sliderWalk.value >= 1)
            {
                walkRemainTime = walkMaxTime;

                break;
            }
            else if ( sliderWalk.value <= 0)
            {
                walkRemainTime = 0;

                break;
            }
        }
    }

    private IEnumerator RunFast()
    {
        yield return new WaitForSeconds(0.5f);

        anim.SetFloat("runSpeed", 1f);
        effectAnim.SetTrigger("default");

        speed = runSpeed;

        runFast = false;
    }

    // 땅 체크하는 PlayerFoot 스크립트에서 호출
    public void SetIsJump(bool value)
    {
        if(value == true) // 점프
        {
            anim.SetBool("isJump", true);
        }
        else
        {
            jumpCurrentTime = 0;
            anim.SetInteger("jumpTime", 0);
            anim.SetBool("isJump", false);
        }
    }

    // 장애물로부터 호출되는 피격 함수, 리턴값 : 피격되었는지
    public bool PlayerHit(int damage = 1)
    {
        if (invinsible) return false;
        if (runFast) return false;
        if (isDie) return false;

        if (currentHealth < damage)
            damage = currentHealth;

        // currentHealth -= damage; // 디버깅

        if (playerHealthUI != null)
        {
            for(int i = 0; i < damage; ++i)
            {
                playerHealthUI.DecreaseHealth();
            }
        }

        anim.SetTrigger("hit");
        effectAnim.SetTrigger("hit");

        if (currentHealth == 0)
        {
            PlayerDie();
        }

        else
        {
            hitting = true;
            invinsible = true;
            Color color = GetComponent<SpriteRenderer>().color;
            color.a = 0.5f;
            GetComponent<SpriteRenderer>().color = color;
            Invoke("HittingChange", hittingTime);
            Invoke("InvinsibleChange", invinsibleTime);
        }

        // 뛰고 있을 때 아무 것도 안함
        if (isRunning)
        {

        }
        else // 걷고 있을때 걷기 취소
        {
            isRunning = true;
            anim.SetFloat("runSpeed", 1f);
            effectAnim.SetTrigger("default");
            // 걷기 게이지 코루틴
            if (walkCor != null)
                StopCoroutine(walkCor);
            walkCor = ProgressWalkTime(1);
            StartCoroutine(walkCor);
        }

        return true;
    }

    private void InvinsibleChange()
    {
        invinsible = false;
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
    }

    private void HittingChange()
    {
        hitting = false;
    }

    public void PlayerHeal()
    {
        if(currentHealth < maxHealth)
        ++currentHealth;

        if (playerHealthUI != null)
            playerHealthUI.IncreaseHealth();
    }

    public void PlayerDie()
    {
        isDie = true;
        anim.SetBool("isDie", true);
        anim.SetTrigger("hit");
        effectAnim.SetTrigger("default");
        cameraMove.CameraStop();
   
        for(int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].StopBackgroundMove();
        }

        FindObjectOfType<MainGameManager>().GameOver();
    }
}
