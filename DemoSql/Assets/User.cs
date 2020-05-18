using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
[Serializable]
[DataContract]
public class User
{
    [DataMember]
    public int id;

    //[DataMember]
    //public int patientid;

    [DataMember]

    public string name;
    [DataMember]

    public string age;

    [DataMember]
    public string dob;

    [DataMember]
    public string ethnicity;

    [DataMember]
    public string gender;

    [DataMember]
    public string profileimage;
}
[Serializable]
public class GetResponseModel
{
    public List<User> users;
}