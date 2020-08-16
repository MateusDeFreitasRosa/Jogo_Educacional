using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserHorizontalAtack : MonoBehaviour
{
    [SerializeField]
    private float _speed = 7.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Disparo
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        //Destroi Objeto
        if (transform.position.x < -9.5f)
            Destroy(this.gameObject);
    }
}
