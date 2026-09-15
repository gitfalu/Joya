using UnityEngine;

[CreateAssetMenu(fileName = "JoyaScoreData", menuName = "Scriptable Objects/JoyaScoreData")]
public class JoyaScoreData : ScriptableObject
{
    private int _goriyakuScore;

    public void AddGoriyaku(int val)
    {
        _goriyakuScore += val;
    }
}
