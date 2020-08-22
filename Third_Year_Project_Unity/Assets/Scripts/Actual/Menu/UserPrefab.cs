using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpopl001.JSON;
using UnityEngine.UI;
using tpopl001.Menu;

public class UserPrefab : MonoBehaviour
{
    private string username = "";
    [SerializeField] private Text text = null;

    /// <summary>
    /// Sets the initial values
    /// </summary>
    /// <param name="username">The user to contain</param>
    public void Initialise(string username)
    {
        this.username = username;
        text.text = username;
    }

    /// <summary>
    /// Destroys this gameobject and attempts to remove the cached user from the JSON file
    /// </summary>
    public void RemoveUser()
    {
        UserJson.RemoveUser(username);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Closes the window and starts the users session
    /// </summary>
    public void SelectUser()
    {
        StaticLevel.username = username;
        MainMenu mainMenu = transform.parent.parent.parent.GetComponent<MainMenu>();
        mainMenu.UpdateWelcomeText();
        transform.parent.parent.gameObject.SetActive(false);
        transform.parent.parent.parent.Find("Menu").gameObject.SetActive(true);
    }
}
