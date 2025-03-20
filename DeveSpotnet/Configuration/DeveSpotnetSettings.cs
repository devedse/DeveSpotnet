namespace DeveSpotnet.Configuration
{
    //We could load this from the config but for now keep it simple
    public class DeveSpotnetSettings
    {
        public DeveSpotnetSettings_NzbHandling NzbHandling { get; } = new DeveSpotnetSettings_NzbHandling();
    }

    public class DeveSpotnetSettings_NzbHandling
    {
        public string NzbPrepareAction { get; } = "blahblahgeenzip";
    }
}
