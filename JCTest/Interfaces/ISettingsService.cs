namespace JCTest.Interfaces
{
    public interface ISettingsService
    {
        string Get(string name, string defaultValue = null);
        bool GetBoolean(string name, bool defaultValue = false);
        int GetNumeric(string name, int defaultValue = 0);
    }
}