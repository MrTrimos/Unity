using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algoritm1 : MonoBehaviour
{

    //public int allgo1;
    public Field alg;


    //public GameObject otherGameObject;

    //private Field field;
    
    void Awake()
    {
        //field = otherGameObject.GetComponent<Field>();
        //alg = otherGameObject.GetComponent<Field>();
    }
    // Start is called before the first frame update
    void Start()
    {

        //field.Algoritm = 1;
        //Debug.Log("" + field.Algoritm);
    }

    // Update is called once per frame
    public void AIM()
    {
        alg.Allgo = 1;
    }


}