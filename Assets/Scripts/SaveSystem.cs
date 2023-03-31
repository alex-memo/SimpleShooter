using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    private static string filePath = Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + ".json";
    private const string encryptionKey = "MyEncryptionKey123";

    /// <summary>
    /// Save the player position using JSON and XOR encryption
    /// </summary>
    public static void SavePlayerPosition()
    {
        var _playerPosition = new PlayerPositionData
        {
            X = UserController.Instance.transform.position.x,
            Y = UserController.Instance.transform.position.y,
            Z = UserController.Instance.transform.position.z
        };
        string _jsonString = JsonUtility.ToJson(_playerPosition);
        byte[] _encryptedBytes = XOR_Encrypt(Encoding.ASCII.GetBytes(_jsonString), encryptionKey);
        File.WriteAllBytes(filePath, _encryptedBytes);
    }

    /// <summary>
    /// Load the player position using JSON and XOR encryption
    /// </summary>
    public static void LoadPlayerPosition()
    {
        if (File.Exists(filePath))
        {
            byte[] _encryptedBytes = File.ReadAllBytes(filePath);
            string _decryptedString = Encoding.ASCII.GetString(XOR_Decrypt(_encryptedBytes, encryptionKey));
            var _playerPosition = JsonUtility.FromJson<PlayerPositionData>(_decryptedString);
            UserController.Instance.transform.position = new Vector3(_playerPosition.X, _playerPosition.Y, _playerPosition.Z);
        }
    }
    /// <summary>
    /// XOR encryption method
    /// </summary>
    /// <param name="_inputBytes"></param>
    /// <param name="_key"></param>
    /// <returns></returns>
    private static byte[] XOR_Encrypt(byte[] _inputBytes, string _key)
    {
        byte[] _outputBytes = new byte[_inputBytes.Length];
        for (int i = 0; i < _inputBytes.Length; i++)
        {
            _outputBytes[i] = (byte)(_inputBytes[i] ^ _key[i % _key.Length]);
        }
        return _outputBytes;
    }

    /// <summary>
    /// XOR decryption method
    /// </summary>
    /// <param name="_inputBytes"></param>
    /// <param name="_key"></param>
    /// <returns></returns>
    private static byte[] XOR_Decrypt(byte[] _inputBytes, string _key)
    {
        return XOR_Encrypt(_inputBytes, _key);
    }

    /// <summary>
    /// Class to store player position data
    /// </summary>
    private class PlayerPositionData
    {
        public float X;
        public float Y;
        public float Z;
    }
}