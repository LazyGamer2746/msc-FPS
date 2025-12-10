using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed = 6.0f;
    public float Damage = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10);
        GetComponent<Rigidbody>().velocity = transform.forward * Speed;


    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.forward * Speed * Time.deltaTime;
    }


    void OnCollisionEnter(Collision collision)
    {

        Debug.Log($"Hit Object {collision.gameObject.transform.name}");
        Destroy(gameObject);
    }
}
