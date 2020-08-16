using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abatedor : MonoBehaviour
{
    [SerializeField] private GameObject _collider01;
    [SerializeField] private GameObject _collider02;
    [SerializeField] private GameObject _collider03;

    //Velocidade da nave.
    private float _speed = .5f;

    //Animação da explosão.
    [SerializeField] private GameObject _enemyExplosion;
    [SerializeField] private GameObject _pequenaExplosao;

    //Laser
    [SerializeField] private GameObject _laser;

    //Player alvo.
    private Player _player;

    private GameManager _gameManager;
    //UIManager.
    private UIManager _uiManager;
    private int _HPEnemy;
    private float _TIMEOFSHOOT = 6.0f;
    private bool _canFire;
    private bool _eraser;
    private SpawnManager _spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnMananger").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _HPEnemy = 4;
        Init();
        GameObject find;
        find = GameObject.Find("Player(Clone)");
        if (find)
        {
            _player = find.GetComponent<Player>();
        }
        _canFire = true;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player)
        {
            Moviment();
            if (_player.transform.position.x <= transform.position.x + 2 && _player.transform.position.x >= transform.position.x - 2 && _canFire == true)
            {
                _canFire = false;
                StartCoroutine(ShootWithTime());
            }
        }

        if (_player && _eraser == true)
        {
            Destroy(this.gameObject);
        }
        if (!_player && _eraser == false)
            _eraser = true;
    }

    private void Init()
    {
        _collider01.SetActive(false);
        _collider02.SetActive(false);
        _collider03.SetActive(false);
        _HPEnemy = 4;
    }

    //Movimentação.
    public void Moviment()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //Se ele sair da tela, para reaproveitarmos o inimigo, ele volta para cima na tela.
        if (transform.position.y <= -7 || transform.position.x > 10 || transform.position.x < -10)
        {
            _uiManager.UpdateScore(-5);
            Destroy(this.gameObject);
        }
        else
        {
            if (_player.transform.position.x > transform.position.x)
            {
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
            }
        }
    }

    IEnumerator ShootWithTime()
    {
        Instantiate(_laser,transform.position + new Vector3(0,-.45f,0), Quaternion.identity);
        yield return new WaitForSeconds(_TIMEOFSHOOT);
        _canFire = true;
    }

    //Explosão Enenemy.
    public void Explosion_Enemy()
    {
        Instantiate(_enemyExplosion, transform.position, Quaternion.identity);
    }


    private void instanciaLaser()
    {
        Instantiate(_laser, transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
    }


    //Colisão do laser com o inimigo
    public void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Object colided with: " + other);
        if (other.tag == "laser")
        {
            _uiManager.UpdateScore(2);
            _HPEnemy--;
            // Instancia pequena explosao
            Instantiate(_pequenaExplosao, new Vector3(other.transform.position.x, other.transform.position.y + .4f, 0), Quaternion.identity);

            if (_HPEnemy == 3)
            {
                _collider01.SetActive(true);
            }
            if (_HPEnemy == 2)
            {
                _collider02.SetActive(true);
            }
            else if (_HPEnemy == 1)
            {
                _collider03.SetActive(true);
            }
            //Destroi laser
            Destroy(other.gameObject);

            //Verifica se o inimigo esta vivo
            if (_HPEnemy <= 0)
            {
                //Animação de explosão do inimigo
                Explosion_Enemy();
                _uiManager.UpdateScore(20);
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
