using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript  : MonoBehaviour
{
    public static NewBehaviourScript Instance;

    //public Field alg; // связь с файлом
    public int alg;
    //private Toggle toggle;

    public Toggle is1;
    public Toggle is2;
    public Toggle is3;

    public Toggle ai1;
    public Toggle ai2;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        alg = 0; DataHolder._levelStart = alg;
        //alg.Allgo = 0; 
    }
 
    void Update()
    {
        
    }

    public void StartAl()
    {
        if (is1.isOn == true) { alg = 1;  DataHolder._levelStart = alg; }   // is1
        else if (is2.isOn == true) { alg = 2; DataHolder._levelStart = alg; }   // is2
        else if (is3.isOn == true) { alg = 3; DataHolder._levelStart = alg; }   // is3

        else if (ai1.isOn == true) { alg = 4; DataHolder._levelStart = alg; }   // ai1
        else if (ai2.isOn == true) { alg = 5; DataHolder._levelStart = alg; }   // ai2
        else { alg = 0; DataHolder._levelStart = alg; }

        //alg.allgo1 = 1; // присвоение значения 

    }

}
