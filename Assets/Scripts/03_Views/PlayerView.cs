using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
/*
[RequireComponent]��?
�� ��ũ��Ʈ�� �پ� �ִ� GameObject����
�ݵ�� Ư�� ������Ʈ�� �Բ� �پ� �־�� �Ѵٴ� ���� Unity���� �˷��ִ� ���
��, �� PlayerView ��ũ��Ʈ�� GameObject�� ���̸�,
�ش� ������Ʈ���� ������ Rigidbody2D�� Animator�� ���� �ٴ´�.
= �����Ϳ��� �Ǽ����� �ʰ� + NullReference�� ���ϱ�����
*/

public class PlayerView : MonoBehaviour
{
    //������ٵ� (������ �̵�, �ӵ�, ����) ������Ʈ
    private Rigidbody2D _rigidbody;
    //�ִϸ��̼� ������ ���� ������Ʈ
    private Animator animator;
    

    // �����̵�� ũ�� ������ ���
    [SerializeField] private Transform spriteTransform;
    //�����̵��� �� �پ�� Y ���� (�⺻ ������ �������� ���̰ڴٴ� �ǹ�)
    [SerializeField] private float slideScaleY = 0.5f;

    //�÷��̾��� �⺻ Y ũ�⸦ �����صδ� ���� (�����̵� �� ������ �� ���)
    private float defaultScaleY;

    private void Awake()
    {
        //������Ʈ�� ���� ������ٵ� + �ִϸ����� ������
        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //SpriteTransform�� �ִٸ�
        if (spriteTransform != null)
            //���� Y ũ�⸦ ���� = �����̵� �� ���� ũ��� �ǵ��� �� ���
            defaultScaleY = spriteTransform.localScale.y;
    }

    //�ڵ� �̵� ó��
    public void Move(float speed)
    {
        //x������ Speed��ŭ �̵���Ŵ (y���� �ӵ� ����)
        _rigidbody.velocity = new Vector2(speed, _rigidbody.velocity.y);

        //�ִϸ����Ϳ��� �ӵ� ����
        animator.SetFloat("Speed", speed);
    }

    //���� ó��
    public void Jump(float jumpForce)
    {
        //�̹��� �ݴ�� y������ ���� ���� ���� (x���� �ӵ� ����)
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
        //�ִϸ����Ϳ��� Jump��� Ʈ���� ���� + ���
        animator.SetTrigger("Jump"); //�ִϸ��̼� ���� �� �ּ�ó�� ����
    }

    //���� �ߴ����� ���� �ִϸ��̼� ���� �޼���
    public void SetGrounded(bool value)
    {
        animator.SetBool("isGrounded", value);
    }

    //�����̵� ���� ó��
    public void Slide()
    {
        //SpriteTransform�� �ִٸ�
        if (spriteTransform != null)
        {
            //�ð������� ���� ���߱� ���� Sprite�� Y �������� ����
            spriteTransform.localScale = new Vector3(1f, slideScaleY, 1f);
        }
        //Animator���� IsSliding�� true�� ����
        //animator.SetBool("IsSliding", true); //�ִϸ��̼� ���� �� �ּ�ó�� ����
    }

    //�����̵� ���� ó��
    public void EndSlide()
    {
        //SpriteTransform�� �ִٸ�
        if (spriteTransform != null)
        {
            //�����̵� ���� �� ���� ũ��� �ǵ���
            spriteTransform.localScale = new Vector3(1f, defaultScaleY, 1f);
        }
        //IsSliding �ִϸ��̼� Bool�� ���� �ִϸ��̼� ����
        //animator.SetBool("IsSliding", false); //�ִϸ��̼� ���� �� �ּ�ó�� ����
    }

    //��� �ִϸ��̼� ���
    public void PlayDeathAnimation()
    {
        //�׾��� �� ����Ǵ� �ִϸ��̼� Ʈ���� "Die"�� Animator���� ����
        //���ε� �Ѿ���, ����, ��Ʋ�Ÿ� �� ������ �ִϸ��̼� ���
        //animator.SetTrigger("Die");
    }
}
