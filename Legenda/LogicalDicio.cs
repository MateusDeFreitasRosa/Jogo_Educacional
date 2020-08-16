using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogicalDicio : MonoBehaviour
{
    private int _contEnemys = -1;
    private string[] _words = new string[] { "bola", "goiaba", "plutão", "globo", "arvore",
    "leite", "menor", "trigo"};
    [SerializeField]
    private AudioSource[] _sounds = new AudioSource[]
    {
        //Sons foram adicionadas na interface.
    };
    private UIManager _uiManager;
    [SerializeField] private Image _imgVolume;
    private int _pos;
    private string _aWord;
    public int dificult;
   

    // Others Components.
    private Dicionario _dicio;



    private void Start()
    {
        _dicio = GameObject.Find("GameManager").GetComponent<Dicionario>();
        _imgVolume.enabled = false;
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }
    private void Update()
    {
        //StartCoroutine(cooldownTalk());
    }

    public void chooseAWord()
    {
        _pos = Random.Range(0, _words.Length);
        _aWord = _words[_pos];
        Debug.Log("Palavra: " + _aWord+"\n Tamanho dela: "+_aWord.Length);
        _dicio.createCubeColorForPhrase(_aWord, dificult);
        _dicio.restartWord();
        Initialize();
        talk();
    }

    public void bemVindo()
    {
        _dicio.showMensagemMiddleScreen("Bom jogo");
    }


    public void killedEnemy(char c)
    {
        _contEnemys++;
        Debug.Log("Char: " + c + "\n Palavra" + _aWord[_contEnemys]);
        if (checkPhrase(c, _contEnemys))
        {
            _dicio.printChar(c, _contEnemys);
            if(_contEnemys == _aWord.Length-1)
            {
                _uiManager.UpdateScore(20);
                
                _dicio.showMensagemMiddleScreen("Palavra formada com sucesso!\nUm novo desafio foi imposto!!");
                StartCoroutine(Complete());
            }
            _uiManager.UpdateScore(5);
        }
        else
        {
            Initialize();
        }
    }

    IEnumerator Complete()
    {
        yield return new WaitForSeconds(5.0f);
        chooseAWord();
    }
    private void Initialize()
    {
        _contEnemys = -1;
        _dicio.restartWord();
    }

    public bool checkPhrase(char c, int pos)
    {
        if (c.ToString().ToUpper().Equals(_aWord[pos].ToString().ToUpper()))
        {
            return true;
        }
        return false;
    }
   

    private void talk()
    {
        StartCoroutine(returnTalk());
    }

    IEnumerator returnTalk()
    {
        _imgVolume.enabled = true;
        _sounds[_pos].Play();
        yield return new WaitForSeconds(2.0f);
        _imgVolume.enabled = false;
        StartCoroutine(cooldownTalk());
    }

    IEnumerator cooldownTalk()
    {
        Debug.Log("Entrou na coolDownTalk");
        yield return new WaitForSeconds(10.0f);
        talk();
    }

}
