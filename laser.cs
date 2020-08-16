using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{
    // Variaveis
    [SerializeField]
    private float _speed = 20.0f;

    // Update is called once per frame
    void Update()
    {
        Disparo();
        Destroi();
    }

    //Movimento Laser
    public void Disparo()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }

    //Destroi objectos que sairam da tela
    public void Destroi()
    {
        if (transform.position.y >= 6)
            Destroy(this.gameObject);
    }

    //Destroi objeto

}
