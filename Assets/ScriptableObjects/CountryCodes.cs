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
        countryCodeDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile.text);
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