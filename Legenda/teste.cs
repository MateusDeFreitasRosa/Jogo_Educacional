using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class teste : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    private string _phrase = "Olá amigos da rede globo.";
    private string _auxPhrase = "";
    private int _pos = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(write());
    }
    
    IEnumerator write()
    {
        if (_pos > 0)
            _text.SetText(_text.GetParsedText() + _phrase[_pos].ToString());
        else
            _text.SetText(_phrase[_pos].ToString());
        yield return new WaitForSeconds(0.1f);
        _pos++;
        if (_pos < _phrase.Length)
            StartCoroutine(write());
    }

}
