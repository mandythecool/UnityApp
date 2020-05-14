using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewPatient : MonoBehaviour
{
    public GameObject form_create,form_confirm;
    // Start is called before the first frame update
    void Start()
    {

        //Read();
        //call api for data 
    }

    IEnumerator AddUser(User user)
    {
        DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(User));
        MemoryStream msObj = new MemoryStream();
        js.WriteObject(msObj, user);
        msObj.Position = 0;
        StreamReader sr = new StreamReader(msObj);

        // "{\"Description\":\"Share Knowledge\",\"Name\":\"C-sharpcorner\"}"  
        string json = sr.ReadToEnd();
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);



        UnityWebRequest unityWebRequest = new UnityWebRequest("http://localhost:52324/User/AddUser", "POST");
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
            imageData = null;

            Debug.Log("GetUsers API Success : Status Code: " + unityWebRequest.responseCode);

            string responsejson = unityWebRequest.downloadHandler.text;
            form_confirm.SetActive(true);
//            SceneManager.LoadScene("DashboardScene");

        }

    }

    
    public void Create()
    {
        User newUser = new User()
        {
            id = Guid.NewGuid(),
            name = form_create.transform.GetChild(0).GetComponent<InputField>().text,
            age = form_create.transform.GetChild(1).GetComponent<InputField>().text,
            dob = DateTime.Parse(form_create.transform.GetChild(2).GetComponent<InputField>().text).ToShortDateString(),
            ethnicity = form_create.transform.GetChild(3).GetComponent<InputField>().text,
            gender = form_create.transform.GetChild(4).GetComponent<InputField>().text
            ,
            profileimage = Convert.ToBase64String(imageData)
        };
        //call api for user creation 

        StartCoroutine(AddUser(newUser));

    }

    
    // Update is called once per frame
    void Update()
    {

    }


    public void LogOut()
    {
        Debug.Log("Logged Out");
        SceneManager.LoadScene("LoginScene");
    }

    public void UploadImage()
    {
        string path = EditorUtility.OpenFilePanel("Upload Profile Image", "", "*");
        List<string> supportedTypes = new List<string>() { ".jpg", ".png", ".bmp" };
        while (!supportedTypes.Contains(Path.GetExtension(path)))
        {
            path = EditorUtility.OpenFilePanel("Upload Profile Image", "", "*");
        }

        if (path.Length != 0)
        {
            form_create.transform.GetChild(5).GetComponent<InputField>().text = path.ToString();
            var fileContent = File.ReadAllBytes(path);
            imageData = fileContent;
            //  texture.LoadImage(fileContent);
        }

    }
    public byte[] imageData { get; set; }

}
