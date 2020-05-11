using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
[DataContract]
public class RequestModel
{
    [DataMember]
    public string username { get; set; }
    [DataMember]

    public string password { get; set; }
}