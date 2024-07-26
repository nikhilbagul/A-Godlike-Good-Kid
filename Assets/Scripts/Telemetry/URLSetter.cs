using UnityEngine;
using System.Collections;

public static class URLSetter
{
    /* 
     * Constant vars: varchar guid, varchar game_version, tinyint is_developer_data
     * Table-dependent: 
        * game_time: float game_time
        * page_time: varchar scene_name, float solve_time, float turn_time, tinyint is_solved
        * new_game_count: none
    */

    private const string domainName = "http://ec2-54-149-126-51.us-west-2.compute.amazonaws.com/";
    private const string gameTimeURL = "game_time.php?";
    private const string pageTimeURL = "page_time.php?";
    private const string newGameCountURL = "new_game_count.php?";
    private static string platform = Application.platform.ToString();

    private static string GenerateConstants() { return "guid=" + GMPersistor.Instance.GetGUID() + "&game_version=" + GameManager._GameVersion + "&platform=" + platform; }

    public static void GameTime(float gameTime, bool isDeveloperData = false)
    {
        int isDeveloperDataINT = isDeveloperData ? 1 : 0;
        string url = domainName + gameTimeURL + GenerateConstants() + "&game_time=" + gameTime + "&is_developer_data=" + isDeveloperDataINT;
        DataPoster.Instance.SendUserData(url);
    }

    public static void PageTime(string sceneName, float solveTime, float turnTime, float totalTime, bool isSolved, bool isDeveloperData = false)
    {
        int isDeveloperDataINT = isDeveloperData ? 1 : 0, isSolvedINT = isSolved ? 1 : 0;
        string url = domainName + pageTimeURL + GenerateConstants() + "&scene_name=" + sceneName + "&solve_time=" + solveTime + "&turn_time=" + turnTime + "&total_time=" + totalTime + "&is_solved=" + isSolvedINT + "&is_developer_data=" + isDeveloperDataINT;
        DataPoster.Instance.SendUserData(url);
    }

    public static void NewGameCount(bool isDeveloperData = false)
    {
        int isDeveloperDataINT = isDeveloperData ? 1 : 0;
        string url = domainName + newGameCountURL + GenerateConstants() + "&is_developer_data=" + isDeveloperDataINT;
        DataPoster.Instance.SendUserData(url);
    }
}
