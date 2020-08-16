using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private Player _player;

    private GameManager gameManager;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _littleExplosionPrefab;
    [SerializeField] private GameObject _explosao;

    //Colisões
    [SerializeField] private GameObject _explosao1;
    [SerializeField] private GameObject _explosao2;
    [SerializeField] private GameObject _explosao3;
    [SerializeField] private GameObject _explosao4;
    [SerializeField] private GameObject _explosao5;

    private GameManager _gameManager;
    private SpawnManager _spawManager;
    private bool _eraser;
    public float _timeOfShoot = 0.5f;

    private float pPlayer;
    private float pBos;

    public float _speed = 2f;
    private int _control = 0;
    public int _hpBoos = 20;

    private int point;
    // Start is called before the first frame update
    void Start()
    {
        GameObject find, find2;
        _spawManager = GameObject.Find("SpawnMananger").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _gameManager.bossAlive = true;
        if (_gameManager.playerAlive == false)
            _eraser = true;
        find = GameObject.Find("Player(Clone)");

        _spawManager.stopSpawn();
        if (!find)
        {
            Debug.Log("Player not exist");
            point = UnityEngine.Random.Range(1, 3);
            find = GameObject.Find("Player 1(Clone)");
            find2 = GameObject.Find("Player 2(Clone)");
            Debug.Log("O jogador principal não existe!");

            if (find && find2)
            {
                Debug.Log("Portanto, há dois jogadores do modo (CoOp) vivos!");
                if (point == 1)
                    _player = find.GetComponent<Player>();
                else if (point == 2)
                    _player = find2.GetComponent<Player>();
            }
            else if (find)
                _player = find.GetComponent<Player>();
            else if (find2 != null)
                _player = find.GetComponent<Player>();

        }
        else
        {
            Debug.Log("Player exist");
            _player = GameObject.Find("Player(Clone)").GetComponent<Player>();
        }
        StartCoroutine(ShootWithTime());
        _explosao1.SetActive(false);
        _explosao2.SetActive(false);
        _explosao3.SetActive(false);
        _explosao4.SetActive(false);
        _explosao5.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.playerAlive == false && _eraser == false)
            _eraser = true;
        if (_eraser == true && _gameManager._gamerStarted == true)
        {
            Destroy(this.gameObject);
            _spawManager.AtivaSpawn();
        }

        if (transform.position.y > 3)
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        else
        {
            SearchPlayer();
            ShootWithTime();
        }
    }


    private void SearchPlayer()
    {
        if (_gameManager._isCoOpMode == false)
        {
            if (_gameManager.playerAlive == true)
            {
                pPlayer = _player.transform.position.x;
                pBos = transform.position.x;

                if (pPlayer > pBos)
                {
                    transform.Translate(Vector3.right * (_speed) * Time.deltaTime);
                }
                else if (pPlayer < pBos)
                {
                    transform.Translate(Vector3.left * (_speed) * Time.deltaTime);
                }
            }
        }
        else
        {
            if (!_player)
            {
                int nextPlayer = (int) Math.Pow(((point + 1) % 3), Math.Abs(point - 2));
                if(GameObject.Find("Player "+nextPlayer+"(Clone)"))
                {
                    GameObject find = GameObject.Find("Player " + nextPlayer + "(Clone)");
                    _player = find.GetComponent<Player>();
                }
            }
            pPlayer = _player.transform.position.x;
            pBos = transform.position.x;

            if (pPlayer > pBos)
            {
                transform.Translate(Vector3.right * (_speed) * Time.deltaTime);
            }
            else if (pPlayer < pBos)
            { 
                transform.Translate(Vector3.left * (_speed) * Time.deltaTime);
            }
                              
            

        }
    }

    private void Shoot()
    {
        if (_control == 0)
        {
             Instantiate(_laserPrefab, transform.position + new Vector3(-0.65f, 0, 0), Quaternion.identity);
            _control++;
        }
       else if (_control == 1)
       {
             Instantiate(_laserPrefab, transform.position + new Vector3(0, -1.55f, 0), Quaternion.identity);
             _control++;
       }
       else if (_control == 2)
       {
            Instantiate(_laserPrefab, transform.position + new Vector3(+0.65f, 0, 0), Quaternion.identity);
            _control = 0;
       }
    }

    IEnumerator ShootWithTime()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(_timeOfShoot);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "cilindroBomb" || (collision.tag == "Asteroid" && !collision.gameObject.ToString().Contains("Asteroid")))
        {
            Instantiate(_explosao, transform.position, Quaternion.identity);
            _spawManager.AtivaSpawn();
            UIManager uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
            uiManager.UpdateScore(50);
            Destroy(this.gameObject);
        }
        if (_gameManager.playerAlive)
        {
            if (collision.tag == "laser")
            {
                Instantiate(_littleExplosionPrefab, collision.transform.position, Quaternion.identity);
                _hpBoos--;

                if (_hpBoos == 17)
                    _explosao1.SetActive(true);
                else if (_hpBoos == 14)
                    _explosao2.SetActive(true);
                else if (_hpBoos == 11)
                    _explosao3.SetActive(true);
                else if (_hpBoos == 8)
                    _explosao4.SetActive(true);
                if (_hpBoos == 3)
                    _explosao5.SetActive(true);

                if (_hpBoos <= 0)
                {
                    Instantiate(_explosao, transform.position, Quaternion.identity);                    
                    _spawManager.AtivaSpawn();
                    UIManager uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
                    uiManager.UpdateScore(50);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
