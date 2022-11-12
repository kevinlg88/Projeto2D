using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public GameObject bullet;

    public float repeatRate;
    public float jumpForce;

    bool facingRight = false;
    void Start()
    {
        InvokeRepeating("Shoot", 3.0f, repeatRate);
        StartCoroutine(Walk());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Bullet(Clone)")
        {
            ScoreManager.score += 10;
            Destroy(collision.gameObject);
            StartCoroutine("EnemyDeath");
        }
    }

    void Shoot()
    {
        GameObject bulletClone;
        bulletClone = (Instantiate(bullet, transform.position, transform.rotation)) as GameObject;
        if(facingRight)
        bulletClone.GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0) * 10;
        else
        bulletClone.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0) * 10;
    }

    void Jump()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    void Flip()
    {
        facingRight = !(facingRight);
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    IEnumerator EnemyDeath()
    {
        GetComponent<Animator>().Play("Explosion");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        yield return null;
    }

    IEnumerator Walk()
    {
        //Anda para esquerda
        GetComponent<Animator>().Play("Enemy1Walk");
        GetComponent<Rigidbody2D>().velocity = Vector2.left*2f;

        yield return new WaitForSeconds(2f);

        //Para
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Animator>().Play("Enemy1Idle");

        yield return new WaitForSeconds(4f);

        //Anda para direita
        Flip();
        GetComponent<Animator>().Play("Enemy1Walk");
        GetComponent<Rigidbody2D>().velocity = Vector2.right*2f;

        yield return new WaitForSeconds(2f);

        //Para
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Animator>().Play("Enemy1Idle");

        yield return new WaitForSeconds(4f);
        Flip();
        StartCoroutine(Walk());
    }
}
