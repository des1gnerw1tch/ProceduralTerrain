using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSelector : MonoBehaviour
{
    [SerializeField] private GameObject[] objsToSelect;

    private void Start()
    {
        int rand = Random.Range(0, objsToSelect.Length);
        objsToSelect[rand].SetActive(true);
    }
}
