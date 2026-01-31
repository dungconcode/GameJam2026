using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DollType
{
    Normal,
    Abnormal
}
public enum AbnormalTrait
{
    None,
    JumpWhenNeedle,
    HeadMove,
    WaveHand,
    BodyCrack,
    BloodyBody
}

public class DollData : MonoBehaviour
{
    [Header("Truth (Game knows)")]
    public DollType dollType;

    [Header("Traits (Player sees)")]
    public AbnormalTrait abnormalTrait;

    public bool IsAbnormal()
    {
        return dollType == DollType.Abnormal;
    }
}
