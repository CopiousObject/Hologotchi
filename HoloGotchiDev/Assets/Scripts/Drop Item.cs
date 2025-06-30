using LookingGlass;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] GameObject objectToDuplicate;
    GameObject duplicate;
    [SerializeField] private InterProcessCommunicator communicator;

    /// <summary>
    /// Will run to delete the duplicated object after falling below a certain point
    /// </summary>
    private void Update()
    {
        if (duplicate != null && duplicate.GetComponent<RectTransform>().anchoredPosition.y <= -4000)
        {
            Debug.Log("Sending IPC message: Drop Item");
            communicator.SendData("Drop Item");
            Destroy(duplicate);
            duplicate = null;

            objectToDuplicate.SetActive(true);
        }
    }

    /// <summary>
    /// Duplicates this GameObject and then drops it from the scene
    /// </summary>
    public void Drop()
    {
        if (duplicate != null)
            return;

        RectTransform environment = GameObject.Find("Environment").GetComponent<RectTransform>();

        duplicate = Instantiate(objectToDuplicate, environment);

        duplicate.transform.SetSiblingIndex(1);

        if (duplicate.GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = duplicate.AddComponent<Rigidbody2D>();
            rb.gravityScale = 20f;
        }

        objectToDuplicate.SetActive(false);
    }
}
