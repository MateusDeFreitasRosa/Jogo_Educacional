using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class editText : MonoBehaviour
{
    private Dicionario _dicio;
    // Start is called before the first frame update
    void Start()
    {
        _dicio = GameObject.Find("GameManager").GetComponent<Dicionario>();
        if (_dicio)
        {
            //Debug.Log("Encontrou");
            Text _c;
            _c = this.GetComponent<Text>();
            _c.text = _dicio.whatColor_ForText();
        }
        else
        {
            Debug.Log("Algum problema apareceu!!!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
