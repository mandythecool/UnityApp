using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturningPatientScript : MonoBehaviour
{

    public GameObject form_patient;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void submit()
    {
        UserVisitRequest user = new UserVisitRequest();

        user.Id = int.Parse(form_patient.transform.GetChild(1).GetComponent<InputField>().text);


    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
