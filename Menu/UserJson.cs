using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace tpopl001.JSON
{
    //Handles any JSON interactions
    public static class UserJson
    {
        const string user_path = "/User.json";

        /// <summary>
        /// Create and add a user to the JSON file
        /// </summary>
        public static bool CreateUser(string username)
        {
            if(!IsUsernameTaken(username))
            {
                JsonUser[] jsonUsers = GetAllUsers();
                if (jsonUsers == null) jsonUsers = new JsonUser[0];
                JsonUser[] users = new JsonUser[jsonUsers.Length + 1];
                for (int i = 0; i < jsonUsers.Length; i++)
                {
                    users[i] = jsonUsers[i];
                }
                users[users.Length - 1] = new JsonUser(username, 0, new Level[0]);
                JsonWrapper j = new JsonWrapper();
                j.users = users;
                string json = JsonUtility.ToJson(j, true); 
                File.WriteAllText(Application.streamingAssetsPath + user_path, json);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retrieves a user
        /// </summary>
        public static JsonUser GetUser(string username)
        {
            JsonUser[] jsonUsers = GetAllUsers();
            if (jsonUsers == null) jsonUsers = new JsonUser[0];
            for (int i = 0; i < jsonUsers.Length; i++)
            {
                if(jsonUsers[i].Username.Equals(username))
                {
                    return jsonUsers[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves all the users
        /// </summary>
        public static JsonUser[] GetAllUsers()
        {
            string json = File.ReadAllText(Application.streamingAssetsPath + user_path);
            if (string.IsNullOrEmpty(json))
                return null;
            JsonWrapper jW = JsonUtility.FromJson<JsonWrapper>(json);
            JsonUser[] jsonUser = jW.users;
            return jsonUser;
        }

        /// <summary>
        /// Gets the level data from a user
        /// </summary>
        public static Level GetUserLevelData(string username, string levelName, string levelTitle)
        {
            JsonUser user = GetUser(username);
            if(user.Levels != null)
            {
                for (int i = 0; i < user.Levels.Length; i++)
                {
                    if(user.Levels[i].LevelTitle.Equals(levelTitle) && user.Levels[i].LevelName.Equals(levelName))
                    {
                        return user.Levels[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if the username is already taken
        /// </summary>
        static bool IsUsernameTaken(string username)
        {
            JsonUser[] jsonUsers = GetAllUsers();
            if (jsonUsers == null) jsonUsers = new JsonUser[0];
            for (int i = 0; i < jsonUsers.Length; i++)
            {
                if (jsonUsers[i].Username.Equals(username))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Update level stats to reflect latest stats
        /// </summary>
        public static void ModifyLevelJson(string username, Level level)
        {
            JsonUser[] jsonUsers = GetAllUsers();
            if (jsonUsers == null) jsonUsers = new JsonUser[0];
            for (int i = 0; i < jsonUsers.Length; i++)
            {
                //JsonUser user = jsonUsers[i];
                if (jsonUsers[i].Username.Equals(username))
                {
                    if (jsonUsers[i].Levels != null)
                    {
                        for (int n = 0; n < jsonUsers[i].Levels.Length; n++)
                        {
                            if (jsonUsers[i].Levels[n].LevelTitle.Equals(level.LevelTitle) && jsonUsers[i].Levels[n].LevelName.Equals(level.LevelName))
                            {
                                jsonUsers[i].Levels[n] = level;
                                JsonWrapper jW1 = new JsonWrapper();
                                jW1.users = jsonUsers;
                                string j = JsonUtility.ToJson(jW1, true);
                                File.WriteAllText(Application.streamingAssetsPath + user_path, j);
                                return;
                            }
                        }
                    }

                    //level not found
                    Level[] levels = new Level[jsonUsers[i].Levels.Length + 1];
                    for (int n = 0; n < jsonUsers[i].Levels.Length; n++)
                    {
                        levels[n] = jsonUsers[i].Levels[n];
                    }
                    levels[levels.Length - 1] = level;
                    jsonUsers[i].Levels = levels;
                    jsonUsers[i].Progress = levels.Length;
                    JsonWrapper jW = new JsonWrapper();
                    jW.users = jsonUsers;
                    string json = JsonUtility.ToJson(jW);
                    File.WriteAllText(Application.streamingAssetsPath + user_path, json);
                    return;
                }
            }
        }

        /// <summary>
        /// Remove a user from the JSON file
        /// </summary>
        public static void RemoveUser(string username)
        {
            JsonUser[] jsonUsers = GetAllUsers();
            if (jsonUsers == null) jsonUsers = new JsonUser[0];
            for (int i = 0; i < jsonUsers.Length; i++)
            {
                if (jsonUsers[i].Username.Equals(username))
                {
                    JsonUser[] users = new JsonUser[jsonUsers.Length - 1];
                    int u = 0;
                    for (int n = 0; n < jsonUsers.Length; n++)
                    {
                        if (!jsonUsers[n].Username.Equals(username))
                            users[n-u] = jsonUsers[n];
                        else
                            u++;
                    }

                    return;
                }
            }
        }

    }

    // wrapper is needed to ensure JSON is extracted and taken from the file properly 
    [System.Serializable] public struct JsonWrapper { public JsonUser[] users; }

    //In the following classes the variables have to be public in order for the JSONUtility to convert the JSON file to and from these classes properly
    //They alse require the System.Serializable property
    [System.Serializable]
    public class JsonUser
    {
        public string Username = "";
        public float Progress = 0;
        public Level[] Levels = null;


        public JsonUser(string username, float progress, Level[] levels)
        {
            this.Username = username;
            this.Progress = progress;
            this.Levels = levels;
        }
    }

    [System.Serializable]
    public class Level
    {
        public string LevelTitle;
        public string LevelName;
        public tpopl001.Questing.Stats[] stats;

        public Level(string levelName, string levelTitle, tpopl001.Questing.Stats[] stats)
        {
            this.LevelName = levelName;
            this.LevelTitle = levelTitle;
            this.stats = stats;
        }
    }
}
