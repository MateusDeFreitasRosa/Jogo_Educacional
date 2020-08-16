using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalAtack : MonoBehaviour
{
    private Player _player;
    private GameManager _gameManager;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _explosao;
    [SerializeField] private float _speed = 1.0f;
    private float _pPlayer;
    private float _pHorizontalAtack;
    private int _controlShoot = 0;
    private bool _eraser;
    private float point;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject find, find2;
        transform.localRotation = Quaternion.Euler(0, 0, 90); 
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        find = GameObject.Find("Player(Clone)");
        if (find)
        {
            _player = find.GetComponent<Player>();
        }
        else
        {
            find = GameObject.Find("Player 1(Clone)");
            find2 = GameObject.Find("Player 2(Clone)");
            if(find && find2)
            {
                int point = UnityEngine.Random.Range(1, 3);
                _player = GameObject.Find("Player " + (char)point + "(Clone)").GetComponent<Player>();
            }
            else if (find)
            {
                _player = find.GetComponent<Player>();
            }
            else  if (find2)
            {
                _player = find2.GetComponent<Player>();
            }
        }
        if (_player != null)
            Debug.Log("Exist");
        else
        {

            Debug.Log("Not Exist");
        }
        if (_gameManager._gamerStarted == false)
            _eraser = true;
        StartCoroutine(KeepCalm());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (_gameManager.playerAlive == true)
        {
            SearchPlayer();
            if (transform.position.x <= -10)
                Destroy(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    private void SearchPlayer()
    {
        if (!_player)
        {
            int nextPlayer = (int)Math.Pow(((point + 1) % 3), Math.Abs(point - 2));
            if (GameObject.Find("Player " + nextPlayer + "(Clone)"))
            {
                GameObject find = GameObject.Find("Player " + nextPlayer + "(Clone)");
                _player = find.GetComponent<Player>();
            }
        }

        _pPlayer = _player.transform.position.y;
        _pHorizontalAtack = transform.position.y;

        if (_pPlayer > _pHorizontalAtack)
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        else if (_pPlayer < _pHorizontalAtack)
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
    }


    private void Dispara()
    {
        if (_gameManager.playerAlive == true)
        {
            if (_controlShoot == 0)
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(-1, 0, 0), Quaternion.Euler(0, 0, 90));
                _controlShoot++;
            }
            else if (_controlShoot == 1)
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(-.25f, .4f, 0), Quaternion.Euler(0, 0, 90));
                _controlShoot++;
            }
            else if (_controlShoot == 2)
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0.25f, -.4f, 0), Quaternion.Euler(0, 0, 90));
                _controlShoot = 0;
            }
        }
    }

    IEnumerator KeepCalm()
    {
        while (true)
        {
            Dispara();
            yield return new WaitForSeconds(1);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Asteroid" || collision.tag == "laser")
        {
            Instantiate(_explosao, transform.position, Quaternion.identity);
            _player.CheckPlayerHP();
            Destroy(this.gameObject);
        }

    }
}
