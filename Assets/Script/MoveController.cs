using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveController : MonoBehaviour
{
    // �� ������Ʈ
    private Rigidbody2D myRigid;
    private Animator anim;
    [SerializeField]
    private Animator effectAnim;
    private AttackController attackController;
    private PlayerFoot playerFoot;

    private KeyCode jumpKey;
    private KeyCode walkKey;

    // Ÿ ������Ʈ ����
    [SerializeField]
    private Slider sliderWalk;
    [HideInInspector]
    public PlayerHealthUI playerHealthUI; // Ÿ ������Ʈ���� �ʱ�ȭ
    private CameraMove cameraMove;
    [SerializeField]
    private Background[] backgrounds; // ���� �� ��� ���ߵ���

    // �̵�
    private float runSpeed = 15f;
    public float RunSpeed
    {
        get { return runSpeed; }
        set { runSpeed = value; }
    }
    [HideInInspector]
    public float speed;
    private float addSpeed = 1; // ���ǵ� ����ġ
    public float AddSpeed
    {
        get { return addSpeed; }
    } 
    private bool isRunning = true; // �ٴ��� �ȴ���
    public bool runFast; // ��������
    private float walkMaxTime = 2f; // �ִ� �ȱ� ���� �ð�
    private float walkRemainTime; // �ȱ� ���� �ð�(���� ������)
    private float walkCurrentTime; // �ȱ� ���� ���� �ð�
    private float walkCoolTime = 1f;
    private IEnumerator walkCor;
    [HideInInspector]
    public float accelerateSpeed;
    private float dashAccelerate = 5f;
    [HideInInspector]
    public bool hitPillar; // ��տ� ��Ҵ°�
    private float pillarCameraleaveSpeed = 0.75f; // ��տ� ���� �� ī�޶� ������ �ʴ� �ӵ�;

    // ����
    private int jumpMaxTime = 2;
    private int jumpCurrentTime;
    [HideInInspector]
    public bool isJump = false; // ������ ���� üũ
    [HideInInspector] 
    public bool doJump = false; // ���� ��ư�� ���� �����ߴ°� �ƴϸ� �� ����� �������� �����ΰ�
    [HideInInspector] 
    public int grounds; // �������� �� ����, �ʽ����� ���� ����� ���� ����������� exit, enter ��ø�߻�

    // ü��
    [HideInInspector]
    public int maxHealth = 3;
    private int currentHealth;
    [HideInInspector]
    public bool invinsible;
    private float invinsibleTime = 0.8f;
    private bool hitting; // �ǰݽ� �̵��ӵ� ����
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

        Debug.Log($"������ư : {jumpKey}, �ȱ��ư : {walkKey}");
    }

    private void Update()
    {
        if (isDie) return;
        if (!isGameRunning)
        {
            // return;
        }
        

        // ���ǵ尪 ����
        addSpeed = 1f;
        if (!isRunning) addSpeed *= 0.6f; // �ȱ� �ӵ�
        if (hitting) addSpeed *= 0.5f; // �ǰݽ� �̵� �ӵ�
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
            addSpeed = accelerateSpeed; // �뽬�� �ȱ� �� �ǰ� �ӵ� ���� X
        }    
        if (hitPillar)
        {
            addSpeed = 0f; // ������˽� �̵� �Ұ�

            if(!attackController.recoveringSpeed && !attackController.isDash) // �뽬�� �뽬��Ŀ�����߿��� ī�޶� addX���� ����
            cameraMove.distanceX += Time.deltaTime * pillarCameraleaveSpeed;
        }


        // ����
        float moveDisX = transform.position.x + speed * addSpeed * Time.deltaTime;
        transform.position = new Vector2(moveDisX, transform.position.y);


        // ����
        if (Input.GetKeyDown(jumpKey))
        {
            if (jumpCurrentTime >= jumpMaxTime) return;
            if (attackController.isDash) return;

           if(isJump) playerFoot.Jump(); // �Ʒ����� ���� �÷������� �����ϱ� ���� �ڵ�

            ++jumpCurrentTime;
            anim.SetInteger("jumpTime", jumpCurrentTime);

            //anim.SetBool("doJump", true);
            anim.SetTrigger("jump");

            myRigid.velocity = Vector2.zero;
            myRigid.AddForce(Vector2.up * 730f);
        }

        // �ȱ�
        if (Input.GetKeyDown(walkKey))
        {
            if (isRunning == false) return;
            if (runFast) return;
            if (invinsible) return;

            walkCurrentTime = Time.time;

            isRunning = false;
            anim.SetFloat("runSpeed", 0.6f);

            effectAnim.SetTrigger("walk");

            // �ȱ� ������ �ڷ�ƾ
            if (walkCor != null)
                StopCoroutine(walkCor);
            walkCor = ProgressWalkTime(-1);
            StartCoroutine(walkCor);
        }
        // �ȱ� ���� = �޸��� or ��������
        else if (Input.GetKeyUp(walkKey))
        {
            if (isRunning) return;

            isRunning = true;
            anim.SetFloat("runSpeed", 1f);

            // ��������
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

            // �ȱ� ������ �ڷ�ƾ
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

    // �� üũ�ϴ� PlayerFoot ��ũ��Ʈ���� ȣ��
    public void SetIsJump(bool value)
    {
        if(value == true) // ����
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

    // ��ֹ��κ��� ȣ��Ǵ� �ǰ� �Լ�, ���ϰ� : �ǰݵǾ�����
    public bool PlayerHit(int damage = 1)
    {
        if (invinsible) return false;
        if (runFast) return false;
        if (isDie) return false;

        if (currentHealth < damage)
            damage = currentHealth;

        // currentHealth -= damage; // �����

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

        // �ٰ� ���� �� �ƹ� �͵� ����
        if (isRunning)
        {

        }
        else // �Ȱ� ������ �ȱ� ���
        {
            isRunning = true;
            anim.SetFloat("runSpeed", 1f);
            effectAnim.SetTrigger("default");
            // �ȱ� ������ �ڷ�ƾ
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
