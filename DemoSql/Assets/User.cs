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
    public int Id;

    //[DataMember]
    //public int patientid;

    [DataMember]

    public string Name;
    [DataMember]

    public string Age;

    [DataMember]
    public string Dob;

    [DataMember]
    public string Ethnicity;

    [DataMember]
    public string Gender;

    [DataMember]
    public string Profileimage;
}
[Serializable]
public class GetResponseModel
{
    public List<User> users;
}