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
        get //get으로 했기 때문에 읽기 전용 즉, 다른 곳에서 health값을 바꿀 수 없음.
        {
            return currentHealth; //외부에서 가져와서 쓸 수 만 있도록. 해킹때문에 set은 사용 하지 않음. 외부에서 지멋대로 조작할 수 있기 때문
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


        //GetComponent<Transform>().position 이런식으로해서 스크립트에 붙어있는 오브젝트의 컴포넌트를 가져올 수 있다. 
        // 근데 여기서 있는 소문자 position은 위에 있는거를 함축시킨 것 쉽게 쓸 수 있게. 즉, position == GetComponent<Transform>().position 이 외에도 gameobject = GetComponent<GameObject>(), rotation...

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

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);//첫번째 파라미터 (currentHealth)가 두번째 파라미터(0) 부터 세번째 파라미터(maxHealth)까지의 범위 내로 설정
    
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
