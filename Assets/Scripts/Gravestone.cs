using AI;
using Unity.Netcode;
using UnityEngine;

public class Gravestone : NetworkBehaviour
{
    public void SetData(AI.SettlerData settlerData, DeathCause cause) {
        //TODO add caching death cause
    }
}
