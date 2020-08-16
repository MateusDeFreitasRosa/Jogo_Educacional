using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class naveLancaCilindro : MonoBehaviour
{
    //Objetos.
    [SerializeField] private GameObject _cilindroPart;
    [SerializeField] private GameObject _cilindroCarry;
    [SerializeField] private GameObject _enemyExplosion;
    [SerializeField] private int _HPEnemy;
    [SerializeField] private GameObject _collider01;
    [SerializeField] private GameObject _collider02;
    [SerializeField] private GameObject _collider03;
    [SerializeField] private GameObject _collider04;
    [SerializeField] private GameObject _collider05;
    [SerializeField] private GameObject _collider06;
    [SerializeField] private GameObject _fullSpeed01;
    [SerializeField] private GameObject _fullSpeed02;
    [SerializeField] private GameObject _pequenaExplosao;

    //Atributos.
    private float _speed = 0.4f;
    private bool _eraser;
    private bool _canFire = true;


    //Instancias;
    private UIManager _uiManager;
    private Player _player;
    private GameObject _findPlayer;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _findPlayer = GameObject.Find("Player(Clone)");
        if (_findPlayer)
        {
            _player = _findPlayer.GetComponent<Player>();
        }
        Init();
    }

    private void Init()
    {
        _collider01.SetActive(false);
        _collider02.SetActive(false);
        _collider03.SetActive(false);
        _collider04.SetActive(false);
        _collider05.SetActive(false);
        _collider06.SetActive(false);
        _fullSpeed01.SetActive(false);
        _fullSpeed02.SetActive(false);
        _HPEnemy = 12;
    }

    // Update is called once per frame
    void Update()
    {
        Moviment();
        if (_player)
            SearchPlayer();

        if (_gameManager._gamerStarted == true && _eraser == true)
        {
            Destroy(this.gameObject);
        }
        if (_gameManager.playerAlive == false && _eraser == false)
            _eraser = true;

    }

    private void shoot()
    {
        Instantiate(_cilindroPart, transform.position + new Vector3(-.61f, -1, 0), Quaternion.identity);
        Instantiate(_cilindroPart, transform.position + new Vector3(.61f, -1, 0), Quaternion.Euler(0,0,180));
        
        _canFire = false;
        _cilindroCarry.SetActive(false);
        _fullSpeed01.SetActive(true);
        _fullSpeed02.SetActive(true);
        _speed += 2;
    }

    private void SearchPlayer()
    {
        if (_player.transform.position.y <= (transform.position.y-1) + .5 && _player.transform.position.y >= (transform.position.y - 1) - .5)
        {
            if (_canFire)
                shoot();
        }
    }

    private void fullSpeed()
    {
        _speed = 1.0f;
        _fullSpeed01.SetActive(true);
        _fullSpeed02.SetActive(false);
    }

    public void Moviment()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //Se ele sair da tela, para reaproveitarmos o inimigo, ele volta para cima na tela.
        if (transform.position.y <= -7 || transform.position.x > 10 || transform.position.x < -10)
        {
            Destroy(this.gameObject);
        }
    }

    //Instanciação do Objeto (Enemy_Explosion)
    public void Explosion_Enemy()
    {
        if (_canFire)
        {
            shoot();
        }
        Instantiate(_enemyExplosion, transform.position, Quaternion.identity);
    }


    //Colisão do laser com o inimigo
    public void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Object colided with: " + other);
        if (other.tag == "laser")
        {
            //uiManager.UpdateScore(2);
            _HPEnemy--;
            // Instancia pequena explosao
            Instantiate(_pequenaExplosao, new Vector3(other.transform.position.x, other.transform.position.y + .4f, 0), Quaternion.identity);

            if (_HPEnemy == 11)
            {
                _collider01.SetActive(true);
            }
            else if (_HPEnemy == 9)
            {
                _collider02.SetActive(true);
            }
            else if (_HPEnemy == 7)
            {
                _collider03.SetActive(true);
            }
            else if (_HPEnemy == 5)
            {
                _collider04.SetActive(true);
            }
            else if (_HPEnemy == 3)
            {
                _collider05.SetActive(true);
            }
            else if (_HPEnemy == 1)
            {
                _collider06.SetActive(true);
            }
            //Destroi laser
            Destroy(other.gameObject);

            //Verifica se o inimigo esta vivo
            if (_HPEnemy <= 0)
            {
                //Animação de explosão do inimigo
                Explosion_Enemy();
                _uiManager.UpdateScore(50);
                _uiManager.notifyBornAndDeathEnemy("death");
                //Destroi inimigo
                Destroy(this.gameObject);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.4f, 0);
            }
        }
        else if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            player.CheckPlayerHP();
            //uiManager.UpdateScore(10);
            Explosion_Enemy();

            Destroy(this.gameObject);
        }
        else if (other.tag == "Asteroid" || other.tag == "cilindroBomb")
        {
            Explosion_Enemy();
            Destroy(this.gameObject);
        }
    }
}
