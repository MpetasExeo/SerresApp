namespace SerresApp.Interfaces
{
    public interface IPlatformSpecificLocationService
    {
        bool IsLocationServiceEnabled();
        bool OpenDeviceLocationSettingsPage();
    }
}
