using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Vitals : MonoBehaviour
{
    public GameObject form_Vitals, form_toggle;
    // Start is called before the first frame update
    void Start()
    {
        form_Vitals.transform.GetChild(1).GetComponent<Text>().text = SessionObject.SessionPatientID;
    }

    public void submit()
    {
        Vital vt = new Vital();
        vt.Dia_BP = Decimal.Parse(form_Vitals.transform.GetChild(6).GetComponent<InputField>().text);
        vt.Id = Guid.NewGuid();
        vt.PainLevel = int.Parse(form_Vitals.transform.GetChild(10).GetComponent<Slider>().value.ToString());
        vt.PainLocation = getEnum((form_toggle.transform.GetChild(0).GetComponent<ToggleGroup>().ActiveToggles().FirstOrDefault()).name);
        vt.Pulse = int.Parse(form_Vitals.transform.GetChild(8).GetComponent<InputField>().text);
        vt.Sys_BP = Decimal.Parse(form_Vitals.transform.GetChild(4).GetComponent<InputField>().text);
        vt.UserId = SessionObject.SessionPatientID_Guid;
        vt.VisitId = SessionObject.userVisitID;
        

        StartCoroutine(UpdateVitals(vt));

    }

    public int getEnum(string name)
    {
        int ret = 99;
        switch (name)
        {
            case "front-left":
                ret = (int)PainLocation.FrontLeft;
                break;
            case "front-right":
                ret = (int)PainLocation.FrontRight;

                break;
            case "front-upper-middle":
                ret = (int)PainLocation.FrontUpperMiddle;

                break;
            case "front-lower-middle":
                ret = (int)PainLocation.FrontLowerMiddle;

                break;
            case "back-left":
                ret = (int)PainLocation.BackLeft;

                break;
            case "back-right":
                ret = (int)PainLocation.BackRight;

                break;
            case "back-middle":
                ret = (int)PainLocation.BackUpper;

                break;
            case "back-bottom":
                ret = (int)PainLocation.BackLower;

                break;
            default:

                break;
        }
        return ret;
    }


    IEnumerator UpdateVitals(Vital vital)
    {
        DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Vital));
        MemoryStream msObj = new MemoryStream();
        js.WriteObject(msObj, vital);
        msObj.Position = 0;
        StreamReader sr = new StreamReader(msObj);
        string json = sr.ReadToEnd();
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest unityWebRequest = new UnityWebRequest("http://localhost:50471/Vitals/AddVitals", "POST");
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

            Debug.Log("Vitals Created Success . . . ");

            SceneManager.LoadScene("DashboardScene");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
public class Vital
{
    public Guid Id;
    public Guid UserId;
    public Guid VisitId;
    public decimal Sys_BP;
    public decimal Dia_BP;
    public int Pulse;
    public int PainLevel;
    public int PainLocation;
}
public enum PainLocation
{
    FrontLeft = 0,
    FrontRight,
    FrontUpperMiddle,
    FrontLowerMiddle,
    BackLeft,
    BackRight,
    BackUpper,
    BackLower
}