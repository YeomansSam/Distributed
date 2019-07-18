using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;

namespace SecuroteckClient
{
    // Task 10 and beyond
    class Client
    {
        static HttpClient client = new HttpClient();

        private static string currentUser;
        private static string currentAPIkey;

        static void Main(string[] args)
        {
            RunAsync().Wait();
            Console.ReadKey();
        }

        static async Task RunAsync()
        {
            // client.BaseAddress = new Uri("http://secur-o-teck-student-test.azurewebsites.net/5662363/");
            client.BaseAddress = new Uri("http://localhost:24702/");
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
         Console.WriteLine("Hello. What would you like to do?");
            int counter = 0;
                bool cont = false;

            while (cont == false)
            {
                if (counter != 0)
                {
                    Console.WriteLine("What would you like to do next ?");
                }
                    string input = Console.ReadLine();
                    string[] inputs = input.Split(' ');
                    Console.Clear();
                    Console.WriteLine("...please wait...");
                
                switch (inputs[0])
                {
                    case "TalkBack":
                        switch (inputs[1])
                        {
                            case "Hello":
                                try
                                {
                                    Task<string> task = GetStringAsync(client.BaseAddress + "/api/talkback/hello");
                                    if (await Task.WhenAny(task, Task.Delay(20000)) == task)
                                    { Console.WriteLine(task.Result); }
                                    else
                                    { Console.WriteLine("Request Timed Out"); }

                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.GetBaseException().Message);
                                }
                                counter++;
                                break;

                            case "Sort":
                                try
                                {
                                    int[] inputArray = ReturnIntArray(inputs[2]);
                                    string inputString = "";
                                    for (int i = 0; i < inputArray.Length; i++)
                                    {
                                        inputString = inputString + "integers=" + inputArray[i] + "&";
                                    }
                                    Task<string> task = GetStringAsync(client.BaseAddress + "/api/talkback/sort?" + inputString.TrimEnd('&'));
                                    if (await Task.WhenAny(task, Task.Delay(20000)) == task)
                                    { Console.WriteLine(task.Result); }
                                    else
                                    { Console.WriteLine("Request Timed Out"); }

                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.GetBaseException().Message);
                                }
                                counter++;
                                break;
                            default:
                                Console.WriteLine("No input match");
                                counter++;
                                break;
                        }
                        break;
                    case "User":
                        switch (inputs[1])
                        {
                            case "Get":
                                try
                                {
                                    Task<string> task = GetStringAsync(client.BaseAddress + "/api/user/new?username=" + inputs[2]);
                                    if (await Task.WhenAny(task, Task.Delay(20000)) == task)
                                    { Console.WriteLine(task.Result);}
                                    else
                                    { Console.WriteLine("Request Timed Out"); }

                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.GetBaseException().Message);
                                }
                                counter++;
                                break;

                            case "Post":
                                try
                                {
                                    Task<string> task = PostStringAsync(client.BaseAddress + "/api/user/new", inputs[2]);
                                    if (await Task.WhenAny(task, Task.Delay(20000)) == task)
                                    { Console.WriteLine(task.Result);
                                        currentUser = inputs[2];
                                    }
                                    else
                                    { Console.WriteLine("Request Timed Out"); }

                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.GetBaseException().Message);
                                }
                                counter++;
                                break;

                            case "Set":
                                try
                                {
                                    currentUser = inputs[2];
                                    currentAPIkey = inputs[3];
                                    Console.WriteLine("Stored");
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.GetBaseException().Message);
                                }
                                counter++;
                                break;

                            case "Delete":
                                if (currentUser == null || currentAPIkey == null)
                                {
                                    Console.WriteLine("You need to do a User Post or User Set first ");
                                }
                                try
                                {
                                    string path = client.BaseAddress + "/api/user/removeuser?username=" + currentUser;
                                    WebRequest request = WebRequest.Create(path);
                                    request.Method = "Delete";
                                    request.Headers.Add("ApiKey", currentAPIkey);
                                    WebResponse response = request.GetResponse();
                                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII);
                                    Console.WriteLine(reader.ReadToEnd());
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.GetBaseException().Message);
                                }

                                counter++;
                                break;

                            default:
                                Console.WriteLine("No input match");
                                counter++;
                                break;
                        }
                        break;
                    case "Protected":
                        switch (inputs[1])
                        {
                            case "Hello":
                                if (currentUser == null || currentAPIkey == null)
                                {
                                    Console.WriteLine("You need to do a User Post or User Set first ");
                                }
                                try
                                {
                                    string path = client.BaseAddress + "/api/protected/hello";
                                    WebRequest request = WebRequest.Create(path);
                                    request.Method = "Get";
                                    request.Headers.Add("ApiKey", currentAPIkey);
                                    WebResponse response = request.GetResponse();
                                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII);
                                    Console.WriteLine(reader.ReadToEnd());
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.GetBaseException().Message);
                                }

                                counter++;
                                break;

                            case "SHA1":
                                if (currentUser == null || currentAPIkey == null)
                                {
                                    Console.WriteLine("You need to do a User Post or User Set first ");
                                }
                                try
                                {
                                    string path = client.BaseAddress + "/api/protected/sha1?message=" + inputs[2];
                                    WebRequest request = WebRequest.Create(path);
                                    request.Method = "Get";
                                    request.Headers.Add("ApiKey", currentAPIkey);
                                    WebResponse response = request.GetResponse();
                                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII);
                                    Console.WriteLine(reader.ReadToEnd());
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.GetBaseException().Message);
                                }

                                counter++;
                                break;

                            case "SHA256":
                                if (currentUser == null || currentAPIkey == null)
                                {
                                    Console.WriteLine("You need to do a User Post or User Set first ");
                                }
                                try
                                {
                                    string path = client.BaseAddress + "/api/protected/sha256?message=" + inputs[2];
                                    WebRequest request = WebRequest.Create(path);
                                    request.Method = "Get";
                                    request.Headers.Add("ApiKey", currentAPIkey);
                                    WebResponse response = request.GetResponse();
                                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII);
                                    Console.WriteLine(reader.ReadToEnd());
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.GetBaseException().Message);
                                }

                                counter++;
                                break;

                            default:
                                Console.WriteLine("No input match");
                                counter++;
                                break;
                        }
                        break;
                    case "Exit":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("No input match");
                        counter++;
                        break;

                }
            }
            
        }
        static async Task<string> GetStringAsync(string path)
        {
            string responsestring = "";
            HttpResponseMessage response = await client.GetAsync(path);
            responsestring = await response.Content.ReadAsStringAsync();
            return responsestring;
        }

        static async Task<string> PostStringAsync(string path, string content)
        {
            string responsestring = "";
            HttpResponseMessage response = await client.PostAsJsonAsync(path, content);
            response.EnsureSuccessStatusCode();
            if(response.IsSuccessStatusCode)
            {
                currentAPIkey = response.Content.ReadAsStringAsync().Result;
                currentAPIkey = currentAPIkey.Trim('"');
                return responsestring = "Got APIkey";
            }
            responsestring = await response.Content.ReadAsStringAsync();
            return responsestring;
        }

        

        public static int[] ReturnIntArray(string intArrayString)
        {
            string arrayInt = intArrayString;
            arrayInt = arrayInt.TrimStart('[');
            arrayInt = arrayInt.TrimEnd(']');
            string[] intsArray = arrayInt.Split(',');
            int[] arrayOfInts = new int[intsArray.Length];
            for (int i = 0; i < intsArray.Length; i++)
            {
                arrayOfInts[i] = int.Parse(intsArray[i]);
            }
            return arrayOfInts;
        }

    }
    
}
