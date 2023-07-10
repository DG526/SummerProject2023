using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFluctuation : MonoBehaviour
{
    public Color[] colors;
    public float[] colorTimes; //How long to stay on each color
    public float lerpTime; //Time spent shifting between colors
    int currentColor = 0;
    float timeOnColor = 0;
    SpriteRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        if(colors.Length > 0)
            rend.color = colors[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(colors.Length == 0 || colorTimes.Length == 0 || colors.Length != colorTimes.Length) return;
        timeOnColor += Time.deltaTime;
        if(timeOnColor >= lerpTime + colorTimes[currentColor])
        {
            timeOnColor -= lerpTime + colorTimes[currentColor];

            currentColor++;
            currentColor %= colors.Length;
        }
        if(timeOnColor > colorTimes[currentColor])
        {
            rend.color = Color.Lerp(colors[currentColor], colors[(currentColor + 1) % colors.Length], (timeOnColor - colorTimes[currentColor]) / lerpTime);
        }
        else
        {
            rend.color = colors[currentColor];
        }
    }
}
