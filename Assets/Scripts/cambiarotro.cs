using UnityEngine;
using System.Collections;
public class cambiarotro : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")

            Application.LoadLevel("Play3");

    }
    [RPC]
    void Test() { }
}