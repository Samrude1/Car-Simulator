using UnityEngine;

public class ButtonDebug : MonoBehaviour
{
    void Update()
    {
        for (int i = 0; i <= 19; i++) // Testaa 20 nappia
        {
            if (Input.GetKeyDown("joystick button " + i))
            {
                Debug.Log("Pressed: joystick button " + i);
            }
        }
    }
}
