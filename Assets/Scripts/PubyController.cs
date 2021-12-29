using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PubyController : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 15.0f;
    public int maxHealth = 5;

    public int health 
    {
        get //get���� �߱� ������ �б� ���� ��, �ٸ� ������ health���� �ٲ� �� ����.
        {
            return currentHealth; //�ܺο��� �����ͼ� �� �� �� �ֵ���. ��ŷ������ set�� ��� ���� ����. �ܺο��� ���ڴ�� ������ �� �ֱ� ����
        }  
    }

    int currentHealth;


    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;


    Rigidbody2D rigidbody2d;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;

    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 10;

        rigidbody2d = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }


        //GetComponent<Transform>().position �̷��������ؼ� ��ũ��Ʈ�� �پ��ִ� ������Ʈ�� ������Ʈ�� ������ �� �ִ�. 
        // �ٵ� ���⼭ �ִ� �ҹ��� position�� ���� �ִ°Ÿ� �����Ų �� ���� �� �� �ְ�. ��, position == GetComponent<Transform>().position �� �ܿ��� gameobject = GetComponent<GameObject>(), rotation...

        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        Vector2 _position = rigidbody2d.position;

        _position = _position + move * speed * Time.deltaTime;

        rigidbody2d.MovePosition(_position);

        if(isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }


        //transform.position = _position;
    }

    public void ChangeHealth(int amount)
    {
        if(amount <0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;
            isInvincible = true;
            invincibleTimer = timeInvincible;

        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);//ù��° �Ķ���� (currentHealth)�� �ι�° �Ķ����(0) ���� ����° �Ķ����(maxHealth)������ ���� ���� ����
    
        Debug.Log(currentHealth + "/" + maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }
}
