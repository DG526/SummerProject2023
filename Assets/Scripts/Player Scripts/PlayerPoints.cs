using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    public int points = 0;

    public void AddPoints(int val) { points += val; }

    public int GetPoints() { return points; }

    public void RemovePoints(int val) { points -= val; }

    public void ResetPoints() { points = 0; }
}
