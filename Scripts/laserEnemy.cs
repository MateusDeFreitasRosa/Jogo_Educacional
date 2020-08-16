using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserEnemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private GameManager _gameManager;
    private bool _eraser;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager._gamerStarted == false)
            _eraser = true;
    }

    // Update is called once per frame
    void Update()
    {
        Disparo();
        Destroilaser();
        if (_eraser == true && _gameManager._gamerStarted == true)
            Destroy(this.gameObject);

        if (_gameManager.playerAlive == false && _eraser == false)
            _eraser = true;
    }

    private void Disparo()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void Destroilaser()
    {
        if (transform.position.y <= -6)
            Destroy(this.gameObject);
    }
}
