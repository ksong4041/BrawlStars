using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    private string[] Scenes = { "MainMenu", "InGame" };

    private int Index;

    void Start()
    {
        Index = 0;
    }

    public void LoadScene()
    {
        ++Index;
        SceneLoadController.SetScene(Scenes[Index]);
    }
}