using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using tpopl001.JSON;

public class UserSelect : MonoBehaviour
{
    [SerializeField]private Transform prefabContainer = null;
    [SerializeField]private Text createUser = null;
    private const string USER_PREFAB = "user_prefab";

    #region Initialise
    public static UserSelect instance;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        DisplayUserPrefabs();
    }
    #endregion

    /// <summary>
    /// Visually displays all users from JSON to select
    /// </summary>
    void DisplayUserPrefabs()
    {
        JsonUser[] jsonUsers = UserJson.GetAllUsers();
        if (jsonUsers == null) return;
        for (int i = 0; i < jsonUsers.Length; i++)
        {
            UserPrefab uP = Instantiate<UserPrefab>(Resources.Load<UserPrefab>("Menu/"+ USER_PREFAB));
            uP.Initialise(jsonUsers[i].Username);
            uP.transform.SetParent(this.prefabContainer);
            RectTransform rT = uP.GetComponent<RectTransform>();
            rT.localPosition = new Vector3(rT.localPosition.x, rT.localPosition.y, 0);
            rT.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// Saves the new username to Json file and creates a visual option to select the user
    /// </summary>
    public void AddUser()
    {
        string username = createUser.text;
        if(UserJson.CreateUser(username))
        {
            UserPrefab uP = Instantiate<UserPrefab>(Resources.Load<UserPrefab>("Menu/" + USER_PREFAB));
            uP.Initialise(username);
            uP.transform.SetParent(this.prefabContainer);
            RectTransform rT = uP.GetComponent<RectTransform>();
            rT.localPosition = new Vector3(rT.localPosition.x, rT.localPosition.y, 0);
            rT.localScale = Vector3.one;
        }
    }

}
