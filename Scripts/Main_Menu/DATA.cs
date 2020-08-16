using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DATA : MonoBehaviour
{
    [SerializeField] private bool _isCoOpMode;

    public void gameModeCoOp(bool mode)
    {
        _isCoOpMode = mode;
    }
    
    public bool gameModeCoOp()
    {
        return _isCoOpMode;
    }
}
