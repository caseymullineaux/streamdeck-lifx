using BarRaider.SdTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace au.com.mullineaux.lifx.Classes
{

    public class LIFXApi
    {
        public static HttpClient Client = new HttpClient();
        public async static Task<bool> ValidateAuthToken(string authToken)
        {
            try
            {
                await GetLights(authToken);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async static Task<List<Light>> GetLights(string authToken)
        {
            List<Light> lights = new List<Light>();
            string requestUri = $"https://api.lifx.com/v1/lights/all";

            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("Authorization", $"Bearer {authToken}");
            var response = await Client.SendAsync(request);


            if (!response.IsSuccessStatusCode)
            {
                Logger.Instance.LogMessage(TracingLevel.INFO, $"Call to LIFX API failed");
                Logger.Instance.LogMessage(TracingLevel.INFO, $"Status Code: {response.StatusCode}");
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
            }
            else
            {
                var data = await response.Content.ReadAsStringAsync();
                lights = JsonConvert.DeserializeObject<List<Light>>(data);
            }
            return lights;

        }


        public async static Task<List<LightGroup>> GetLightGroups(string authToken)
        {
            List<LightGroup> lightGroups = new List<LightGroup>();
            string requestUri = $"https://api.lifx.com/v1/lights/all";

            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("Authorization", $"Bearer {authToken}");
            var response = await Client.SendAsync(request);


            if (!response.IsSuccessStatusCode)
            {
                Logger.Instance.LogMessage(TracingLevel.INFO, $"Call to LIFX API failed");
                Logger.Instance.LogMessage(TracingLevel.INFO, $"Status Code: {response.StatusCode}");
            }
            else
            {
                var data = await response.Content.ReadAsStringAsync();
                var lights = JsonConvert.DeserializeObject<List<Light>>(data);


                foreach (var light in lights)
                {
                    lightGroups.Add(light.Group);
                }

            }
            return lightGroups;
        }




        public static void SetState()
        {
            throw new NotImplementedException();
        }

        public static void TogglePower()
        {
            throw new NotImplementedException();
        }

        public static void BreatheEffect()
        {
            throw new NotImplementedException();
        }

        public static void MoveEffect()
        {

            throw new NotImplementedException();
        }

        public static void MorphEffect()
        {
            throw new NotImplementedException();
        }

        public static void PulseEffect()
        {
            throw new NotImplementedException();
        }

        public static void EffectsOff()
        {
            throw new NotImplementedException();
        }



    }
}
