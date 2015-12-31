using UnityEngine;
using System.Collections;

// Data class used to hold player's stats
public class PlayerStats {

	public int HP;
    public int FirstObjectiveTotal;
    public int SecondObjectiveTotal;
    public int TotalMagicPoints;
    public int TotalTechPoints;
    public int MPointsSpent;
    public int TPointsSpent;
    public int TotalMissilesFired;
    public int TotalSlashes;

    // Gameplay timer variables
    public int TotalTimeUnmars;
    public int TotalTimeIndustrial;

    // Upgrade variables
        // Movement (Player goals)
    private int AAUpgrades;
    private float BonusMoveSpeed;
    private float JumpBonus;
        // Tech (TS goals?)
    private int TSUpgrades;
    private float ScanSpeed;
    private int MissileDamage;

    public PlayerStats()
    {
        HP = 100;
        TotalMagicPoints = 0;
        TotalTechPoints = 0;
        BonusMoveSpeed = 0;
        JumpBonus = 0;
        ScanSpeed = 5;
        MissileDamage = 1;
        AAUpgrades = 0;
        TSUpgrades = 0;
    }

    public void UpgradeAA()
    {
        switch(AAUpgrades)
        {
            case 0:
                UpgradeMoveSpeed(.2f);
                break;
            case 1:
                UpgradeJumpHeight(.5f);
                break;
            default:
                Debug.Log("No more AA upgrades");
                break;
        }
        AAUpgrades++; // Increment for tracking reasons
    }

    // Move speed upgrade
    public float GetMoveSpeed() { return BonusMoveSpeed; }
    public void UpgradeMoveSpeed(float newspeed) { BonusMoveSpeed = newspeed; }
    // Jump height upgrade
    public float GetJumpBonus() { return JumpBonus; }
    public void UpgradeJumpHeight(float newjump) { JumpBonus = newjump; }

    public void UpgradeTS()
    {
        switch (TSUpgrades)
        {
            case 0:
                UpgradeScanSpeed(7f);
                break;
            case 1:
                UpgradeMissileDmg(2);
                break;
            default:
                Debug.Log("No more TS upgrades");
                break;
        }
        TSUpgrades++; // Increment for tracking reasons
    }

    // Scanning speed upgrade
    public float GetScanSpeed() { return ScanSpeed; }
    public void UpgradeScanSpeed(float newspeed) { ScanSpeed = newspeed; }
    // Missile strength upgrade
    public int GetMissileStrength() { return MissileDamage; }
    public void UpgradeMissileDmg(int newdmg) { MissileDamage = newdmg; }
}
