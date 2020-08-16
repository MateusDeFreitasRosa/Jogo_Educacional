using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [SerializeField]
    private float _speed = 0.2f;

    [SerializeField]
    private GameObject _enemyExplosion;

    [SerializeField]
    private GameObject _pequenaExplosao;

    [SerializeField]
    private int EnemyHPTotal = 3;

    [SerializeField]
    private GameObject _laserEnemy;

    [SerializeField]
    private float _speedOfShoot;

    //FireCollider
    [SerializeField]
    private GameObject _fireCollider;
    [SerializeField]
    private GameObject _fireCollider2;

    public GameObject spawn;

    private bool _eraser;

    public int HPEnemy;
    private UIManager uiManager;
    private GameManager _gameManager;

    //Sistema de alfabetização.
    private Dicionario _dicio;
    private LogicalDicio _logical;
    [SerializeField] private Renderer _cap01;
    [SerializeField] private Renderer _cap02;
    [SerializeField] private char _letterMatching;
    // Start is called before the first frame update
    void Start()
    {
        _fireCollider.SetActive(false);
        _fireCollider2.SetActive(false);
         _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
         if (_gameManager._gamerStarted == false)
        {
            _eraser = true;
        }

        HPEnemy = EnemyHPTotal;
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (uiManager.Score > 500 && uiManager.Score < 2000)
        {
            _speedOfShoot = 3.0f;
            startaOCorote();
        }
        else if (uiManager.Score >= 2000)
        {
            _speedOfShoot = 1.5f;
            startaOCorote();
        }
        _dicio = GameObject.Find("GameManager").GetComponent<Dicionario>();
        _logical = GameObject.Find("GameManager").GetComponent<LogicalDicio>();
        if (_dicio)
        {
            _cap01 = GetComponent<Renderer>();
            int a = _dicio.lenLista();
            _letterMatching = _dicio.whatLetter(a);
            _cap01.material.color = _dicio.whatColor_ForCube(a);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Moviment();
        if (_gameManager._gamerStarted == true && _eraser ==  true)
        {
            Destroy(this.gameObject);
        }
        if (_gameManager.playerAlive == false && _eraser == false)
            _eraser = true;
    }
    private void startaOCorote()
    { 
        StartCoroutine(CoolDownlaserEnemy());
    }

    //CoolDown Enemy
    IEnumerator CoolDownlaserEnemy()
    {
        while (true)
        {
            EnemyWithLaser();
            yield return new WaitForSeconds(_speedOfShoot);
        }
    }

    //MovimentacaoIA
    public void Moviment()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        //Se ele sair da tela, para reaproveitarmos o inimigo, ele volta para cima na tela.
        if (transform.position.y <= -7 || transform.position.x > 10 || transform.position.x < -10)
        {
            _fireCollider.SetActive(false);
            _fireCollider2.SetActive(false);
            float randomX = UnityEngine.Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(randomX, 7, 0);
            //uiManager.UpdateScore(-5);
            HPEnemy = 3;
            _cap01 = GetComponent<Renderer>();
            int a = _dicio.lenLista();
            _letterMatching = _dicio.whatLetter(a);
            _cap01.material.color = _dicio.whatColor_ForCube(a);
        }
    }
    
    //Instanciação do Objeto (Enemy_Explosion)

    public void Explosion_Enemy()
    {
        Instantiate(_enemyExplosion, transform.position, Quaternion.identity);
    }


    //Colisão do laser com o inimigo
    public void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Object colided with: " + other);
        if (other.tag == "laser")
        {
            //uiManager.UpdateScore(2);
            HPEnemy--;
            // Instancia pequena explosao
            Instantiate(_pequenaExplosao, new Vector3(other.transform.position.x, other.transform.position.y +.4f, 0), Quaternion.identity);
            
            if (HPEnemy == 2)
            {
                _fireCollider.SetActive(true);
            }
            else if (HPEnemy == 1)
            {
                _fireCollider2.SetActive(true);
            }
            //Destroi laser
            Destroy(other.gameObject);
            
            //Verifica se o inimigo esta vivo
            if (HPEnemy <= 0)
            {
                //Animação de explosão do inimigo
                Explosion_Enemy();
                uiManager.UpdateScore(10);

                _logical.killedEnemy(_letterMatching);
                uiManager.notifyBornAndDeathEnemy("death");
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
            
            _logical.killedEnemy(_letterMatching);

            Destroy(this.gameObject);
        }
        else if (other.tag == "Asteroid" || other.tag == "cilindroBomb")
        {
            Explosion_Enemy();
            Destroy(this.gameObject);
        }
    }

    public void EnemyWithLaser()
    {
        Instantiate(_laserEnemy, transform.position + new Vector3 (0,-0.4f,0), Quaternion.identity);
    }
}
