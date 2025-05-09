using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //�÷��̾� �ִϸ��̼� + ����ó�� ���� ���̸Ӱ� ������ �� �ֵ��� ����
    [SerializeField] private PlayerView playerView;

    //�÷��̾ �ӵ� up, ������ ���� ���� �Ǵ��ϸ� ����ٰ� �ݿ�
    private PlayerModel model;

    //�÷��̾� �ִ� ü��
    //initialHealth�� �ʱⰪ�� [����]�ؼ� [����]�ϴ� ����
    //PlayerModel�� MaxHealth�� �ʱⰪ�� [����]�ϴ� ����
    //�Ѹ���� �ܺο��� �ʱⰪ�� �ֱ����� = ������������ ü���� �ٸ� �� �ִٴ°� ���
    [SerializeField] private int initialHealth = 100;
    //�⺻ �̵� �ӵ�
    [SerializeField] private float initialSpeed = 5f;
    //������ �� ���� �������� ��
    [SerializeField] private float jumpForce = 8f;
    //�����̵� ���� �ð�
    [SerializeField] private float slideDuration = 1f; 

    //�����̵��� �ߴ°�?
    private bool isSliding = false;
    //���� �����̵� �ð� ����� ����
    private float slideTimer = 0f; 

    private void Start()
    {
        // �� ���� �� �ʱ�ȭ (ü�°� �ӵ��� ����)
        model = new PlayerModel(initialHealth, initialSpeed);
    }

    private void Update()
    {
        //Ű �Է� ó�� Ȯ��
        HandleInput();
        //�����̵� ���� �ð� ó�� Ȯ��
        HandleSlide(); 
    }

    //Ű �Է� ó���ϴ� �޼���
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(); //����Ű�� Space
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Slide(); //�����̵� Ű�� Shift
        }
    }

    //����
    private void Jump()
    {
        // �信�� ���� �ִϸ��̼�/���� ���� ���
        //playerView.Jump(jumpForce); //�ִϸ��̼� ���� �� �ּ�ó�� ����
    }

    //�����̵�
    private void Slide()
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

    //�����̵� ����
    private void HandleSlide()
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
}
