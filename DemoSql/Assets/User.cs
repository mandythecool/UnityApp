using Assets;
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
    public int UserId;

    [DataMember]
    public Guid Id;

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
public class GetUsersResponseModel
{
    public List<User> users;
}

//[Serializable]
//public class VisitRequestModel
//{
   // public UserVisitRequest visit;
//}