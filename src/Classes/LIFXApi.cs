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

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Properties.Settings.Default.AuthTokenIsValid = false;
            } else
            {
                Properties.Settings.Default.AuthToken = _authToken;
                Properties.Settings.Default.AuthTokenIsValid = true;
            }
            
            return Properties.Settings.Default.AuthTokenIsValid;
        }


        public static async Task<Tuple<List<Selector>, List<Selector>>> GetSelectors(string _authToken)
        {

            List<Selector> lights = new List<Selector>();
            List<Selector> groups = new List<Selector>();

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
                List<Light> lights_list = JsonConvert.DeserializeObject<List<Light>>(data);

                foreach (var light in lights_list)
                {
                    lights.Add(new Selector()
                    {
                        Id = light.Id,
                        Name = light.Label

                    });

                    groups.Add(new Selector()
                    {
                        Id = light.Group.Id,
                        Name = light.Group.Name
                    });
                }

                groups = groups.Distinct().ToList();

            }

            return Tuple.Create(lights, groups);

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

        public class Selector
        {
            public string Id { get; set; }
            public string Name { get; set; }

            // public Selector(Light _light)
            // {
            //     this.Id = _light.Id;
            //     this.Name = _light.Label;
            // }
            // public Selector(LightGroup _group)
            // {
            //     this.Id = _group.Id;
            //     this.Name = _group.Name;

            // }
        }

    }
    #endregion

}

