using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHP = 100f;
    public float currentHP;

    // LAST STAND
    public static bool inLastStand = false;
    private float lastStandTimeScale = 0.25f;
    private bool mustKillToRevive = false;

    // ADRENALINE
    public static float adrenaline = 0f;
    public float adrenalineToActivate = 100f;
    public static float adrenalineDecayTime = 3f;
    private static float killStreakTimer = 0f;

    public float adrenalineSpeedBoost = 2f;
    public float adrenalineDamageBoost = 2f;
    private bool adrenalineActive = false;

    private FPController controller;

    void Start()
    {
        currentHP = maxHP;
        controller = GetComponent<FPController>();
    }

    void Update()
    {
        // KILL STREAK TIMER
        if (killStreakTimer > 0)
            killStreakTimer -= Time.unscaledDeltaTime;
        else
            adrenaline = Mathf.Max(0, adrenaline - 20f * Time.deltaTime);

        // ACTIVATE ADRENALINE MODE
        if (!adrenalineActive && adrenaline >= adrenalineToActivate)
            ActivateAdrenaline();
    }

    public void TakeDamage(float dmg)
    {
        if (inLastStand) return;

        currentHP -= dmg;

        if (currentHP <= 0)
            EnterLastStand();
    }

    void EnterLastStand()
    {
        inLastStand = true;
        mustKillToRevive = true;
        currentHP = 1;

        Time.timeScale = lastStandTimeScale;
    }

    void ExitLastStand()
    {
        inLastStand = false;
        mustKillToRevive = false;
        currentHP = maxHP * 0.5f; // revive at 50%
        Time.timeScale = 1f;
    }

    public static void OnEnemyKilled()
    {
        // Adrenaline
        killStreakTimer = adrenalineDecayTime;
        adrenaline += 25f;

        // Last Stand revival
        if (inLastStand)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerHealth>().ExitLastStand();
        }
    }

    void ActivateAdrenaline()
    {
        adrenalineActive = true;
        adrenaline = 0;

        controller.moveSpeed *= adrenalineSpeedBoost;
        controller.strafeSpeed *= adrenalineSpeedBoost;

        StartCoroutine(EndAdrenaline());
    }

    System.Collections.IEnumerator EndAdrenaline()
    {
        yield return new WaitForSeconds(5f);
        adrenalineActive = false;

        // Reset speeds to original
        controller.moveSpeed /= adrenalineSpeedBoost;
        controller.strafeSpeed /= adrenalineSpeedBoost;
    }
}

