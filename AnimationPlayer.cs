using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    private Animator _anim;
    [SerializeField] private byte b;
    private string[] _control;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _control = new string[10];
        
    }

    // Update is called once per frame
    void Update()
    { 



        if ((Input.GetKey(KeyCode.A) && b == 1) || (Input.GetKey(KeyCode.LeftArrow) && b == 2)) 
        {
            _anim.SetBool("Turn_Left", true);
            _anim.SetBool("Turn_Right", false);
        }
        if ((Input.GetKeyUp(KeyCode.A) && b == 1) || (Input.GetKeyUp(KeyCode.LeftArrow) && b == 2))
        {
            _anim.SetBool("Turn_Left", false);
            _anim.SetBool("Turn_Right", false);
        }

        if ((Input.GetKey(KeyCode.D) && b == 1) || (Input.GetKey(KeyCode.RightArrow) && b == 2))
        {
            _anim.SetBool("Turn_Left", false);
            _anim.SetBool("Turn_Right", true);
        }
        if ((Input.GetKeyUp(KeyCode.D) && b == 1) || (Input.GetKeyUp(KeyCode.RightArrow) && b == 2))
        {
            _anim.SetBool("Turn_Left", false);
            _anim.SetBool("Turn_Right", false);
        }
    }
}
