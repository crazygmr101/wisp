namespace WispBot.Libs
{
    public class Utils
    {
        int GetTimestampFromSnowflake(int snowflake) => (snowflake >> 22);
    }
}