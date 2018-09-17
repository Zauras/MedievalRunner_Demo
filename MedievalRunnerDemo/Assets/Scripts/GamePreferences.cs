using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePreferences {

    public static string IsTheMusicOn = "IsTheMusicOn";

    // 0 is off - 1 is on
    public static int GetMusicState(){
        return PlayerPrefs.GetInt(IsTheMusicOn);
    }

    // 0 is off - 1 is on
    public static void SetMusicState(int state){
        PlayerPrefs.SetInt(IsTheMusicOn, state);
    }
}
