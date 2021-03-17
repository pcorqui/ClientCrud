using ClientCRUD.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

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
            client.BaseAddress = new Uri(uri);
            try
            {


                string json = JsonSerializer.Serialize<Usuario>(user, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                client.Timeout = System.TimeSpan.FromMinutes(30);
                //HttpResponseMessage response = await client.PostAsync("/auth/login", content);
                HttpResponseMessage response = await client.PostAsync("/users", content);

                var result = await response.Content.ReadAsStringAsync();

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
            // Uri uri = new Uri(string.Format(Constants.RestUrl, string.Empty));
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = authHeader;
            client.BaseAddress = new Uri(uri);
            try
            {
                string json = JsonSerializer.Serialize<Client>(cliente, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewClient)
                {
                    response = await client.PostAsync("/api/cliente", content);
                }
                else
                {
                    response = await client.PutAsync("/api/cliente/"+cliente.ClientID, content);
                }

                if (response.IsSuccessStatusCode)
                {
                   return 200;
                }

            }
            catch (Exception ex)
            {
               // Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return 404;
        }

        //borrado de un cliente
        public static async Task<int> DeleteClient(string id)
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
