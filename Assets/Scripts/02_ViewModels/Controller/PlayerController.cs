using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(PlayerView))]
public class PlayerController : MonoBehaviour
{
    //�÷��̾� �ִϸ��̼� + ����ó�� ���� ���̸Ӱ� ������ �� �ֵ��� ����
    //[SerializeField] ��� �ּ�ó��
    private PlayerView playerView;

    //�÷��̾ �ӵ� up, ������ ���� ���� �Ǵ��ϸ� ����ٰ� �ݿ�
    private PlayerModel model;

    //�÷��̾� �ִ� ü��
    //initialHealth�� �ʱⰪ�� [����]�ؼ� [����]�ϴ� ����
    //PlayerModel�� MaxHealth�� �ʱⰪ�� [����]�ϴ� ����
    //�Ѹ���� �ܺο��� �ʱⰪ�� �ֱ����� = ������������ ü���� �ٸ� �� �ִٴ°� ���
    [Header("Model �ʱⰪ")]
    [SerializeField] private int initialHealth = 6; // ü�� 6ĭ _ryang
    //�⺻ �̵� �ӵ�
    [SerializeField] private float initialSpeed = 5f;

    [Header("Jump/Slide")]
    //������ �� ���� �������� ��
    [SerializeField] private float jumpForce = 13f;
    //�����̵� ���� �ð�
    [SerializeField] private float slideDuration = 1f;

    private bool isGround = true; // ���� �ִ� ����
    private bool isDoubleJump = false; //���� ���� ���� ����

    //�����̵��� �ߴ°�?
    private bool isSliding = false;
    //���� �����̵� �ð� ����� ����
    private float slideTimer = 0f;
    public bool IsInvincible { get; private set; }

    private void Awake() //update���� ���� ����
    {
        // �÷��̾� �並 ������
        playerView = GetComponent<PlayerView>();

        // �� ���� �� �ʱ�ȭ (ü�°� �ӵ��� ����)
        model = new PlayerModel(initialHealth, initialSpeed);
    }

    private void Update()
    {
        //GameOver ���¸� �Է� ����
        if (GameManager.Instance.CurrentState == GameManager.GameState.GameOver)
            return;
        //Ű �Է� ó�� Ȯ��
        HandleInput();
    }

    //�ڵ� �̵��� ���� FixedUpdate
    private void FixedUpdate()
    {
        //GameOver ������ �� �ڵ� �̵� ����
        if (GameManager.Instance.CurrentState == GameManager.GameState.GameOver)
            return;
        // View�� �̵� ��û
        playerView.Move(model.Speed);
    }

    //Ű �Է� ó���ϴ� �޼���
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(); //����Ű�� Space
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isSliding)
        {
            Slide(); //�����̵� Ű�� Shift
        }
        if (isSliding)
        {
            SlideTime(); // �����̵� ���� �ð� ó��
        }

        //ü�°��� �׽�Ʈ
        if (Input.GetKeyDown(KeyCode.H))
        {
            GameManager.Instance.TakeDamage(1); //ü�� ���� �׽�Ʈ
        }
        //���� ���� �׽�Ʈ
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameManager.Instance.AddScore(36); //���� ���� �׽�Ʈ
        }
    }

    //���� ����
    private void Jump()
    {
        if (isGround)
        {
            playerView.Jump(jumpForce); //���� �ִϸ��̼� ��û
            playerView.SetGrounded(false); // ���� ���� ���� ���� ����
            // �信�� ���� �ִϸ��̼�/���� ���� ���
            isGround = false; //����!
            isDoubleJump = true; //���� ���� ����
        }
        else if (isDoubleJump)
        {
            playerView.Jump(jumpForce); //���� ���� �ִϸ��̼� ��û
            isDoubleJump = false; //���� ����!
            playerView.SetGrounded(false); // ���� ���� ���� ���� ���� ����
        }
        else
        {
            Debug.Log("���� ���������� �����մϴ�.");
        }
    }

    //�����̵� ����
    private void Slide()
    {
        Debug.Log("�����̵� ���Դϴ�.");
        isSliding = true; //�����̵� �� ����
        slideTimer = slideDuration; //�����̵� ���� �ð� ����
        playerView.Slide(); //�����̵� �ִϸ��̼� ��û

        //�信�� ���� �ִϸ��̼� ��û
        //playerView.EndSlide(); //�ִϸ��̼� ���� �� �ּ�ó�� ����
    }

    //�����̵�
    private void SlideTime()
    {
        //�����̵� ���� �ð� ó��
        slideTimer -= Time.deltaTime;

        if (slideTimer <= 0f)
        {
            Debug.Log("�����̵� ����");
            isSliding = false;
            playerView.EndSlide(); //�����̵� ���� �ִϸ��̼� ��û
        }
    }

    // ���� ��Ҵ��� Ȯ��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGround = true; //���� ����� ��
            isDoubleJump = false; //���� ���� �ʱ�ȭ
            playerView.SetGrounded(true); //�����ߴ����� PlayerView���� �˸�
        }
    }

    // �������� ���� ���
    public void TakeDamage(int damage)
    {

        //�������� �޾��� �� ü�� ����
        model.TakeDamage(damage);
        int updateHp = model.CurrentHealth;

        UIManager.Instance.UpdateHealth(updateHp); //UI ������Ʈ

        //ü���� 0 �����̸�
        if (model.CurrentHealth == 0)
        {
            Debug.Log("�׾����ϴ�.");
            //�׾��� �� �ִϸ��̼� ����
            //playerView.PlayDeathAnimation(); //�ִϸ��̼� ���� �� �ּ�ó�� ����
            //���ó�� �޼��� ȣ��
            Die();
        }
        else
        {
            Debug.Log("ü�� : " + model.CurrentHealth);
        }
    }

    //���ó�� �޼���
    private void Die()
    {
        //playerView.SetAnimatorSpeed(0f);
        //�׾��� �� �ִϸ��̼� ����
        //playerView.PlayDeathAnimation(); //�ִϸ��̼� ���� �� �ּ�ó�� ����
        //View�� �״� ���� + ���ӸŴ����� ���º�ȭ
        GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
        playerView.StopMovementAnimation();
    }

    // ������ ȹ�� �� �ܺο��� ȣ��
    public void AddScore(int score)
    {
        //�𵨿� ������ ���� 
        model.AddScore(score);
        //���ӸŴ��� AddScore�� ������Ʈ
        GameManager.Instance.AddScore(score);
    }

    //ȸ�������� ����
    public void Heal(int amount)
    {
        //ȸ�� �������� �Ծ��� �� ȸ�� ó��
        model.Heal(amount);
    }

    //�ӵ������� ����
    public void SetSpeed(float newSpeed)
    {
        //�ӵ� �������� �Ծ��� �� ���� �̵� �ӵ� ����
        model.SetSpeed(newSpeed);
    }
    public void SetInvincible(bool value)
    {
        IsInvincible = value;
    }
}
