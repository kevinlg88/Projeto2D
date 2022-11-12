using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour {

    public float speed = 30.0f;
    private bool facingRight = false;
    public bool isDead = false;

    public Image healthBar;

    public GameObject player;
    private Meca playerScript;

    public GameObject boundsLeft;
    public GameObject boundsRight;
    public GameObject bullet;

    private Animator anim;

    private BossStates currentState = BossStates.Idle;

    public enum BossStates
    {
        Idle,
        Moving,
        Shooting,
        Dashing,
        Dead
    }

	void Start () {
        healthBar.fillAmount = 1;

        anim = GetComponent<Animator>();
        playerScript = player.GetComponent<Meca>();
        StartCoroutine(Walk());
        InvokeRepeating("ShootTrigger",3,1);
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Bullet(Clone)")
        {
            healthBar.fillAmount -= 0.1f;
            if (healthBar.fillAmount > 0)
            {
                ScoreManager.score += 10;
                StartCoroutine("CreateHitEffect");
            }
            else
            {
                CancelInvoke();
                StopAllCoroutines();
                StartCoroutine("BossDefeated");
            }

            Destroy(collision.gameObject);
        }
    }

    private void ShootTrigger()
    {
        StartCoroutine(Shoot());
    }
    IEnumerator Shoot()
    {
        GameObject bulletClone;
        bulletClone = (Instantiate(bullet, transform.position, transform.rotation)) as GameObject;

        if (facingRight)
            bulletClone.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * 10;
        else
            bulletClone.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0) * 10;

        anim.Play("BossShoot");
        yield return new WaitForSeconds(0f);
    }

    IEnumerator CreateHitEffect()
    {
        for (int n = 0; n < 5; n++)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(.1f);
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(.1f);
        }

        GetComponent<SpriteRenderer>().enabled = true;
    }

    IEnumerator BossDefeated()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Debug.Log("Boss Defeated");
        currentState = BossStates.Dead;
        GetComponent<BoxCollider2D>().enabled = false;
        anim.Play("BossDead");
        yield return new WaitForSeconds(1.5f);
        isDead = true;
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        yield return null;
    }

    void Flip()
    {
        facingRight = !(facingRight);
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    IEnumerator Walk()
    {
        //Anda para esquerda
        GetComponent<Animator>().Play("BossWalk");
        GetComponent<Rigidbody2D>().velocity = Vector2.left*5f;

        yield return new WaitForSeconds(2f);

        //Para
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Animator>().Play("BossIdle");

        yield return new WaitForSeconds(2f);

        //Anda para direita
        Flip();
        GetComponent<Animator>().Play("BossWalk");
        GetComponent<Rigidbody2D>().velocity = Vector2.right*5f;

        yield return new WaitForSeconds(2f);

        //Para
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Animator>().Play("BossIdle");

        yield return new WaitForSeconds(2f);
        Flip();
        StartCoroutine(Walk());
    }
}
