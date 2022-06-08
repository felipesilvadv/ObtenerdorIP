using System;
using System.Net;
using System.IO;
using System.Text.Json;


namespace ObtenedorIP
{
    class IPAddress{
        string ip;
        public string getIp(){
            return ip;
        }
    }
    class Program
    {
        static string urlApp = "https://my-ip-value.herokuapp/";
        string token;
        string path;
        static string GetIp(Program p){
            WebRequest request = HttpWebRequest.Create(urlApp);
            try {
                if (p.token == null){
                    p.setToken();
                }
                string token_value = p.token;
                request.Headers.Add("Authorization", token_value);
            }catch (ArgumentNullException) {
                Console.WriteLine("No se ha definido el token");
            }
            WebResponse response = request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string ip = reader.ReadToEnd();
            IPAddress ip_json = JsonSerializer.Deserialize<IPAddress>(ip);
            string ip_return = ip_json.getIp();
            return ip_return;
        }
        void setToken(){
            token = Environment.GetEnvironmentVariable("IP_TOKEN");
            if (token == null){
                throw new ArgumentNullException("IP_TOKEN");
            }
            
        }
        void setPath(){
            path = Environment.GetEnvironmentVariable("IP_PATH");
            if (path == null){
                throw new ArgumentNullException("IP_PATH");
            }
        }
        static void Main(string[] args)
        {
            Program p = new Program();
            p.setToken();
            p.setPath();
            string ip = GetIp(p);
            using (StreamWriter writer = new StreamWriter(p.path)){
                writer.WriteLine($"La dirección ip es : {ip}");
                writer.WriteLine("Copiala en el programa de escritorio remoto");
            }



        }
    }
}
