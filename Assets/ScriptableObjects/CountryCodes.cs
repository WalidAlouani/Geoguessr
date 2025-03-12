using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CountryCodes", menuName = "ScriptableObjects/CountryCodes")]
public class CountryCodes : ScriptableObject, ICountryCodeLookup
{
    [SerializeField] private TextAsset jsonFile;

    private Dictionary<string, string> countryCodeDictionary;

    private void OnEnable()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        countryCodeDictionary = new Dictionary<string, string>();

        try
        {
            countryCodeDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile.text);

            if (countryCodeDictionary != null)
            {
                // Convert all keys to lowercase for case-insensitive lookup
                Dictionary<string, string> lowercaseDict = new Dictionary<string, string>();
                foreach (var kvp in countryCodeDictionary)
                {
                    lowercaseDict[kvp.Key.ToLower()] = kvp.Value;
                }
                countryCodeDictionary = lowercaseDict;
            }
            else
            {
                Debug.LogError("Failed to deserialize JSON: countryCodeDictionary is null.");
            }
        }
        catch (JsonException ex)
        {
            Debug.LogError($"Error deserializing JSON: {ex.Message}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"An unexpected error occured: {ex.Message}");
        }
    }

    public string GetCountryName(string countryCode)
    {
        if (countryCodeDictionary != null && countryCodeDictionary.TryGetValue(countryCode.ToLower(), out string countryName))
        {
            return countryName;
        }
        return null;
    }
}

public interface ICountryCodeLookup
{
    string GetCountryName(string countryCode);
}