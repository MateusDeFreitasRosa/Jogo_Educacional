using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public bool _isCoOpMode = false;
    public bool _gamerStarted;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _player1;
    [SerializeField] private GameObject _player2;
    private UIManager uiManager;
    public bool playerAlive;
    public bool bossAlive;
    private bool _variable;
    private int quantBos = 0;
    private float TIME_BORNBOS = 150.0f;
    private float _TIMETALK = .01f;

    private Dicionario _dicio;

    //Troca de dados entre cenas.
    [SerializeField] private DATA _callBetweenScenes;
    private LogicalDicio _logical;

    private bool _noticeNaveCilindro;



    //Instruções dentro de jogo
    [SerializeField] private history _hist;
    private string _instruct01 = "Bom dia Combatente! Sou o Mareshal Hector!";
    private string _instruct02 = "Venho lhe explicar como iremos vencer essa batalha!";
    private string _instruct03 = "Deixamos em sua cabine uma legenda de cores!";
    private string _instruct04 = "Você por ser nosso mensageiro, tem a função de nos enviar mensagens.";
    private string _instruct05 = "Você terá que formar as palavras que seus companheiros pedem!";
    private string _instruct06 = "Tome cuidado com Abatedores. São naves que estão prontas para te destruir.";
    private string _instruct07 = "Não tema em destrui-las. Elas não serão contabilizadas na frase.";
    private string _instruct08 = "Tenha uma boa batalha! Que nosso Deus cuide de nós!";
    private string _instructBosArrive = "Soldado fique atento, pois estamos detectando uma nave diferente das outras indo em sua direção!!!";
    private string _instructNaveLancaCilindro01 = "Soldado mantenha a atenção. Pois detectamos uma nave que carrega um cilindro e se detectado você ela o dispara para os lados!!";
    private string _instructNaveLancaCilindro02 = "Isso de alguma forma também pode te ajudar. Use com sabedoria!";
    private string _instructNaveLancaCilindro03 = "As destruições que esse cilindro causar não serão contabilizadas em suas mensagens!";

    [SerializeField] private TextMeshProUGUI _text;
    private SpawnManager _spawn;

    public int whatInstruct = 0;
    public int _pos = 0;

    //Alerta da nave do cilindro.
    
    public void ativeAlertNaveCilindro()
    {
        StartCoroutine(naveCilindroAlert());
    }

    IEnumerator naveCilindroAlert()
    {
        if (_instructNaveLancaCilindro01.Equals("NOONE"))
        {
            _text.SetText("");
            whatInstruct = 0;
        }
        else
        {
            if (_pos > 0)
                _text.SetText(_text.GetParsedText() + _instructNaveLancaCilindro01[_pos].ToString());
            else
                _text.SetText(_instructNaveLancaCilindro01[_pos].ToString());
          
            yield return new WaitForSeconds(_TIMETALK);
            _pos++;
            if (_pos < _instructNaveLancaCilindro01.Length)
                StartCoroutine(naveCilindroAlert());
            else
            {
                whatInstruct++;
                modeNaveCilindro(whatInstruct);
                StartCoroutine(stopTimeCilindro());
            }
        }
    }

    IEnumerator stopTimeCilindro()
    {
        yield return new WaitForSeconds(2.0f);
        _pos = 0;
        ativeAlertNaveCilindro();
    }

    private void modeNaveCilindro(int x)
    {
        if (x == 1)
        {
            _instructNaveLancaCilindro01 = _instructNaveLancaCilindro02;
        }
        else if (x == 2)
        {
            _instructNaveLancaCilindro01 = _instructNaveLancaCilindro03;
        }
        else
        {
            _instructNaveLancaCilindro01 = "NOONE";
        }
        _pos = 0;
    }

    



    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Communication_Scenes"))
        {
            _callBetweenScenes = GameObject.Find("Communication_Scenes").GetComponent<DATA>();
            _isCoOpMode = _callBetweenScenes.gameModeCoOp();
        }
        _spawn = GameObject.Find("SpawnMananger").GetComponent<SpawnManager>();
        Debug.Log("DATA: " + _isCoOpMode);
        _dicio = this.GetComponent<Dicionario>();

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _logical = GameObject.Find("GameManager").GetComponent<LogicalDicio>();
        if (uiManager && _gamerStarted == false)
        {
            uiManager.ScoreText.enabled = false;
            uiManager.levelUp.enabled = false;
            uiManager.Help.enabled = true;
            if (_isCoOpMode)
                uiManager.livesImage2.enabled = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Waiting();
        if (_spawn.dificult == 4 && _noticeNaveCilindro == false)
        {
            _noticeNaveCilindro = true;
            ativeAlertNaveCilindro();
        } 
    }

    public void SetStateOfGame(bool t)
    {
        _gamerStarted = t;
        playerAlive = t;
    }
    private void EnableBoos()
    {
        SpawnManager spawManager = GameObject.Find("SpawnMananger").GetComponent<SpawnManager>();
        GameObject find = GameObject.Find("Boos(Clone)");
        if (!find)
        {
            if (quantBos < 3)
            {
                quantBos++;
                spawManager.InstanciaBos(2.0f, 0.5f, 20, quantBos);
            }
            else if (quantBos == 3)
            {
                quantBos++;
                spawManager.InstanciaBos(2.5f, 0.4f, 35, quantBos);
            }
            else
            {
                spawManager.InstanciaBos(2.7f, 0.3f, 45, quantBos);
            }
        }
        else
            Debug.Log("Já existe.");
    }

    IEnumerator alertBos()
    {
        yield return new WaitForSeconds(TIME_BORNBOS / 1.2f);
        StartCoroutine(alertBosText());
        Debug.Log("Deveria estar mostrando a imagem!");
    }

    //Mensagem Alerta do nascimento do bos.
    IEnumerator alertBosText()
    {
        if (_pos > 0)
            _text.SetText(_text.GetParsedText() + _instructBosArrive[_pos].ToString());
        else
            _text.SetText(_instructBosArrive[_pos].ToString());

        yield return new WaitForSeconds(_TIMETALK);
        _pos++;
        if (_pos < _instructBosArrive.Length)
            StartCoroutine(alertBosText());
        else
        {
            _pos = 0;
        }
    }
    

    IEnumerator WaitTime()
    {
        while (_gamerStarted != false)
        {
            StartCoroutine(alertBos());
            yield return new WaitForSeconds(TIME_BORNBOS);
            EnableBoos();
        }
    }

    //Esperando o jogador a começar a partida
    public void Waiting()
    {
        if (_gamerStarted == false)
        {
            quantBos = 0;
            if (_variable == false) {
                StopAllCoroutines();
                _variable = true;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_isCoOpMode == true)
                {
                    // Caso seja Co-Op, teremos dois jogadores. Portanto a inicialização, tera 2
                    // naves, uma vermelha e outra auzl, por questão de diferenciação de ambas.
            
                    Instantiate(_player1, new Vector3(-5.2f, 0, 0), Quaternion.identity);
                    GameObject.Find("Player 1(Clone)").GetComponent<Player>().setControl(2);
                    
                    Instantiate(_player2, new Vector3(5.2f, 0, 0), Quaternion.identity);
                    GameObject.Find("Player 2(Clone)").GetComponent<Player>().setControl(1);
                }
                else
                {
                    Instantiate(_player, Vector3.zero, Quaternion.identity);
                    GameObject.Find("Player(Clone)").GetComponent<Player>().setControl(1);
                }
                StopAllCoroutines();
                _pos = 0;
                whatInstruct = 0;
                tutorialGame();
                uiManager.NotShowScreen();
                SetStateOfGame(true);
                StartCoroutine(WaitTime());
                _variable = false;
            }
        }

        

        else
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.L))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void setCo_Op_State(bool state)
    {
        _isCoOpMode = state;
    }

    //InterfaceJogo

    public void tutorialGame()
    {
        StartCoroutine(tut());
    }



    IEnumerator tut()
    {
        if (_instruct01.Equals("NOONE"))
        {
            _text.SetText("");
            whatInstruct = 0;
        }
        else
        {
            if (_pos > 0)
                _text.SetText(_text.GetParsedText() + _instruct01[_pos].ToString());
            else
                _text.SetText(_instruct01[_pos].ToString());
          
            yield return new WaitForSeconds(_TIMETALK);
            _pos++;
            if (_pos < _instruct01.Length)
                StartCoroutine(tut());
            else
            {
                whatInstruct++;
                mode(whatInstruct);
                StartCoroutine(stopTime());
            }
        }
    }

    IEnumerator stopTime()
    {
        yield return new WaitForSeconds(2.0f);
        _pos = 0;
        tutorialGame();
    }

    private void mode(int x)
    {
        if (x == 1)
            _instruct01 = _instruct02;
        else if (x == 2)
            _instruct01 = _instruct03;
        else if (x == 3)
        {
            _instruct01 = _instruct04;
            _logical.chooseAWord();
        }
        else if (x == 4)
            _instruct01 = _instruct05;
        else if (x == 5)
            _instruct01 = _instruct06;
        else if (x == 6)
            _instruct01 = _instruct07;
        else if (x == 7)
            _instruct01 = _instruct08;
        else
        {
            _instruct01 = "NOONE";
            _logical.bemVindo();
            _spawn.activeSpawn();
        }
        _pos = 0;
    }



}

