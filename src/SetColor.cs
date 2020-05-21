using au.com.mullineaux.lifx.Classes;
using BarRaider.SdTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static au.com.mullineaux.lifx.Classes.LIFXApi;

namespace au.com.mullineaux.lifx
{
    [PluginActionId("au.com.mullineaux.lifx.setcolor")]
    public class SetColor : PluginBase
    {
        private class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                PluginSettings instance = new PluginSettings()
                {
                    AuthToken = Properties.Settings.Default.AuthToken,
                    AuthTokenIsValid = Properties.Settings.Default.AuthTokenIsValid,
                    SelectorType = "single",
                    SelectorList = null,
                    Brightness = 100,
                    Color = "white"
                };

                return instance;
            }


            [JsonProperty(PropertyName = "authToken")]
            public string AuthToken { get; set; } = Properties.Settings.Default.AuthToken;

            [JsonProperty(PropertyName = "authTokenIsValid")]
            public bool AuthTokenIsValid { get; set; }


            [JsonProperty(PropertyName = "selectorType")]
            public string SelectorType { get; set; }

            [JsonProperty(PropertyName = "selectorList")]
            public List<Selector> SelectorList { get; set; }


            [JsonProperty(PropertyName = "selector")]
            public string Selector { get; set; }


            [JsonProperty(PropertyName = "color")]
            public string Color { get; set; }

            [JsonProperty(PropertyName = "brightness")]
            public double Brightness { get; set; }


