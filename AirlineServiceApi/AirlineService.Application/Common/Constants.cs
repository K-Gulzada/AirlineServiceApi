namespace AirlineService.Application.Common;

public static class Constants
{
    public static class Cache
    {
        public const string FlightsPrefix = "flights_";
        public const string FlightsPattern = "flights_*";
        public const int DefaultExpirationMinutes = 10;
        public const int DefaultCacheMinutes = 30;
    }

    public static class Pagination
    {
        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 10;
    }

    public static class Policies
    {
        public const string ModeratorOnly = "ModeratorOnly";
    }

    public static class Roles
    {
        public const string Moderator = "Moderator";
    }
}

