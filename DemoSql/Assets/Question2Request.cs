using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question2Request
{
    public HeadachesTime HeadachesTime;
    public HeadachesatNight HeadachesatNight;
    public string PremonitorySymptoms;
    public string MedicationsTaken;
}
public enum HeadachesTime
{
    Morning,
    AfterNoon,
    Evening,
    NoPattern,
    DuringNight
}
public enum HeadachesatNight
{
    Never,
    Occaasionally,
    Often
}