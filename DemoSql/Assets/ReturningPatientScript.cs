using Assets;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ReturningPatientScript : MonoBehaviour
{

    public GameObject form_patient;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(GetUser());

        
    }

    IEnumerator GetUser()
    {

        UnityWebRequest unityWebRequest = new UnityWebRequest("http://localhost:50471/Users/GetUser?id=" + SessionObject.SessionPatientID.ToString(), "GET");
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");
        DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();
        unityWebRequest.downloadHandler = downloadHandlerBuffer;
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
        }
        else
        {
            Debug.Log("GetUser API Success : Status Code: " + unityWebRequest.responseCode);

            string responsejson = unityWebRequest.downloadHandler.text;


            //GetResponseModel lis = JsonUtility.FromJson<GetResponseModel>(bject);
            currentUser = new User();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(responsejson));
            var ser = new DataContractJsonSerializer(currentUser.GetType());
            currentUser = ser.ReadObject(ms) as User;
            ms.Close();

            Debug.Log("User Retrieved ID : " + currentUser.Id);
            LoadValues();
        }

    }
    public User currentUser;
    public void LoadValues()
    {
        form_patient.transform.GetChild(1).GetComponent<InputField>().text = currentUser.Id.ToString();
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
