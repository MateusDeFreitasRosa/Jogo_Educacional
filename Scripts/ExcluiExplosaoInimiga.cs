using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcluiExplosaoInimiga : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Exclui());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Exclui()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(this.gameObject);
    }
}
