using DotNetEnv;
using Sinch;

// Assume .env file is present in your output directory
Env.Load();


var sinch = new SinchClient(Environment.GetEnvironmentVariable("SINCH_PROJECT_ID")!,
    Environment.GetEnvironmentVariable("SINCH_KEY_ID")!,
    Environment.GetEnvironmentVariable("SINCH_KEY_SECRET")!);
_ = sinch.Verification(Environment.GetEnvironmentVariable("SINCH_APP_KEY")!,
    Environment.GetEnvironmentVariable("SINCH_APP_SECRET")!);

Console.ReadLine();
