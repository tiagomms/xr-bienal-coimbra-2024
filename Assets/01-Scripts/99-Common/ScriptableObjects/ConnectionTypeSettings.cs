using UnityEngine;

public enum ConnectionType
{
    Standard, ForceQuestEvenInUnityEditor
}

[CreateAssetMenu(fileName = "Platform Used Settings", menuName = "Settings/Platform Used Settings")]
public class ConnectionTypeSettings : ScriptableObject
{
    [Tooltip("Standard - if on Editor Mode will go to Simulator; on Build - Quest; ForceQuest will always build for Quest")] 
    public ConnectionType type;

    public static ConnectionTypeSettings CreateSimulatorInstance()
    {
        ConnectionTypeSettings connectionTypeSettings = ScriptableObject.CreateInstance<ConnectionTypeSettings>();
        connectionTypeSettings.type = ConnectionType.Standard;

        return connectionTypeSettings;
    }
    
}
