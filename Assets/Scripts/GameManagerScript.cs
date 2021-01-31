using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{

    public void SceneChange(string name){
        SceneManager.LoadScene(name);

        if(name == "Quit"){
            Application.Quit();
        }
    }
}
