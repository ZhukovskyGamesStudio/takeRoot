using UnityEngine;

[CreateAssetMenu(fileName = "OutsideLinksConfig", menuName = "Scriptable Objects/OutsideLinksConfig", order = 0)]
public class OutsideLinksConfig : ScriptableObject {
    public string TgLink = "https://t.me/takeroot_pub";

    public string SteamLink;
    public string DiscordLink;
}