using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onClick() {
        Scenemanager.Instance.LoadMainMenu();
    }
    public void onURLCLick() {
        Application.OpenURL("https://youtube.com/user/r8teful");
    }
}
