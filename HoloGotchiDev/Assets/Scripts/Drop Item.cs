using LookingGlass;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] GameObject objectToDuplicate;
    GameObject duplicate;
    [SerializeField] 
    private InterProcessCommunicator communicator;

    /// <summary>
    /// Will run to delete the duplicated object after falling below a certain point
    /// </summary>
    private void Update()
    {
        if (duplicate != null && duplicate.GetComponent<RectTransform>().anchoredPosition.y <= -4000)
        {
            // Sends a message to the Receiver and should be able to be viewed in the debug log
            Debug.Log("Sending IPC message: Drop Item");
            communicator.SendData("Drop " + objectToDuplicate.name);
            Destroy(duplicate);
            duplicate = null;

            // Reinstate the button to continue function
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

        // Ensures that the item is within the canvas positioning plane
        RectTransform environment = GameObject.Find("Environment").GetComponent<RectTransform>();

        duplicate = Instantiate(objectToDuplicate, environment);

        duplicate.transform.SetSiblingIndex(1);

        if (duplicate.GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = duplicate.AddComponent<Rigidbody2D>();
            rb.gravityScale = 20f;
        }

        // Allow the dropping without spamming
        objectToDuplicate.SetActive(false);
    }
}
