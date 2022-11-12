using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limit : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<Meca>().isDefeated = true;   
    }
}
