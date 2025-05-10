using UnityEngine;

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
    [SerializeField] private float jumpForce = 8f;
    //�����̵� ���� �ð�
    [SerializeField] private float slideDuration = 1f;

    private bool isJumping = false; //������ �ߴ°�?
    private bool isDoubleJump = false; //���� ������ �ߴ°�?

    //�����̵��� �ߴ°�?
    private bool isSliding = false;
    //���� �����̵� �ð� ����� ����
    private float slideTimer = 0f;
    public bool IsInvincible { get; private set; }

    private void Start()
    {
        // �÷��̾� �並 ������
        playerView = GetComponent<PlayerView>();

        // �� ���� �� �ʱ�ȭ (ü�°� �ӵ��� ����)
        model = new PlayerModel(initialHealth, initialSpeed);
    }

    private void Update()
    {
        //Ű �Է� ó�� Ȯ��
        HandleInput();
        //�����̵� ���� �ð� ó�� Ȯ��
        SlideTime();
    }

    //�ڵ� �̵��� ���� FixedUpdate
    private void FixedUpdate()
    {
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

        //ü�°��� �׽�Ʈ
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1); //ü�� ���� �׽�Ʈ
        }
        //���� ���� �׽�Ʈ
        if (Input.GetKeyDown(KeyCode.S))
        {
            AddScore(36); //���� ���� �׽�Ʈ
        }
    }

    //���� ����
    private void Jump()
    {
        if (isJumping)
        {
            playerView.Jump(jumpForce); //���� �ִϸ��̼� ��û
            //playerView.Jump(jumpForce); //�ִϸ��̼� ���� �� �ּ�ó�� ����
            // �信�� ���� �ִϸ��̼�/���� ���� ���
            isJumping = false; //���� ���� ����
            isDoubleJump = true; //���� ���� ����
        }
        else if (isDoubleJump)
        {
            playerView.Jump(jumpForce); //���� ���� �ִϸ��̼� ��û
            //playerView.Jump(jumpForce); //�ִϸ��̼� ���� �� �ּ�ó�� ����
            isDoubleJump = false; //���� ���� ���� ����
        }
        //else
        //{
        //    // ���� �Ұ� ����
        //}
    }

    //�����̵� ����
    private void Slide()
    {
        if (isSliding)
        {
            //������ �ð���ŭ �����̵� �ð� ����
            slideTimer -= Time.deltaTime;
            //�����̵� �ð��� ������
            if (slideTimer <= 0)
            {
                //�����̵� false
                isSliding = false;
                //�信�� ���� �ִϸ��̼� ��û
                //playerView.EndSlide(); //�ִϸ��̼� ���� �� �ּ�ó�� ����
            }
        }
    }

    //�����̵�
    private void SlideTime()
    {
        //�����̵� ���� �ƴ� ��
        if (!isSliding)
        {
            //�����̵� true
            isSliding = true;
            //�����̵� Ÿ�̸� ����
            slideTimer = slideDuration;
            //�信�� �ִϸ��̼� ��û
            //playerView.Slide(); //�ִϸ��̼� ���� �� �ּ�ó�� ����
        }
    }

    // �������� ���� ���
    public void TakeDamage(int damage)
    {

        //�������� �޾��� �� ü�� ����
        model.TakeDamage(damage);

        //ü���� 0 �����̸�
        if (model.CurrentHealth <= 0)
        {
            Die(); //���
        }
    }

    //���ó�� �޼���
    private void Die()
    {
        //�׾��� �� �ִϸ��̼� ����
        //playerView.PlayDeathAnimation(); //�ִϸ��̼� ���� �� �ּ�ó�� ����
        //View�� �״� ���� + ���ӸŴ����� ���º�ȭ
        GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
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
