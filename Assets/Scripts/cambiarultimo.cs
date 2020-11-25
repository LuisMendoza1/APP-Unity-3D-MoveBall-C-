using UnityEngine;
using System.Collections;
public class cambiarultimo : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")

            Application.LoadLevel("MainMenu");

    }
    [RPC]
    void Test() { }
}