using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserGuide : MonoBehaviour
{
    public void Guidebtn()
    {
        SceneManager.LoadScene("User Guide sousahouhou");//スタート画面から操作方法画面へと移る
    }
}
