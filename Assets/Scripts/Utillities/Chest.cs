using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject potionPrefab;
    public void OpenChest()
    {
        Instantiate(potionPrefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z), Quaternion.identity);
        Destroy(gameObject);
    }
}
