using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // Variaveis globais;
    [SerializeField]
    private GameObject _laserPrefab = null; // Laser

    [SerializeField]
    private GameObject _tripleShoot = null; // Triple laser

    [SerializeField]
    private GameObject _explosionPlayerPrefab; // Animação de Explosão de Player

    //Sound pick PowerUp
    private AudioSource _audioSource;

    //Objetos do Player
    [SerializeField]
    private GameObject _ShieldPrefab;
    [SerializeField]
    private GameObject _thrusterNormal;
    [SerializeField]
    private GameObject _thursterSpeedBoost;
    [SerializeField]
    private GameObject _littleExplosion;
    [SerializeField]
    private GameObject _collider1;
    [SerializeField]
    private GameObject _collider2;
    [SerializeField] private GameObject _posOutLaiser;


    //Manobras com o Player

    [SerializeField]
    private float _fireRate = 0.25f; //CoolDown da habiidade
    private float _canFire = 0.0f; // Verifica se acabou o CoolDown para o Player utilizar novamente a habilidade

    [SerializeField]
    private float _speed = 2.0f; // Velocidade

    [SerializeField]
    private float _jumpInput; // Impulso de velocidade

    public int playerHP = 3; // HP Player
    public int ShieldHP; // HP Shield

    //Classes
    private UIManager uiManager;

    //Variaveis do PowerUp
    public bool canTripleShoot; //Permissão para o Tiro Triplo (TripleShoot)
    public bool canSpeedBoost; // Permissão para o Aumento de Velocidade (Speed Boost)
    public bool canShield; //Permissao para usar escudo (Shield)

    
    //Modo de jogo.
    [SerializeField] private bool _modoFlexivel;
    private bool _isCoOpMode;
    private DATA _data;

    //Controle
    private byte _control;

    private bool _start = false;
    // Start is called before the first frame update
    void Start()
    {
        
        _modoFlexivel = true;
        _audioSource = GetComponent<AudioSource>();
        _collider1.SetActive(false);
        _collider2.SetActive(false);
        _ShieldPrefab.SetActive(false);
        _thursterSpeedBoost.SetActive(false);
        
        if(GameObject.Find("Communication_Scenes")) 
            _data = GameObject.Find("Communication_Scenes").GetComponent<DATA>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_start)
        {
            Moviment();
            Regressividade(-.1f);
            InstanciacaoLaser();
            if (_modoFlexivel)
            {
                turnNave();
            }
        }
    }


    public void setControl(byte b)
    {
        _control = b;
        _start = true;

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (uiManager != null)
            uiManager.UpdateLives(playerHP, b);
    }

    public byte getControl()
    {
        return _control;
    }

    // Volta Regressiva para trás, (VELOCIDADE);
    public void Regressividade(float impact)
    {
        if (transform.position.y > -4.5)
            transform.Translate(Vector3.up * Time.deltaTime * impact);
    }



    // CoolDown
    public bool CoolDown()
    {
        if (Time.time > _canFire)
            return true;
        else
            return false;
    }

    public void PotionHP()
    {
        if (playerHP < 3)
        {
            playerHP++;
            uiManager.UpdateLives(playerHP, _control);
        }
        if (playerHP == 2)
        {
            _collider2.SetActive(false);
        }
        else if (playerHP == 3)
        {
            _collider1.SetActive(false);
        }
    }

    //Laser
    private void InstanciacaoLaser()
    {
        bool c = false;
        if (_control == 2)
            c = Input.GetKeyDown(KeyCode.Q);
        else if (_control == 1)
            c = Input.GetKeyDown(KeyCode.RightShift);

        if (c)
        {
            //CoolDown do Laser
            if (CoolDown() == true)
            {
                if (canTripleShoot == true)
                {
                    Instantiate(_tripleShoot, transform.position, transform.rotation);
                }
                else
                {
                    if (_modoFlexivel == true)
                    {
                        Instantiate(_laserPrefab, new Vector3(_posOutLaiser.transform.position.x,
                                                              _posOutLaiser.transform.position.y,
                                                              0), _posOutLaiser.transform.rotation);
                    }
                    else
                    {
                        Instantiate(_laserPrefab, transform.position + new Vector3(0,0.88f,transform.position.z), Quaternion.identity);
                    }
                }
                _canFire = Time.time + _fireRate;
            }
        }
    }

    //Pick PowerUp
    public void PickPowerUp()
    {
        _audioSource.Play();
    }

    

    //Controlando animacao Player, esquerda direita

    // Movimentação;
    public void Moviment()
    {
        float horizontalInput = 0;
        float verticalInput = 0;

        SpeedBoostConfirm();
        if (_control == 1)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }
        else if (_control == 2)
        {
            horizontalInput = Input.GetAxis("Horizontal2");
            verticalInput = Input.GetAxis("Vertical2");
        }
        float localX = transform.position.x;
        float localY = transform.position.y;

        _jumpInput = Input.GetAxis("Jump");

        // Bloqueio de movimentação no limite X ************************************************************************************************************************;
        if (localX <= 8.3)
            transform.Translate(Vector3.right * Time.deltaTime * _speed * horizontalInput);
        else
            transform.position = new Vector3(8.3f, localY, 0);

        if (localX >= -8.3)
            transform.Translate(Vector3.right * Time.deltaTime * _speed * horizontalInput);
        else
            transform.position = new Vector3(-8.29f, localY, 0);

        // *************************************************************************************************************************************************************;


        // Bloqueio de movimentação no limite y ************************************************************************************************************************;

        // Jump pois caso ele ative o turbo da nave.
        if (verticalInput > 0.0)
        {
            if (localY >= -4.5)
                transform.Translate(Vector3.up * Time.deltaTime * _speed * verticalInput * ((_jumpInput * 1) + 1));
            else
                transform.position = new Vector3(localX, -4.5f, 0);

            if (localY <= 4.5)
                transform.Translate(Vector3.up * Time.deltaTime * _speed * verticalInput * ((_jumpInput * 1) + 1));
            else
                transform.position = new Vector3(localX, 4.5f, 0);
        }
        else
        {
            if (localY >= -4.5)
                transform.Translate(Vector3.up * Time.deltaTime * _speed * verticalInput);
            else
                transform.position = new Vector3(localX, -4.5f, 0);
            if (localY <= 4.5)
                transform.Translate(Vector3.up * Time.deltaTime * _speed * verticalInput);
            else
                transform.position = new Vector3(localX, 4.5f, 0);
        }
        // *************************************************************************************************************************************************************;
    }

    //Ativação do TripleShoot
    // Essa função tem o Objetivo de mesmo excluindo o TripleShootPowerUp o tempo de contagem não seja perdido
    public void TripleShootOn()
    {
        canTripleShoot = true;
        StartCoroutine(TripleShootCoolDown());
    }
    
    //CoolDown de TripleShoot (PowerUp)
    //Esse codigo é apenas executado apos passado o tempo determinado
    public IEnumerator TripleShootCoolDown()
    {
        //yield suspende a execução APENAS desse codigo, por um determinado tempo,(O resto do codigo continua sendo executado normalmente).
        //Enfim, quando o determinado tempo passa, o codigo volta para essa execução.

        yield return new WaitForSeconds(7.0f);
        canTripleShoot = false;
    }


    //Ativação do Speed Boost
    public void SpeedBoostOn()      
    {
        canSpeedBoost = true;
        _thrusterNormal.SetActive(false);
        _thursterSpeedBoost.SetActive(true);
        StartCoroutine(SpeedBoostPowerUp());
    }


    //CoolDown SpeedBoost (PowerUps)
    public IEnumerator SpeedBoostPowerUp()
    {
        yield return new WaitForSeconds(6.0f);
        _thursterSpeedBoost.SetActive(false);
        _thrusterNormal.SetActive(true);
        canSpeedBoost = false;
    }

    //Verifica se existe SpeedBoost
    public void SpeedBoostConfirm()
    {
        if (canSpeedBoost == true)
            _speed = 4.0f;
        else
            _speed = 2.0f;
    }

    // Ativa o escudo
    public void EnableShield()
    {
        canShield = true;
        ShieldHP = 3;
        _ShieldPrefab.SetActive(true);
        StartCoroutine(Shield());
    }

    public IEnumerator Shield()
    {
        yield return new WaitForSeconds(10.0f);
        canShield = false;
        _ShieldPrefab.SetActive(false);
    }

    //Instanciação da explosão do player
    public void Explosion_Player()
    {
        Instantiate(_explosionPlayerPrefab, transform.position, Quaternion.identity);
    }


    //Verifica se o player esta vivo, caso ele morra, a animação de explosão da nave acontece
    public void CheckPlayerHP()
    {
        if (canShield == true)
        {
            ShieldHP--;
            if (ShieldHP <= 0)
            {
                canShield = false;
                if (_ShieldPrefab != null)
                    _ShieldPrefab.SetActive(false);
            }
        }
        else
        {
            if (playerHP <= 1)
            {
                uiManager.UpdateLives(playerHP - 1, _control);
                //Animação do player Explodindo
                Explosion_Player();
                //Destroi o player
                Destroy(this.gameObject);
            }
            else
            {
                playerHP--;
                uiManager.UpdateLives(playerHP, _control);
                if (playerHP == 2)
                    _collider1.SetActive(true);
                else if (playerHP == 1)
                    _collider2.SetActive(true);
            }
        }
        
    }

    private void turnNave()
    {
        if (Input.GetKey(KeyCode.J))
        {
            transform.Rotate(0, 0, transform.rotation.z + 2);
        }
        if (Input.GetKey(KeyCode.K))
        {
            transform.Rotate(0, 0, transform.rotation.z - 2);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with: " + collision.tag);
        if (collision.tag == "laserEnemy" || collision.tag == "AtackHorizontal")
        {
            if (canShield == true)
            {
                CheckPlayerHP();
                Instantiate(_littleExplosion, collision.transform.position, Quaternion.identity);
                Destroy(collision.transform.gameObject);
            }
            CheckPlayerHP();
            Instantiate(_littleExplosion, collision.transform.position, Quaternion.identity);
            Destroy(collision.transform.gameObject);
        }  
        else if (collision.tag == "cilindroBomb")
        {
            uiManager.UpdateLives(0, getControl());
            Explosion_Player();
            Destroy(this.gameObject);
        }
    }
}
