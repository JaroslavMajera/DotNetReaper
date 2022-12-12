// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

var processStartInfo = new ProcessStartInfo()
{
    RedirectStandardError = true,
    RedirectStandardOutput = true,
    CreateNoWindow = true,
    UseShellExecute = false,
    FileName = "chromium",
    Arguments = string.Join(" ", new List<string>
    {
                "--remote-debugging-port=20000",
                $"--user-data-dir=\"/tmp\"",
                "--headless",
                "--disable-gpu",
                "--bwsi",
                "--no-first-run",
                "--enable-logging",
                "--v=1",
                "--disable-default-apps",
                "--disable-extensions",
                "--disable-sync",
                "--disable-background-networking",
                "--mute-audio",
                "--no-sandbox",  
            }),
};

while (true)
{
    var process = new Process();
    process.StartInfo = processStartInfo;
    process.EnableRaisingEvents = true;
    process.ErrorDataReceived += Process_ErrorDataReceived;
    process.OutputDataReceived += Process_OutputDataReceived;
    process.Start();
    process.BeginOutputReadLine();
    process.BeginErrorReadLine();
    await Task.Delay(300);
    Console.WriteLine("start dispose");
    try
    {
        if (!process.HasExited)
        {
            Console.WriteLine("kill");
            process.Kill();
            Console.WriteLine("wait");
            process.WaitForExit();
        }
        Console.WriteLine("dispose");
        process.Dispose();
        Console.WriteLine("end dispose");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        throw;
    }
}

void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
{
    Console.WriteLine($"Data {e.Data}");
}

void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
{
    Console.WriteLine($"Error {e.Data}");
}