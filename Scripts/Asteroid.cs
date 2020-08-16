using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //Partes Asteroid.
    [SerializeField] private GameObject _partIE;
    [SerializeField] private GameObject _partID;
    [SerializeField] private GameObject _partSE;
    [SerializeField] private GameObject _partSD;


    [SerializeField] private float _speed = 0.3f;

    [SerializeField] private GameObject _pequenaExplosao;
    [SerializeField] private GameObject _explosion;
    private GameManager _gameManager;
    [SerializeField]
    private GameObject _explosaoPrefab;
    private bool _eraser;
    public bool eraserAll;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager._gamerStarted == false)
            _eraser = true;
    }





    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.ToString().Contains("IE"))
        {
            movimentAsteroidIE();
        }
        else if (this.gameObject.ToString().Contains("ID"))
        {
            movimentAsteroidID();
        }
        else if (this.gameObject.ToString().Contains("SE"))
        {
            movimentAsteroidSE();
        }
        else if (this.gameObject.ToString().Contains("SD"))
        {
            movimentAsteroidSD();
        }
        else
        {
            movimentAsteroidNormal();
        }

        if (_eraser == true && _gameManager._gamerStarted == true)
            Destroy(this.gameObject);
        if (_gameManager.playerAlive == false && _eraser == false)
            _eraser = true;
    }

    private void movimentAsteroidNormal()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -6)
            Destroy(this.gameObject);
    }

    private void movimentAsteroidIE()
    {
        transform.Translate(new Vector3(-4,-4,0) * _speed * Time.deltaTime);
        if (transform.position.y <= -6)
            Destroy(this.gameObject);
    }
    private void movimentAsteroidID()
    {
        transform.Translate(new Vector3(4, -4, 0) * _speed * Time.deltaTime);
        if (transform.position.y <= -6)
            Destroy(this.gameObject);
    }
    private void movimentAsteroidSE()
    {
        transform.Translate(new Vector3(-4, 4, 0) * _speed * Time.deltaTime);
        if (transform.position.y >= 6)
            Destroy(this.gameObject);
    }
    private void movimentAsteroidSD()
    {
        transform.Translate(new Vector3(4, 4, 0) * _speed * Time.deltaTime);
        if (transform.position.y >= 6)
            Destroy(this.gameObject);
    }


    private void asteroidExplosion()
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Instantiate(_partIE, transform.position, Quaternion.identity);
        Instantiate(_partID, transform.position, Quaternion.identity);
        Instantiate(_partSE, transform.position, Quaternion.identity);
        Instantiate(_partSD, transform.position, Quaternion.identity);
    }

    public void OnTriggerEnter2D(Collider2D other)  
    {
        Debug.Log("Colider: " + other.name);

        if (other.tag == "Player")
        {
            Player collider = other.GetComponent<Player>();
            UIManager uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

            if (collider)
            {

                collider.Explosion_Player();
                uiManager.UpdateLives(0, collider.getControl());
                Destroy(collider.transform.gameObject);
            }
        }
        else if (other.tag == "laser" || other.tag == "laserEnemy" || other.tag == "AtackHorizontal")
        {
            Instantiate(_pequenaExplosao, new Vector3(other.transform.position.x, other.transform.position.y, 0), Quaternion.identity);
            Destroy(other.gameObject);
        }
        else if (other.tag == "ArtilhariaPesada")
        {
            Instantiate(_explosaoPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if (other.tag == "cilindroBomb" && this.gameObject.ToString().Contains("Asteroid"))
        {
            asteroidExplosion();
            Destroy(this.gameObject);
            Destroy(other.gameObject);
        }
        else if (other.tag == "cilindroBomb")
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
        }
        else if (other.tag == "Asteroid" && this.gameObject.ToString().Contains("Asteroid"))
        {
            asteroidExplosion();
            Destroy(this.gameObject);
            Asteroid _and = other.GetComponent<Asteroid>();
            Destroy(other.gameObject);
        }
    }

}
