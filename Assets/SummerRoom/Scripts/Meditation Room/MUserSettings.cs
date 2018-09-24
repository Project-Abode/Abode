using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MUserSettings
{
    private static int time = -1; //in seconds.
    private static string environ = "ocean";

    public static int getTime() {
        return time;
    }

    public static void setTime(int newTime)
    {
        time = newTime;
    }

    public static string getEnviron()
    {
        return environ;
    }

    public static void setEnviron(string newEnviron)
    {
        environ = newEnviron;
    }
}
