using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{

    [SerializeField]
    private float speed = 3.0f;

    [SerializeField]
    private int PowerUpID; // 0 = Triple Shoot //// 1 = Speed Boost //// 2 = Shield //// 3 = HP

    [SerializeField]
    private GameObject _explosao;
    private GameManager _gameManager;
    private bool _eraser;

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager._gamerStarted == false)
            _eraser = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (transform.position.y <= -6)
            Destroy(this.gameObject);

        if (_eraser == true && _gameManager._gamerStarted == true)
            Destroy(this.gameObject);

        if (_gameManager.playerAlive == false && _eraser == false)
            _eraser = true;

    }
    
    public void Explosao()
    {
        Instantiate(_explosao, transform.position, Quaternion.identity);
    }


    // Verifica se o Power Up colidiu com o Player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            //Pegando o script que esta no Player e colocando no objeto player
            Player player = other.GetComponent<Player>();

            //Verificar se o player existe
            if (player != null)
            {
                //Ativa o Power Up que esta no script Player. (Triple Shoot)
                //Chama a "Coroutine", (CoolDown do PowerUp)
                if (PowerUpID == 0)
                    player.TripleShootOn();
                else if (PowerUpID == 1)
                    player.SpeedBoostOn();
                else if (PowerUpID == 2)
                    player.EnableShield();
                else if (PowerUpID == 3)
                    player.PotionHP();
                player.PickPowerUp();
            }
            
            //Destroi o objeto
            Destroy(this.gameObject);
        }
        else if (other.tag == "Asteroid")
        {
            Explosao();
            Destroy(this.gameObject);
        }
        
        
    }

}
