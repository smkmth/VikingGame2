using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{

    public float currentStamina;
    public float MaxStamina;

    public float StaminaDecayRate;
    public bool canLoseStamina = false;
    public int timeToWakeUp;
    public GameObject fadeToBlackCanvas;

    private Combat stats;


    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Combat>();
        currentStamina = MaxStamina;

    }

    // Update is called once per frame
    void Update()
    {
        if (canLoseStamina)
        {
            if (currentStamina > 0)
            {
                currentStamina -= StaminaDecayRate * Time.deltaTime;

            }
            else
            {
                StartCoroutine(PassOut());
            }

        }
    }

    public IEnumerator PassOut()
    {
        fadeToBlackCanvas.SetActive(true);
        canLoseStamina = false;
        TimeManager.TimeManagerInstance.JumpForwardInTime(timeToWakeUp);
        currentStamina = MaxStamina;
        transform.position = stats.startPos;
        yield return new WaitForSeconds(3);
        canLoseStamina = true;
        fadeToBlackCanvas.SetActive(false);

        yield return null;
    }


    public void RestoreStamina(float amount)
    {
        currentStamina += amount;
        if (currentStamina > 100)
        {
            currentStamina = 100;
        }

    }
}
