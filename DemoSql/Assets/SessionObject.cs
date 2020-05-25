using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class SessionObject //: MonoBehaviour
{
    public static string LoggedInUserName { get; set; }
    public static string SessionPatientID { get; set; }
    public static Guid SessionPatientID_Guid { get; set; }

    public static Guid userVisitID { get; set; }

    public static UserVisitRequest userVisit { get; set; }
}
