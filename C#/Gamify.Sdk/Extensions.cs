namespace Gamify.Sdk
{
    public static class Extensions
    {
        public static ISessionGamePlayerBase ToBase<TMove, UResponse>(this SessionGamePlayer<TMove, UResponse> sessionGamePlayer)
        {
            return sessionGamePlayer;
        }
    }
}
