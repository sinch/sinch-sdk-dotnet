using DotNetEnv;
using Sinch;
using Sinch.Verification;

// Assume .env file is present in your output directory
Env.Load();


var sinch = new SinchClient(new SinchClientConfiguration()
{
    SinchCommonCredentials = new SinchCommonCredentials()
    {
        ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")!,
        KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID")!,
        KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")!
    },
    VerificationConfiguration = new SinchVerificationConfiguration()
    {
        AppKey = Environment.GetEnvironmentVariable("SINCH_APP_KEY")!,
        AppSecret = Environment.GetEnvironmentVariable("SINCH_APP_KEY_SECRET")!
    }
});

Console.ReadLine();