            [JsonProperty(PropertyName = "duration")]
            public double Duration { get; set; }




        }

        #region Private Members

        private PluginSettings settings;

        #endregion
        public SetColor(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {



            if (payload.Settings == null || payload.Settings.Count == 0)
            {

                this.settings = PluginSettings.CreateDefaultSettings();
                SaveSettings();
            }
            else
            {
                this.settings = payload.Settings.ToObject<PluginSettings>();
            }

            Connection.GetGlobalSettingsAsync();
            Connection.OnApplicationDidLaunch += Connection_OnApplicationDidLaunch;
            Connection.OnApplicationDidTerminate += Connection_OnApplicationDidTerminate;
            Connection.OnDeviceDidConnect += Connection_OnDeviceDidConnect;
            Connection.OnDeviceDidDisconnect += Connection_OnDeviceDidDisconnect;
            Connection.OnPropertyInspectorDidAppear += Connection_OnPropertyInspectorDidAppear;
            Connection.OnPropertyInspectorDidDisappear += Connection_OnPropertyInspectorDidDisappear;
            Connection.OnSendToPlugin += Connection_OnSendToPlugin;
            Connection.OnTitleParametersDidChange += Connection_OnTitleParametersDidChange;
        }

        private void Connection_OnTitleParametersDidChange(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.TitleParametersDidChange> e)
        {
        }

        private async void Connection_OnSendToPlugin(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.SendToPlugin> e)
        {
            var payload = e.Event.Payload;
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnSendToPlugin called");
            // Logger.Instance.LogMessage(TracingLevel.DEBUG, $"Token: {settings.AuthToken}");

            if (payload["property_inspector"] != null)
            {

                switch (payload["property_inspector"].ToString())
                {
                    case "validateToken":
                        // Validate the token
                        // settings.AuthTokenIsValid = false;
                        var _authToken = (string)payload["authToken"];
                        Properties.Settings.Default.AuthToken = _authToken;

                        Logger.Instance.LogMessage(TracingLevel.DEBUG, $"Token from PI: {_authToken}");
                        settings.AuthToken = _authToken;

                        if (await LIFXApi.ValidateAuthToken(_authToken))
                        {
                            settings.AuthTokenIsValid = true;
                            Logger.Instance.LogMessage(TracingLevel.DEBUG, $"Token Valid: {_authToken}");
                        } else
                        {
                            settings.AuthTokenIsValid = false;
                            Logger.Instance.LogMessage(TracingLevel.DEBUG, $"Token Invalid: {_authToken}");
                        }
                        
                        await SaveSettings();
                        Properties.Settings.Default.Save();
                        
                        break;
                    case "resetPlugin":
                        settings.AuthToken = String.Empty;
                        settings.AuthTokenIsValid = false;
                        await SaveSettings();

                        Properties.Settings.Default.AuthToken = String.Empty;
                        Properties.Settings.Default.Save();

                        break;
                    case "updateSelectors":
                        if (!String.IsNullOrEmpty(Properties.Settings.Default.AuthToken))
                        {
                            var selectors = await LIFXApi.GetSelectors(Properties.Settings.Default.AuthToken);
                            settings.SelectorType = (string)payload["type"].ToString().ToLower();

                            if (settings.SelectorType == "single")
                            {
                                settings.SelectorList = selectors.Item1;

                            }
                            else
                            {
                                settings.SelectorList = selectors.Item2;
                            }

                            
                            await SaveSettings();
                        }
                        break;
                }
            }
        }

        private void Connection_OnPropertyInspectorDidDisappear(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.PropertyInspectorDidDisappear> e)
        {
        }

        private void Connection_OnPropertyInspectorDidAppear(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.PropertyInspectorDidAppear> e)
        {
        }

        private void Connection_OnDeviceDidDisconnect(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.DeviceDidDisconnect> e)
        {
        }

        private void Connection_OnDeviceDidConnect(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.DeviceDidConnect> e)
        {
        }

        private void Connection_OnApplicationDidTerminate(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.ApplicationDidTerminate> e)
        {
        }

        private void Connection_OnApplicationDidLaunch(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.ApplicationDidLaunch> e)
        {
        }

        public override void Dispose()
        {
            Connection.OnApplicationDidLaunch -= Connection_OnApplicationDidLaunch;
            Connection.OnApplicationDidTerminate -= Connection_OnApplicationDidTerminate;
            Connection.OnDeviceDidConnect -= Connection_OnDeviceDidConnect;
            Connection.OnDeviceDidDisconnect -= Connection_OnDeviceDidDisconnect;
            Connection.OnPropertyInspectorDidAppear -= Connection_OnPropertyInspectorDidAppear;
            Connection.OnPropertyInspectorDidDisappear -= Connection_OnPropertyInspectorDidDisappear;
            Connection.OnSendToPlugin -= Connection_OnSendToPlugin;
            Connection.OnTitleParametersDidChange -= Connection_OnTitleParametersDidChange;
            Logger.Instance.LogMessage(TracingLevel.INFO, $"Destructor called");
        }

        public override async void KeyPressed(KeyPayload payload)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                string selector;
                if (settings.SelectorType == "group")
                {
                    selector = $"group_id:{settings.Selector}";
                } else
                {
                    selector = settings.Selector;
                }

                string requestUri = $"https://api.lifx.com/v1/lights/{selector}/state";

                var state = new Payload()
                {
                    Color = settings.Color.ToLower(),
                    Brightness = Convert.ToDouble(settings.Brightness / 100),
                    Duration = Convert.ToDouble(settings.Duration)
                };
                var content = new StringContent(JsonConvert.SerializeObject(state), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Put, requestUri);
                request.Headers.Add("Authorization", $"Bearer {Properties.Settings.Default.AuthToken}");
                request.Content = content;

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Logger.Instance.LogMessage(TracingLevel.INFO, $"Call to LIFX API failed");
                    Logger.Instance.LogMessage(TracingLevel.INFO, $"Status Code: {response.StatusCode}");
                    Logger.Instance.LogMessage(TracingLevel.INFO, $"{response.Content}");

                }
                else
                {
                    Logger.Instance.LogMessage(TracingLevel.INFO, $"API Response: {response.StatusCode}");
                    Logger.Instance.LogMessage(TracingLevel.INFO, $"{await response.Content.ReadAsStringAsync()}");

                }
            }
            Logger.Instance.LogMessage(TracingLevel.INFO, "Key Pressed");
        }

        public override void KeyReleased(KeyPayload payload) { }

        public override void OnTick()
        {
            // TODO: Set the image of the icon if no access token has been configured

            // if (settings.AuthToken == String.Empty || settings.AuthToken == "")
            // {

            //     Logger.Instance.LogMessage(TracingLevel.DEBUG, $"AuthToken={settings.AuthToken}");
            //     await Connection.SetImageAsync(Properties.Settings.Default.NoToken);

            // }

        }

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Tools.AutoPopulateSettings(settings, payload.Settings);
            SaveSettings();
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
        {

        }

        #region Private Methods

        private Task SaveSettings()
        {
            return Connection.SetSettingsAsync(JObject.FromObject(settings));

        }

        #endregion

        #region Private Classes
        public class Payload
        {
            [JsonProperty(PropertyName = "power")]
            public string Power { get; set; } = "on";

            [JsonProperty(PropertyName = "color")]
            public string Color { get; set; } = "white";

            [JsonProperty(PropertyName = "brightness")]
            public double Brightness { get; set; } = 1.0;

            [JsonProperty(PropertyName = "duration")]
            public double Duration { get; set; } = 0.0;

        }
        #endregion

    }



}