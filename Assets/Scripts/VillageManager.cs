using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour {

    TimeManager time;
    public List<string> CurrentCharacters;

    private void Start()
    {
        time = GetComponent<TimeManager>();
    }

    public VillageInfo RequestVillageInfo()
    {
        VillageInfo villageInfo = new VillageInfo( time.currentHour, time.currentDay, CurrentCharacters );

        return villageInfo;
    }
}

public class VillageInfo
{
    public int CurrentHour;
    public int CurrentDay;
    public List<string> Characters;

    public VillageInfo(int currentHour, int currentDay, List<string> characters)
    {
        CurrentHour = currentHour;
        CurrentDay = currentDay;
        Characters = characters;
    }
}
