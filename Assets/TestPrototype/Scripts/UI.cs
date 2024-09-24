using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UI : MonoBehaviour {
    [SerializeField]
    private NetworkManager _networkManager;

    [SerializeField]
    private TextMeshProUGUI _ipText;

    [SerializeField]
    private TMP_InputField _ipInput;

    private string ipAddress;

    public void Host() {
        _networkManager.StartHost();
        _ipText.text = IpConnection.GetLocalIPAddress();
    }

    public void Client() {
        IpConnection.SetIpAddress(_ipInput.text);
        _networkManager.StartClient();
    }
}