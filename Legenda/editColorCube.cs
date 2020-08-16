using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class editColorCube : MonoBehaviour
{
    private Dicionario _dicio;
    
    // Start is called before the first frame update
    void Start()
    {
        _dicio = GameObject.Find("GameManager").GetComponent<Dicionario>();
        if (_dicio)
        {
            Debug.Log("Encontrou");
            
            Image p;
            p = this.GetComponent<Image>();
            p.color = _dicio.whatColor_ForCube();
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
