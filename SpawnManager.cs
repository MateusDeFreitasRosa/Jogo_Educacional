using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemySpawn;
    [SerializeField] private GameObject[] _powerUps;
    [SerializeField] private GameObject _asteroid;
    [SerializeField] private GameObject _BossPrefab;
    [SerializeField] private GameObject _naveHorizontal;
    [SerializeField] private GameObject _abatedor;
    [SerializeField] private GameObject _naveLancaCilindro;

    private bool _stopSpawn;
    
    public int dificult = 0 ;
    private UIManager uiManager;
    private int _random;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _stopSpawn = false;
    }

    public void activeSpawn()
    {
        StartCoroutine(SpawnEnemysCoroutine());
    }

    public void stopSpawn()
    {
        StopCoroutine(SpawnEnemysCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        Dificulty();
    }

    private void InstanciaEnemy()
    {
        Instantiate(_enemySpawn, new Vector3(Random.Range(-7, 7), 7, 0), Quaternion.identity);
    }

    private void InstanciaPowerUps(int n)
    {
        Instantiate(_powerUps[n], new Vector3(Random.Range(-7, 7), 7, 0), Quaternion.identity);
    }
    private void InstanciaAsteroids()
    {
        Instantiate(_asteroid, new Vector3(Random.Range(-7, 7), 7, 0), Quaternion.identity);
    }

    public void AtivaSpawn()
    {
        _stopSpawn = false;
        activeSpawn(); 
    }


    private void Dificulty()
    {
        if (uiManager.Score < 50)
            dificult = 0;
        else if (uiManager.Score >= 50 && uiManager.Score < 100)
        {   
            dificult = 1;
        }
        else if (uiManager.Score >= 100 && uiManager.Score < 200)
            dificult = 2;
        else if (uiManager.Score >= 200 && uiManager.Score < 300)
            dificult = 3;
        else if (uiManager.Score >= 300 && uiManager.Score < 400)
        {
            dificult = 4;
        }
        else if (uiManager.Score >= 400 && uiManager.Score < 500)
            dificult = 5;
        else if (uiManager.Score > 500 && uiManager.Score < 3000)
            dificult = 6;
        else if (uiManager.Score >= 3000)
            dificult = 7;
        invoc(dificult);
    }

    public void InstanciaBos(float v, float shoot, int hp, int q)
    {
        Instantiate(_BossPrefab, new Vector3(Random.Range(-7, 7), 7, 0), Quaternion.identity);
        Boss boss = GameObject.Find("Boos(Clone)").GetComponent<Boss>();
        if (q < 3)
            _stopSpawn = true;
        else if (q == 3)
        {
            v = 2.5f;
            shoot = 0.4f;
            hp = 35;
            InstanciaNaveHorizontal();
            InstanciaNaveHorizontal();
            _stopSpawn = true;
            boss._hpBoos = hp;
            boss._timeOfShoot = shoot;
            boss._speed = v;
        }
        else if (q >= 4)
        {
            v = 2.7f;
            shoot = 0.3f;
            hp = 45;
            boss._hpBoos = hp;
            boss._timeOfShoot = shoot;
            boss._speed = v;
        }
    }

    public void invoc(int d)
    {
        uiManager.UpdateNivel("Nivel: "+ d);
    }

    private void InstanciaNaveHorizontal()
    {
        Instantiate(_naveHorizontal, new Vector3(10, Random.Range(-4, 4), 0), Quaternion.Euler(0,0,90));
    }

    private void InstanciaAbatedor()
    {
        Instantiate(_abatedor, new Vector3(Random.Range(-7, 7), 7, 0), Quaternion.identity);
    }

    private void InstanciaPortaCilindroBomb()
    {
        Instantiate(_naveLancaCilindro, new Vector3(Random.Range(-7, 7), 7, 0), Quaternion.identity);
    }
    IEnumerator SpawnEnemysCoroutine()
    {
        bool bornEnemy;
        while (_stopSpawn == false)
        {
            for (int i=0; i <= dificult +1; i++)
            {

                bornEnemy = false;
                _random = Random.Range(0, 50);
                if (_random >= 0 && _random <= 3)
                {
                    InstanciaPowerUps(_random);
                }
                else if ((_random > 3 && _random <= 30) && dificult < 5)
                {
                    InstanciaEnemy();
                    bornEnemy = true;
                }
                else if ((_random > 3 && _random <= 25) && dificult >= 5)
                {
                    InstanciaEnemy();
                    bornEnemy = true;
                }
                //else if (_random == 20 && dificult >=5)
                // {
                //    InstanciaNaveHorizontal();
                //}
                else if ((_random > 25 && _random <= 31))
                {
                    InstanciaAbatedor();
                    bornEnemy = true;
                }
                else if (_random >= 32 && _random <= 35)
                {
                    InstanciaAsteroids();
                    bornEnemy = true;
                }
                else if ((_random >= 36 && _random <= 38) && dificult >= 1)
                {
                    InstanciaPortaCilindroBomb();
                    bornEnemy = true;
                }
                if (bornEnemy)
                    uiManager.notifyBornAndDeathEnemy("born");
            }
            yield return new WaitForSeconds(5.0f);
          
        }
    }

}
