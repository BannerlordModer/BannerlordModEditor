namespace BannerlordModEditor.Common
{
    /// <summary>
    /// Represents the game version for XML compatibility.
    /// </summary>
    public enum GameVersion
    {
        /// <summary>
        /// Latest/default version (most recent Bannerlord version)
        /// </summary>
        Latest = 0,

        /// <summary>
        /// Bannerlord version 1.2.9
        /// </summary>
        v1_2_9 = 129
    }

    /// <summary>
    /// Extension methods for GameVersion
    /// </summary>
    public static class GameVersionExtensions
    {
        /// <summary>
        /// Gets the display name for the game version
        /// </summary>
        public static string GetDisplayName(this GameVersion version)
        {
            return version switch
            {
                GameVersion.Latest => "Latest",
                GameVersion.v1_2_9 => "v1.2.9",
                _ => version.ToString()
            };
        }

        /// <summary>
        /// Parses a string to GameVersion
        /// </summary>
        public static GameVersion? TryParse(string? versionString)
        {
            if (string.IsNullOrWhiteSpace(versionString))
                return GameVersion.Latest;

            return versionString.ToLowerInvariant() switch
            {
                "latest" or "default" or "" => GameVersion.Latest,
                "1.2.9" or "v1.2.9" or "v1_2_9" or "129" => GameVersion.v1_2_9,
                _ => null
            };
        }
    }
}
