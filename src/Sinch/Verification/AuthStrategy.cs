namespace Sinch.Verification
{
    public enum AuthStrategy
    {
        /// <summary>
        ///     Utilizes only AppId and AppSecret
        /// </summary>
        Basic,
        
        /// <summary>
        ///     Utilizes <see href="https://developers.sinch.com/docs/verification/api-reference/authentication/signed-request/">Request Signing</see>
        /// </summary>
        ApplicationSign,
    }
}
