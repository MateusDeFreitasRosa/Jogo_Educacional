using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class history : MonoBehaviour
{
    private string _history;
    private string _history01 = "No ano de 2067 a NASA detectou luzes fortes " +
        "vindo de um buraco branco, (buracos brancos são as saídas de buracos negros) " +
        "a bilhões de anos luzes da terra. Essas luzes viraram noticias em todo o " +
        "mundo, estava sendo um sucesso esse brilho no ceu.";
    private string _history02 = " Mais tarde com um novo avanço na tecnologia parcebeu-se que aquelas " +
        "lindas luzes poderiam trazer a destruição da terra, pois de acordo " +
        "estudos feitos, essas luzes nada mais eram do que naves alienigenas " +
        "carregando armas de destruição em massa. Com essas evidências, todas as forças " +
        "mundiais se reuniram para descutir estrategias para defenderem o nosso planeta " +
        "já que essas naves estavam vindo a uma velocidade desconhecida pelo ser humano, " +
        "e rapidamente chegariam na terra, isso no ano 2227.";
    private string _history03 = "Em 2417 com um maior avanço na tecnologia e uma gigantesca aproximação dessas naves, " +
        "era possivel, através de telescopios vizualiza-las. Pelo fato dos seres humanos " +
        "conseguirem ver essas naves foi proposta uma ultima reunião mundial para tratar " +
        "estratégias para a proteção de nosso planeta. No final da reunião veio uma ideia " +
        "que poderia salvar todo o mundo. A ideia foi a seguinte, foi imaginado " +
        "que com a chegada daquelas naves, pela rapidez, todos os nossos sinais de radio " +
        "iriam ser danificados e não haveria como nos comunicar-mos. ";
    private string _history04 = "Iria ser o fim. " +
        "A não ser por um estudo de doutorado de um jovem brasileiro que detectou " +
        "um padrão de 26 cores diferentes entre as naves e dessa maneira utilizando " +
        "o alfabeto, que também contém 26 letras um codigo foi criado para a comunicação. " +
        "Esse codigo funciona da seguinte maneira. Cada nave tem uma cor determinada, e essas cores " +
        "fazem referencia com determinada letra. Há alguns combatentes que são encarregados de apenas " +
        "mandarem sinais para a terra. Você esta encarregado disso.";
    private int whereHistory;
    private bool writing;
    private string _nameSynthesizer = "ScanSoft Raquel_Full_22kHz";
    private string _textStartGame = "O jogo se inicia agora...";


    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _textStart;
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _btnSkip;
    private int _pos = 0;
  
    // Start is called before the first frame update
    void Start()
    {
        _history = _history01;
        whereHistory = 1;

    }


    

    //MenuHistoria.
    IEnumerator writeHistory()
    {
        if (_pos > 0)
        {
            _text.SetText(_text.GetParsedText() + _history[_pos].ToString());
            writing = true;
        }
        else
            _text.SetText(_history[_pos].ToString());
        yield return new WaitForSeconds(0.01f);
        _pos++;
        if (_pos < _history.Length)
            StartCoroutine(writeHistory());
        else
        {
            writing = false;
        }
    }

    public void skip()
    {
        StopAllCoroutines(); 
        _text.SetText("");
        _pos = 0;
        Debug.Log("Parou");
        if (whereHistory < 4)
        {
            if (writing == true)
            {
                writing = false;
                _text.SetText(_history);
            }
            else
            {
                if (whereHistory == 1)
                {
                    _history = _history02;
                }
                else if (whereHistory == 2)
                {
                    _history = _history03;
                }
                else if (whereHistory == 3)
                {
                    _history = _history04;
                }
                whereHistory++;
                StartCoroutine(writeHistory());
            }
        }
        else if (writing == true)
        {
            writing = false;
            _text.SetText(_history);
        }
        else
        {
            _panel.SetActive(false);
            _btnSkip.SetActive(false);
            StartCoroutine(writeStart());
        }
    }


    IEnumerator writeStart()
    {
        if (_pos > 0)
            _textStart.SetText(_textStart.GetParsedText() + _textStartGame[_pos].ToString());
        else
            _textStart.SetText(_textStartGame[_pos].ToString());
        if (_history[_pos].ToString().Equals("."))
            yield return new WaitForSeconds(.5f);
        else
            yield return new WaitForSeconds(0.1f);
        _pos++;
        if (_pos < _textStartGame.Length)
            StartCoroutine(writeStart());
        else
        {
            yield return new WaitForSeconds(2.0f);
            StartCoroutine(cont());
        }
    }

    IEnumerator cont()
    {
        for(int i=3; i>0; i--)
        {
            _textStart.SetText(i.ToString());
            yield return new WaitForSeconds(2.0f);
        }
        _textStart.SetText("Carregando o jogo");
        _pos = 0;
        SceneManager.LoadScene(1);
    }

}
