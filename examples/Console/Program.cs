using DotNetEnv;
using Sinch;

// Assume .env file is present in your output directory
Env.Load();


var sinch = new SinchClient(new SinchClientConfiguration()
{
    SinchCommonCredentials = new SinchCommonCredentials()
    {
        ProjectId = Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")!,
        KeyId = Environment.GetEnvironmentVariable("SINCH_KEY_ID")!,
        KeySecret = Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")!
    }
});
_ = sinch.Verification(Environment.GetEnvironmentVariable("SINCH_APP_KEY")!,
    Environment.GetEnvironmentVariable("SINCH_APP_SECRET")!);

Console.ReadLine();
