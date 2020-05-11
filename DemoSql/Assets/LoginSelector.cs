using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoginSelector : MonoBehaviour
{
    public GameObject userObject,itemParent,otherObject;

    // Start is called before the first frame update
    void Start()
    {
        LoadTable();
    }

    byte[] ReadFile(string sPath)
    {
        //Initialize byte array with a null value initially.
        byte[] data = null;

        //Use FileInfo object to get file size.
        FileInfo fInfo = new FileInfo(sPath);
        long numBytes = fInfo.Length;

        //Open FileStream to read file
        FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);

        //Use BinaryReader to read file stream into byte array.
        BinaryReader br = new BinaryReader(fStream);

        //When you use BinaryReader, you need to supply number of bytes 
        //to read from file.
        //In this case we want to read entire file. 
        //So supplying total number of bytes.
        data = br.ReadBytes((int)numBytes);

        return data;
    }

    public void LoadTable()
    {
        int count = itemParent.transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Destroy(itemParent.transform.GetChild(i).gameObject);
        }
        int number = 0;
        for (int i = 0; i < 1; i++)
        {
            try
            {
                number++;
                if (true) //id != "")
                {
                    GameObject tmp_Item = Instantiate(userObject, itemParent.transform);
                    byte[] imageData = ReadFile(@"C:\\Users\\mandy\\Documents\\Unity Work\\download.jpg");
                    Texture2D tex = new Texture2D(300, 300);
                    tex.LoadImage(imageData);

                    tmp_Item.transform.GetChild(1).GetChild(0).GetComponent<RawImage>().texture = tex;
                    tmp_Item.transform.GetChild(0).GetComponent<Text>().text = "Sample User";
                }
                else
                    number--;
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
            }

        }
        GameObject tmp_Item1 = Instantiate(otherObject, itemParent.transform);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
