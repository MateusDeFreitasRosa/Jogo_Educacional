using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dicionario : MonoBehaviour
{
    public GameObject cube;
    public GameObject text;
    public GameObject bar;
    public Text textBar;
    private int _index=0;
    private int _index2 = 0;
    public List<char> lista;
    private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _mensagem;

    public Color[] colors = new Color[] {Color.black, Color.blue,
        Color.cyan,Color.gray, Color.green,Color.red ,
        Color.magenta, Color.yellow, Color.white};
    


    private void addAndCreateList(List<char> l, int len, string phrase,int dif)
    {
        char f;
        int lenPlusDif = dificultyCalculation(dif, phrase.Length);

        for (int i = 0; i < len; i++)
        {
            l.Add(phrase[i]);
        }
        Debug.Log("Dificuldade: " + lenPlusDif);
        excludeRedundancy(l);
        for (int i = 0; i < lenPlusDif ; i++)
        {
            f = (char)Random.Range(65, 90);
            if (l.Contains(f))
                i--;
            else
                l.Add(f);
        }

       suffle(l,l.Count);

    }

    private void suffle(List<char> lista, int len)
    {
        char aux;
        int pos1, pos2;
        for (int i = 0; i < 15; i++)
        {
            pos1 = Random.Range(0, len);
            pos2 = Random.Range(0, len);
            while (pos2 == pos1)
            {
                pos2 = Random.Range(0, len);
            }

            aux = lista[pos2];
            lista[pos2] = lista[pos1];
            lista[pos1] = aux;
        }
        
    }

    private void suffleForColors(Color[] lista , int len)
    {
        Color aux;
        int pos1, pos2;
        for (int i = 0; i < 15; i++)
        {
            pos1 = Random.Range(0, len);
            pos2 = Random.Range(0, len);
            while (pos2 == pos1)
            {
                pos2 = Random.Range(0, len);
            }

            aux = lista[pos2];
            lista[pos2] = lista[pos1];
            lista[pos1] = aux;
        }

    }


    public void createCubeColorForPhrase(string phrase, int dificulty)
    {
        //Verificando se já existe uma legenda na tela.
        GameObject[] existLegendaCubos = GameObject.FindGameObjectsWithTag("legendCubo");
        GameObject[] existText = GameObject.FindGameObjectsWithTag("text");
        GameObject[] existBar = GameObject.FindGameObjectsWithTag("bar");
        lista.Clear();
        foreach(GameObject obj in existLegendaCubos)
        {
                if (obj)
                {
                    Destroy(obj);
                }
        }
        foreach(GameObject obj in existText)
        {
            if(obj)
            {
                Destroy(obj);
            }
        }

        foreach (GameObject obj in existBar)
        {
            if (obj)
            {
                Destroy(obj);
            }
        }


        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        phrase = phrase.ToUpper();
        new List<char>();
        addAndCreateList(lista, phrase.Length, phrase, dificulty);
        //float x = 6.5f;
        //float y = 3.5f;
        float x = Screen.width * .85f;
        float y = Screen.height * .85f;

        float xlocalBar = Screen.width * .08f;
        float ylocalBar = Screen.height * .05f;

        Rect a;
        a = cube.GetComponent<RectTransform>().rect;

        suffleForColors(colors, colors.Length);

        for (int i=0; i<lista.Count; i++)
        {
            Instantiate(cube, new Vector3(x, y, 1), Quaternion.identity,_canvas.transform);
            Instantiate(text, new Vector3(x, y, 0), Quaternion.identity,_canvas.transform);
            if (i < phrase.Length)
                Instantiate(bar, new Vector3(xlocalBar, ylocalBar, 0), Quaternion.identity, _canvas.transform);
            y -= 21f;
            xlocalBar += 45f;
        }
        _index = 0;
        _index2 = 0;
    }

    private void excludeRedundancy(List<char> phrase)
    {
        int len = phrase.Count;

        for (int i=0; i<len; i++)
        {
            for(int j=i+1; j<phrase.Count; j++)
            {
                if (phrase[i] == phrase[j])
                {
                    phrase.RemoveAt(j);
                } 
            }
        }
    }

    private int dificultyCalculation(int dificult, int len)
    {
        return (len / 4) + dificult;
    }

    public Color whatColor_ForCube()
    {
        return colors[_index++];
    }

    public string whatColor_ForText()
    {
       
        return lista[_index2++].ToString();
    }

    public int lenLista()
    {
        return Random.Range(0, lista.Count);
    }

    public char whatLetter(int a)
    {
        if (lista.Count >= 1)
        {
            return lista[a];
        }
        return 'n';
    }

    public Color whatColor_ForCube(int a)
    {
        return colors[a];
    }

    public void printChar(char c, int pos)
    {
        float xlocalBar = (Screen.width * .08f) + (pos * 45f);
        float ylocalBar = Screen.height * .1f;
        textBar.text = c.ToString();
        Instantiate(textBar, new Vector3(xlocalBar, ylocalBar, 0), Quaternion.identity, _canvas.transform);
    }

    public void restartWord()
    {
        GameObject[] textExist = GameObject.FindGameObjectsWithTag("text1");
        foreach (GameObject obj in textExist)
        {
            if (obj)
            {
                Destroy(obj);
            }
        }
    }

    public void showMensagemMiddleScreen(string menssage)
    {
        _mensagem.SetText(menssage);
        _mensagem.enabled = true;
        StartCoroutine(timeShow());
    }

    IEnumerator timeShow()
    {
        yield return new WaitForSeconds(5.0f);
        _mensagem.enabled = false;
    }

}
