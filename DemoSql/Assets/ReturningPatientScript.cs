using Assets;
using System;
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

        UnityWebRequest unityWebRequest = new UnityWebRequest("http://localhost:50471/Users/GetUser?id=" + SessionObject.SessionPatientID_Guid.ToString(), "GET");
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

            Debug.Log("User Retrieved ID : " + currentUser.UserId);
            LoadValues();
        }

    }
    public User currentUser;
    public void LoadValues()
    {
        form_patient.transform.GetChild(1).GetComponent<InputField>().text = currentUser.UserId.ToString();
        form_patient.transform.GetChild(15).GetComponent<InputField>().text = currentUser.Name;
        form_patient.transform.GetChild(18).GetComponent<InputField>().text = DateTime.Parse(currentUser.Dob).ToShortDateString();
        form_patient.transform.GetChild(20).GetComponent<InputField>().text = currentUser.Ethnicity;


    }
    public void submit()
    {
        UserVisitRequest user = new UserVisitRequest();
        user.Id = SessionObject.SessionPatientID_Guid;
        user.UserId = int.Parse(form_patient.transform.GetChild(1).GetComponent<InputField>().text);
        user.Date = form_patient.transform.GetChild(9).GetComponent<InputField>().text;
        user.Dob = form_patient.transform.GetChild(18).GetComponent<InputField>().text;
        user.Ethnicity = form_patient.transform.GetChild(20).GetComponent<InputField>().text;
        user.Gender = "-";//form_patient.transform.GetChild(18).GetComponent<InputField>().text;
        user.Name = form_patient.transform.GetChild(15).GetComponent<InputField>().text;
        user.PracticeLocation = form_patient.transform.GetChild(5).GetComponent<InputField>().text;
        user.PracticeName = form_patient.transform.GetChild(3).GetComponent<InputField>().text;
        user.ReadingTaker = form_patient.transform.GetChild(7).GetComponent<InputField>().text;
        user.Time = form_patient.transform.GetChild(11).GetComponent<InputField>().text;

        DateTime uDate = DateTime.Parse(user.Date);
        DateTime date = new DateTime(uDate.Year, uDate.Month, uDate.Day, DateTime.Parse(user.Time).Hour, DateTime.Parse(user.Time).Minute, DateTime.Parse(user.Time).Second);
        user.Date = date.ToShortDateString() + " " + date.ToLongTimeString();

        StartCoroutine(CreateUserVisit(user));
    }

    IEnumerator CreateUserVisit(UserVisitRequest user)
    {
        DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(UserVisitRequest));
        MemoryStream msObj = new MemoryStream();
        js.WriteObject(msObj, user);
        msObj.Position = 0;
        StreamReader sr = new StreamReader(msObj);
        string json = sr.ReadToEnd();
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest unityWebRequest = new UnityWebRequest("http://localhost:50471/Users/CreateUserVisit", "POST");
        unityWebRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        unityWebRequest.SetRequestHeader("Content-Type", "application/json");
        unityWebRequest.SetRequestHeader("Accept", "text/csv");
        DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();
        unityWebRequest.downloadHandler = downloadHandlerBuffer;
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
        {
            Debug.Log(unityWebRequest.error);
        }
        else
        {
            Debug.Log("GetUsers API Success : Status Code: " + unityWebRequest.responseCode);

            string responsejson = unityWebRequest.downloadHandler.text;

            Debug.Log("Visit Created Success . . . ");
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
