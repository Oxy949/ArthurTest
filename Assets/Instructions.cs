using UnityEngine;

[ExecuteInEditMode]
public class Instructions : MonoBehaviour
{

    private static bool isFolded = false;
    private static string instructionButtonLable = "Hide Instructions";
    private void OnGUI()
    {
        GUILayout.BeginVertical("box");

        if (!isFolded)
        {
            GUILayout.Label("> Interaction Points on X-Y-Z axis for stretching the Shape in three dimensions individually.");
            GUILayout.Label("> If Clicked on the point Object should stretch in that direction only.");
            GUILayout.Label("> If Clicked on the Object itself and Dragged, the object should rotate around its pivot in the direction the mouse/controller is moved.");
            GUILayout.Label("# Record a 30 sec or less video/gif of your submission and share it along with your solution .");
        }

        
        if (GUILayout.Button(instructionButtonLable, GUILayout.Width(Screen.width)))
        {
            isFolded = !isFolded;

            instructionButtonLable = isFolded ? "Show Instructions" : "Hide Instructions";
        }

        GUILayout.EndVertical();
        
    }
}
