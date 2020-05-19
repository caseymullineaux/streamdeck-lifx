using BarRaider.SdTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace au.com.mullineaux.lifx.Classes
{

    public class LIFXApi
    {
        private static HttpClient Client = new HttpClient();
        public async static Task<bool> ValidateAuthToken(string _authToken)
        {
            //AuthTokenIsValid = false;

            string requestUri = $"https://api.lifx.com/v1/lights/all";
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("Authorization", $"Bearer {_authToken}");
            var response = await Client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                Properties.Settings.Default.AuthToken = _authToken;
                //AuthTokenIsValid = true;
                return true;
            }
            return false;
        }


        public static async Task<Tuple<Dictionary<string, string>, Dictionary<string, string>>> GetSelectors(string _authToken)
        {

            // Queries the LIFX API and returns a tuple of dictionaries for each selector 
            // mapping their selctor Id to the name Name
            // Example:
            // Item1 = Lights: d073d5124, Computer LED
            // Item2 = Groups: b21248782248754774, Office

            var lights_dict = new Dictionary<string, string>();
            var groups_dict = new Dictionary<string, string>();

            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string requestUri = $"https://api.lifx.com/v1/lights/all";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("Authorization", $"Bearer {_authToken}");
            var response = await Client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Logger.Instance.LogMessage(TracingLevel.INFO, $"Call to LIFX API failed");
                Logger.Instance.LogMessage(TracingLevel.INFO, $"Status Code: {response.StatusCode}");

            }
            else
            {
                var data = await response.Content.ReadAsStringAsync();
                List<Light> lights = JsonConvert.DeserializeObject<List<Light>>(data); lights = JsonConvert.DeserializeObject<List<Light>>(data);


                lights_dict = lights.ToDictionary(x => x.Id, x => x.Label);

                foreach (var light in lights)
                {
                    if (!groups_dict.ContainsKey(light.Group.Id))
                    {
                        groups_dict.Add(light.Group.Id, light.Group.Name);
                    }
                }

            }

            return Tuple.Create(lights_dict, groups_dict);

        }

        // public async static Task<List<Light>> GetLights(string authToken)
        // {
        //     List<Light> lights = new List<Light>();
        //     string requestUri = $"https://api.lifx.com/v1/lights/all";

        //     Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        //     var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        //     request.Headers.Add("Authorization", $"Bearer {authToken}");
        //     var response = await Client.SendAsync(request);


        //     if (!response.IsSuccessStatusCode)
        //     {
        //         Logger.Instance.LogMessage(TracingLevel.INFO, $"Call to LIFX API failed");
        //         Logger.Instance.LogMessage(TracingLevel.INFO, $"Status Code: {response.StatusCode}");

        //     }
        //     else
        //     {
        //         var data = await response.Content.ReadAsStringAsync();
        //         lights = JsonConvert.DeserializeObject<List<Light>>(data);
        //     }
        //     return lights;

        // }


        // public async static Task<List<LightGroup>> GetLightGroups(string authToken)
        // {
        //     List<LightGroup> lightGroups = new List<LightGroup>();
        //     string requestUri = $"https://api.lifx.com/v1/lights/all";

        //     Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        //     var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        //     request.Headers.Add("Authorization", $"Bearer {authToken}");
        //     var response = await Client.SendAsync(request);


        //     if (!response.IsSuccessStatusCode)
        //     {
        //         Logger.Instance.LogMessage(TracingLevel.INFO, $"Call to LIFX API failed");
        //         Logger.Instance.LogMessage(TracingLevel.INFO, $"Status Code: {response.StatusCode}");
        //     }
        //     else
        //     {
        //         var data = await response.Content.ReadAsStringAsync();
        //         var lights = JsonConvert.DeserializeObject<List<Light>>(data);


        //         foreach (var light in lights)
        //         {
        //             lightGroups.Add(light.Group);
        //         }

        //     }
        //     return lightGroups;
        // }


        #region TODO: Methods

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

        #endregion

        #region Private Classes

        public class Light
        {
            public string Id { get; set; }
            public string Label { get; set; }
            public LightGroup Group { get; set; }
        }

        public class LightGroup
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

    }
    #endregion

}

