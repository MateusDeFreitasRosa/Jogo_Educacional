using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cilindros : MonoBehaviour
{

    //Objetos.
    [SerializeField] private GameObject _fullSpeed;
    [SerializeField] private AudioSource _foguete;

    //Atributos
    private float _speed = 2.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        _fullSpeed.SetActive(false);
        StartCoroutine(moreSpeed());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * _speed * Time.deltaTime);
        if (transform.position.x > 15 || transform.position.x < -15)
            Destroy(this.gameObject);
      
    }

    IEnumerator moreSpeed()
    {
        yield return new WaitForSeconds(1.0f);
        _foguete.Play();
        _speed += 4;
        _fullSpeed.SetActive(true);
    }
}
