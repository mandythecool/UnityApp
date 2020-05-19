using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SimpleJSON;
using System;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Http;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Drawing;

public class crud : MonoBehaviour
{
    public GameObject itemParent, itemGreen, itemOrange, form_edit;
    public Users SceneUsers { get; set; }
    // Start is called before the first frame update
    void Start()
    {

        //Read();
        //call api for data 
        ReadUsers();
    }

    void ReadUsers()
    {
        StartCoroutine(GetUsers());
    }

    IEnumerator GetUsers()
    {

        UnityWebRequest unityWebRequest = new UnityWebRequest("http://localhost:50471/Users/GetUsers", "GET");
        //unityWebRequest.uploadHandler = new UploadHandlerRaw(byteData);
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
            Debug.Log("GetUsers API Success : Status Code: " + unityWebRequest.responseCode);

            string responsejson = unityWebRequest.downloadHandler.text;


            //GetResponseModel lis = JsonUtility.FromJson<GetResponseModel>(bject);
            SceneUsers = new Users();
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(responsejson));
            var ser = new DataContractJsonSerializer(SceneUsers.GetType());
            SceneUsers = ser.ReadObject(ms) as Users;
            ms.Close();

            Debug.Log("Users Retrieved : " + SceneUsers.users.Count);
            LoadTable();
        }

    }

    //IEnumerator AddUser(User user)
    //{
    //    DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(User));
    //    MemoryStream msObj = new MemoryStream();
    //    js.WriteObject(msObj, user);
    //    msObj.Position = 0;
    //    StreamReader sr = new StreamReader(msObj);

    //    // "{\"Description\":\"Share Knowledge\",\"Name\":\"C-sharpcorner\"}"  
    //    string json = sr.ReadToEnd();
    //    byte[] bodyRaw = Encoding.UTF8.GetBytes(json);



    //    UnityWebRequest unityWebRequest = new UnityWebRequest("http://localhost:50471/Users/AddUser", "POST");
    //    unityWebRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
    //    unityWebRequest.SetRequestHeader("Content-Type", "application/json");
    //    unityWebRequest.SetRequestHeader("Accept", "text/csv");
    //    DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();
    //    unityWebRequest.downloadHandler = downloadHandlerBuffer;

    //    yield return unityWebRequest.SendWebRequest();

    //    if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
    //    {
    //        Debug.Log(unityWebRequest.error);
    //    }
    //    else
    //    {
    //        imageData = null;

    //        Debug.Log("GetUsers API Success : Status Code: " + unityWebRequest.responseCode);

    //        string responsejson = unityWebRequest.downloadHandler.text;

    //        form_create.transform.GetChild(1).GetComponent<InputField>().text = "";
    //        form_create.transform.GetChild(2).GetComponent<InputField>().text = "";

    //        ReadUsers();
    //    }

    //}

    IEnumerator UpdateUser(User user)
    {
        DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(User));
        MemoryStream msObj = new MemoryStream();
        js.WriteObject(msObj, user);
        msObj.Position = 0;
        StreamReader sr = new StreamReader(msObj);

        // "{\"Description\":\"Share Knowledge\",\"Name\":\"C-sharpcorner\"}"  
        string json = sr.ReadToEnd();
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);



        UnityWebRequest unityWebRequest = new UnityWebRequest("http://localhost:50471/Users/UpdateUser", "POST");
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

            //     form_create.transform.GetChild(1).GetComponent<InputField>().text = "";
            //     form_create.transform.GetChild(2).GetComponent<InputField>().text = "";

            ReadUsers();
        }
    }


    public void LoadTable()
    {
        int count = SceneUsers.users.Count;

        for (int i = 0; i < itemParent.transform.childCount; i++)
        {
            Destroy(itemParent.transform.GetChild(i).gameObject);
        }
        int number = 0;
        for (int i = 0; i < count; i++)
        {
            number++;
            string id = SceneUsers.users[i].Id.ToString();
            string name = SceneUsers.users[i].Id.ToString();
            string age = SceneUsers.users[i].Id.ToString();
            if (id != "")
            {

                if (int.Parse(SceneUsers.users[i].Age) > 18)
                {
                    Texture2D tex = new Texture2D(1, 1);

                    tex.LoadImage(Convert.FromBase64String(SceneUsers.users[i].Profileimage));
                    GameObject tmp_Item = Instantiate(itemGreen, itemParent.transform);
                    tmp_Item.name = SceneUsers.users[i].Id.ToString();
                    tmp_Item.transform.GetChild(2).GetComponent<Text>().text = SceneUsers.users[i].Id.ToString();
                    tmp_Item.transform.GetChild(3).GetComponent<Text>().text = SceneUsers.users[i].Name.ToString();
                    tmp_Item.transform.GetChild(5).GetComponent<Text>().text = SceneUsers.users[i].Gender.ToString();
                    tmp_Item.transform.GetChild(7).GetComponent<Text>().text = DateTime.Parse(SceneUsers.users[i].Dob).ToShortDateString();
                    tmp_Item.transform.GetChild(9).GetComponent<Text>().text = SceneUsers.users[i].Ethnicity.ToString();

                    tmp_Item.transform.GetChild(10).GetChild(0).GetComponent<RawImage>().texture = tex;
                }
                else
                {
                    Texture2D tex = new Texture2D(1, 1);

                    tex.LoadImage(Convert.FromBase64String(SceneUsers.users[i].Profileimage));
                    GameObject tmp_Item = Instantiate(itemOrange, itemParent.transform);
                    tmp_Item.name = SceneUsers.users[i].Id.ToString();
                    tmp_Item.transform.GetChild(2).GetComponent<Text>().text = SceneUsers.users[i].Id.ToString();
                    tmp_Item.transform.GetChild(3).GetComponent<Text>().text = SceneUsers.users[i].Name.ToString();
                    tmp_Item.transform.GetChild(5).GetComponent<Text>().text = SceneUsers.users[i].Gender.ToString();
                    tmp_Item.transform.GetChild(7).GetComponent<Text>().text = DateTime.Parse(SceneUsers.users[i].Dob).ToShortDateString();
                    tmp_Item.transform.GetChild(9).GetComponent<Text>().text = SceneUsers.users[i].Ethnicity.ToString();

                    tmp_Item.transform.GetChild(10).GetChild(0).GetComponent<RawImage>().texture = tex;
                    //tmp_Item.transform.GetChild(4).GetChild(0).GetComponent<RawImage>().GetComponent<RectTransform>().sizeDelta = new Vector2(2000, 2000);
                    //tmp_Item.transform.GetChild(4).GetChild(0).GetComponent<RawImage>().uvRect = new Rect(161, 30, 100, 100);
                }
            }
            else
                number--;
        }
    }

    //public void Create()
    //{
    //    User newUser = new User()
    //    {
    //        id = Guid.NewGuid(),
    //        name = form_create.transform.GetChild(1).GetComponent<InputField>().text,
    //        age = form_create.transform.GetChild(2).GetComponent<InputField>().text,
    //        dob = DateTime.Parse(form_create.transform.GetChild(3).GetComponent<InputField>().text).ToShortDateString(),
    //        ethnicity = form_create.transform.GetChild(4).GetComponent<InputField>().text,
    //        gender = form_create.transform.GetChild(5).GetComponent<InputField>().text
    //        ,            profileimage = Convert.ToBase64String(imageData)
    //    };
    //    //call api for user creation 

    //    StartCoroutine(AddUser(newUser));

    //}

    public void Delete(GameObject item)
    {
        string id_perf = item.name;
        PlayerPrefs.DeleteKey("id[" + id_perf + "]");
        PlayerPrefs.DeleteKey("name[" + id_perf + "]");
        PlayerPrefs.DeleteKey("age[" + id_perf + "]");
        ReadUsers();
    }

    string id_edit;

    public void open_update_form(GameObject obj_edit)
    {
        form_edit.SetActive(true);
        imageData = null;
        User user = null;
        foreach (User u in SceneUsers.users)
        {
            if (u.Id.ToString() == obj_edit.name)
            {
                user = u;
                break;
            }
        }
        form_edit.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<InputField>().text = user.Name;
        form_edit.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<InputField>().text = user.Age;
        form_edit.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<InputField>().text = user.Dob;
        form_edit.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<InputField>().text = user.Ethnicity;
        form_edit.transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<InputField>().text = user.Gender;
        form_edit.transform.GetChild(0).GetChild(0).GetChild(5).GetComponent<InputField>().text = string.Empty;
        form_edit.transform.GetChild(0).GetChild(0).GetChild(8).GetComponent<Text>().text = user.Id.ToString();

    }

    public void resetFields()
    {
        imageData = null;
        form_edit.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<InputField>().text = string.Empty;
        form_edit.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<InputField>().text = string.Empty;
        form_edit.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<InputField>().text = string.Empty;
        form_edit.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<InputField>().text = string.Empty;
        form_edit.transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<InputField>().text = string.Empty;
        form_edit.transform.GetChild(0).GetChild(0).GetChild(5).GetComponent<InputField>().text = string.Empty;
        form_edit.transform.GetChild(0).GetChild(0).GetChild(8).GetComponent<Text>().text = string.Empty;
        form_edit.SetActive(false);
    }

    public void update_user()
    {

        User u = new User()
        {
            Id = int.Parse(form_edit.transform.GetChild(0).GetChild(0).GetChild(8).GetComponent<Text>().text),
            Name = form_edit.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<InputField>().text,
            Age = form_edit.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<InputField>().text,
            Dob = form_edit.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<InputField>().text,
            Ethnicity = form_edit.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<InputField>().text,
            Gender = form_edit.transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<InputField>().text,
            Profileimage = Convert.ToBase64String(imageData)
        };
        StartCoroutine(UpdateUser(u));

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
            form_edit.transform.GetChild(0).GetChild(0).GetChild(5).GetComponent<InputField>().text = path.ToString();
            var fileContent = File.ReadAllBytes(path);
            imageData = fileContent;
        }

    }
    public byte[] imageData { get; set; }

}
[Serializable]
[DataContract]
public class Users
{
    [DataMember]
    public List<User> users { get; set; }
}
