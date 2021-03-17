using ClientCRUD.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace ClientCRUD.Config
{
    public class Config
    {      
        public static JsonSerializerOptions serializerOptions;
        public static string uri = "http://apps01.forteinnovation.mx:8590/api";

        //obtener token de acceso
        public static async Task<string> PostWebService(Usuario user)
        {
            var client = new HttpClient();
            Uri uri = new Uri("http://apps01.forteinnovation.mx:8590/api/auth/login");
            try
            {
                serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize<Usuario>(user, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                client.Timeout = System.TimeSpan.FromMinutes(30);
                //HttpResponseMessage response = await client.PostAsync("/auth/login", content);
                HttpResponseMessage response = await client.PostAsync(uri,content);

                var result = await response.Content.ReadAsStringAsync();
                //var result2 = await response.Content.();

                if (response != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            return null;
        }

        //obtener listado de clientes
        public static async Task<string> GetWebServiceWithToken(string value)
        {
             string token = App.Current.Properties["token"] as string;
                var authHeader = new AuthenticationHeaderValue("bearer", token);
            HttpClient client = new HttpClient();
           
            String query = "http://apps01.forteinnovation.mx:8590/api" + value;
            client.DefaultRequestHeaders.Authorization = authHeader;
            client.Timeout = System.TimeSpan.FromMinutes(30);
            var response = await client.GetStringAsync(query);

            if (response != null)
            {
                return response;
            }
            else
            {
                return null;
            }

        }

        

        //guardar o actualizar un usuario
        public static  async Task<int> SaveOrUpdateClient(Client cliente, bool isNewClient = false)
        {
           string token = App.Current.Properties["token"] as string;
           var authHeader = new AuthenticationHeaderValue("bearer",token);
            
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = authHeader;
            var Code = 404;
            try
            {
                string json = JsonSerializer.Serialize<Client>(cliente, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewClient)
                {
                    Uri uri = new Uri("http://apps01.forteinnovation.mx:8590/api/cliente");
                    response = await client.PostAsync(uri, content);
                }
                else
                {
                    Uri uri = new Uri("http://apps01.forteinnovation.mx:8590/api/cliente/" + cliente.clienteId);
                    response = await client.PutAsync(uri, content);

                }

                if(response != null)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var strResultJson = JObject.Parse(result);

                    Code = strResultJson.Value<int>("Code");
                }

                if (response.IsSuccessStatusCode)
                {
                   return Code;
                }

            }
            catch (Exception ex)
            {
               // Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return Code;
        }

        //borrado de un cliente
        public static async Task<int> DeleteClient(int id)
        {
            string token = App.Current.Properties["token"] as string;
            HttpClient client = new HttpClient();
            var authHeader = new AuthenticationHeaderValue("bearer", token);
            client.DefaultRequestHeaders.Authorization = authHeader;

            try
            {
                HttpResponseMessage response = await client.DeleteAsync(new Uri(uri + "/cliente/"+id));

                if (response.IsSuccessStatusCode)
                {
                    return 200;
                }

            }
            catch (Exception ex)
            {
                //Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return 404;
        }
    }
}
