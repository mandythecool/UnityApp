using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using UnityEngine.Networking;

public class LoginClick : MonoBehaviour
{

    public InputField Username;
    public InputField Password;
    public Text messageText;
    RequestModel rq;

    public void Start()
    {
        SessionObject.LoggedInUserName = string.Empty;
        SessionObject.SessionPatientID = string.Empty;
        SessionObject.user = null;
    }
    public void logincall()
    {
        rq = new RequestModel();
        Debug.Log("calling api . . . . .");
        RequestModel mod = new RequestModel();
        mod.username = Username.text;
        mod.password = Password.text;
        try
        {

            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(RequestModel));
            MemoryStream msObj = new MemoryStream();
            js.WriteObject(msObj, mod);
            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);

            string json = sr.ReadToEnd();

            sr.Close();
            msObj.Close();

            StartCoroutine(Post(json));
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }


    IEnumerator Post(string json)
    {
        
        messageText.gameObject.SetActive(false);

        byte[] byteData = System.Text.Encoding.ASCII.GetBytes(json.ToCharArray());

        UnityWebRequest unityWebRequest = new UnityWebRequest("http://localhost:50471/Auth", "POST");
        unityWebRequest.uploadHandler = new UploadHandlerRaw(byteData);
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
            Debug.Log("API Success");
            string response = unityWebRequest.downloadHandler.text;
            Debug.Log("Form upload complete! Status Code: " + unityWebRequest.responseCode);
            if (response.Contains("true"))
            {
                SessionObject.LoggedInUserName = Username.text;
                SceneManager.LoadScene("HomeScene");
            }
            else
            {
                messageText.gameObject.SetActive(true);
                Debug.LogWarning("invalid login");
            }
        }

    }
}
