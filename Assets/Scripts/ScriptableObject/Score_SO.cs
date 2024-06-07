using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Score", menuName = "ScriptableObjects/Score", order = 1)]
public class Score_SO : ScriptableObject
{
    public int Score = 0;
    public int HighestScore = 0;
}
