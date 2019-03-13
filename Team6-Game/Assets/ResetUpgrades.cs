using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetUpgrades : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Pulls current values for weapons --Trevor
        PlayerPrefs.SetInt("PrimaryGun", 0);
        PlayerPrefs.SetInt("SpecialGun", 0);

        // Pulls current upgrade strength for each weapon --Trevor
        PlayerPrefs.SetInt("ShotgunStrength", 0);
        PlayerPrefs.SetInt("HomingStrength", 0);
        PlayerPrefs.SetInt("MachineStrength", 0);
        PlayerPrefs.SetInt("ChargeStrength", 0);
        PlayerPrefs.SetInt("LazerStrength", 0);
        PlayerPrefs.SetInt("BulletStrength", 1);
        PlayerPrefs.SetInt("FireRateStrength", 1);
    }
}
