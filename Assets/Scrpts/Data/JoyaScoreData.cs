using UnityEngine;

[CreateAssetMenu(fileName = "JoyaScoreData", menuName = "Scriptable Objects/JoyaScoreData")]
public class JoyaScoreData : ScriptableObject
{
    private int _goriyakuScore;
    public int Goriyaku => _goriyakuScore;


    public void Initialize()
    {
        _goriyakuScore = 0;
    }

    public void AddGoriyaku(int val)
    {
        _goriyakuScore += val;
    }
}
