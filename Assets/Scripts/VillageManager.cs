using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour {

    TimeManager time;
    public List<string> CurrentCharacters;
    public List<Building> currentBuildings;

    private void Start()
    {
        time = GetComponent<TimeManager>();
    }

    public VillageInfo RequestVillageInfo()
    {
        VillageInfo villageInfo = new VillageInfo( time.currentHour, time.currentDay, CurrentCharacters );

        return villageInfo;
    }

    public void AttackVillage()
    {
        foreach(Building building in currentBuildings)
        {
            if (building.riskFactor > 10.0f)
            {
                building.UpdateBuilding(buildingState.destroyed);

            }
        }
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
