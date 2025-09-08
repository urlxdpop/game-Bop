using UnityEngine;

[CreateAssetMenu(fileName = "SecretsData", menuName = "ScriptableObjects/SecretsData", order = 1)]
public class SecretsData : ScriptableObject
{
    public Vector3[] SecretWayPosition = new Vector3[100];
    public bool[] IsSecretOpen = new bool[100];
    public int[] NumSecretsInLevel = new int[100];
    

}
