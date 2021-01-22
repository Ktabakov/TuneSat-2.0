using System;
using System.Linq;
using System.Diagnostics;
using System.IO;
using ScreenTest;
using WindowsInput;
using WindowsInput.Native;
using VideoLibrary;

namespace appTry
{
    class Program
    {
        static DirectoryInfo di;
        static void Main(string[] args)
        {
            Console.Title = ("Capture App");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("Input number of ID's!");
            int n = int.Parse(Console.ReadLine());

            string[] arrId = new string[n];
            string[] arrURL = new string[n];
            string[] arrName = new string[n];
            string[] fullURLS = new string[n];

            DateTime dateToday = DateTime.Now;

            Console.WriteLine("Input ID's!");
            for (int i = 0; i < n; i++)
            {
                arrId[i] = Console.ReadLine();
            }
            Console.WriteLine("Input URL's!");
            for (int i = 0; i < n; i++)
            {
                string url = Console.ReadLine();
                fullURLS[i] = url;
                string urlToUse = string.Empty;
                string secLast = string.Empty;
                if (url.Contains("www.youtube.com"))
                {

                    string[] urlArr = url.Split("watch?v=").ToArray();
                    urlToUse = urlArr.Last();
                    if (urlToUse.Contains('/'))
                    {
                        urlToUse = urlToUse.Remove(urlToUse.Length - 1);
                    }
                }
                else if (url.Contains("www.facebook.com"))
                {
                    string[] urlArr = url.Split("videos", StringSplitOptions.RemoveEmptyEntries).ToArray();
                    string[] deleteChar = urlArr[1].Split('/').ToArray();
                    urlToUse = deleteChar[1];
                }
                else if (url.Contains("www.twitter.com"))
                {
                    string[] urlArr = url.Split("status", StringSplitOptions.RemoveEmptyEntries).ToArray();
                    string[] deleteChar = urlArr[1].Split('/').ToArray();
                    urlToUse = deleteChar[1];
                    if (urlToUse.Contains('/'))
                    {
                        urlToUse = urlToUse.Remove(urlToUse.Length - 1);
                    }
                }
                else if (url.Contains("www.instagram.com"))
                {
                    string[] urlArr = url.Split("http://www.instagram.com/p/").ToArray();
                    urlToUse = urlArr[1];
                    if (urlToUse.Contains('/'))
                    {
                        urlToUse = urlToUse.Remove(urlToUse.Length - 1);
                    }
                }
                else if (url.Contains("www.tiktok.com"))
                {
                    string[] urlArr = url.Split("video", StringSplitOptions.RemoveEmptyEntries).ToArray();
                    string[] deleteChar = urlArr[1].Split('/').ToArray();
                    urlToUse = deleteChar[1];
                }
                arrURL[i] = urlToUse;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Picture Titles are:");
            for (int i = 0; i < arrId.Length; i++)
            {
                Console.WriteLine($"{arrId[i]}_{arrURL[i]}");
            }
            Console.WriteLine("OBS Titles are:");
            for (int i = 0; i < arrId.Length; i++)
            {
                Console.WriteLine($"{arrId[i]}_{arrURL[i]}_{dateToday.ToString("dd-MM-yyyy")}");
            }
            Console.WriteLine("Video Titles are:");
            for (int i = 0; i < arrId.Length; i++)
            {
                Console.WriteLine($"{arrId[i]}_{arrURL[i]}_VC");
            }

            Console.WriteLine("Press Enter to Start with Screenshots!");
            string pressOk = Console.ReadLine();

            for (int i = 0; i < fullURLS.Length; i++)
            {
                var prs = new ProcessStartInfo(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe");
                prs.Arguments = $"{fullURLS[i]}" + " --new-window";
                try
                {
                    Process.Start(prs);
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                System.Threading.Thread.Sleep(9000);

                if (fullURLS[i].Contains("youtube"))
                {
                    for (int pressTab = 0; pressTab < 15; pressTab++)
                    {
                        InputSimulator tab = new InputSimulator();
                        tab.Keyboard.KeyPress(VirtualKeyCode.TAB);
                    }

                    InputSimulator down = new InputSimulator();
                    down.Keyboard.KeyPress(VirtualKeyCode.DOWN);
                    down.Keyboard.KeyPress(VirtualKeyCode.DOWN);

                    System.Threading.Thread.Sleep(1000);
                }

                args = new string[] { "y" };

                if (args[0] == "y")
                {

                    di = new DirectoryInfo("C:\\CA_Screenshots");
                    if (!di.Exists) { di.Create(); }

                    PrintScreen ps = new PrintScreen();
                    ps.CaptureScreenToFile(di + $"\\{arrId[i]}_{arrURL[i]}.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                System.Threading.Thread.Sleep(1000);

                InputSimulator ctrlW = new InputSimulator();
                ctrlW.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_W);

            }
            Console.WriteLine("Screenshots Done!");
            Console.WriteLine();
            Console.WriteLine("Downloading Youtube Videos!");

            for (int i = 0; i < fullURLS.Length; i++)
            {
                if (fullURLS[i].Contains("youtube"))
                {
                    string uri = $"{fullURLS[i]}";
                    var youTube = YouTube.Default;
                    var video = youTube.GetVideo(uri);

                    string title = video.Title;
                    string fileExtension = video.FileExtension;
                    string fullName = $"{arrId[i]}_{arrURL[i]}_VC.mp4"; // same thing as title + fileExtension
                    int resolution = video.Resolution;

                    byte[] bytes = video.GetBytes();

                    File.WriteAllBytes(@"C:\Users\Dalla\Videos\" + fullName, bytes);
                }
                else
                {
                    continue;
                }         
            }
            Console.WriteLine("Youtube Videos Were Downloaded!");
            pressOk = Console.ReadLine();
        }
    }
}
