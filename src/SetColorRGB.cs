using BarRaider.SdTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace au.com.mullineaux.lifx
{
    [PluginActionId("au.com.mullineaux.lifx.setcolorrgb")]
    public class SetColorRGB : PluginBase
    {
        private class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                PluginSettings instance = new PluginSettings();
                instance.AuthToken = String.Empty;
                instance.Selector = String.Empty;
                instance.Red= "255";
                instance.Green = "255";
                instance.Blue = "255";
                return instance;
            }

          [JsonProperty(PropertyName = "authToken")]
            public string AuthToken { get; set; }

            [JsonProperty(PropertyName = "selector")]
            public string Selector { get; set; }

            [JsonProperty(PropertyName = "red")]
            public string Red { get; set; } = "0";

            [JsonProperty(PropertyName = "green")]
            public string Green { get; set; } = "0";
            [JsonProperty(PropertyName = "blue")]
            public string Blue { get; set; } = "0";

            [JsonProperty(PropertyName = "brightness")]
            public double Brightness { get; set; }


            [JsonProperty(PropertyName = "duration")]
            public double Duration { get; set; }


        }

        #region Private Members

        private PluginSettings settings;
        

        #endregion
        public SetColorRGB(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            if (payload.Settings == null || payload.Settings.Count == 0)
            {
                this.settings = PluginSettings.CreateDefaultSettings();
            }
            else
            {
                this.settings = payload.Settings.ToObject<PluginSettings>();
            }

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

        private void Connection_OnSendToPlugin(object sender, BarRaider.SdTools.Wrappers.SDEventReceivedEventArgs<BarRaider.SdTools.Events.SendToPlugin> e)
        {
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

        public override async void  KeyPressed(KeyPayload payload)
        {

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                string requestUri = $"https://api.lifx.com/v1/lights/{settings.Selector}/state";
                string authToken = settings.AuthToken;

                var state = new Payload()
                {
                    Color = $"rgb:{settings.Red},{settings.Green},{settings.Blue}",
                    Brightness = Convert.ToDouble(settings.Brightness / 100),
                    Duration = Convert.ToDouble(settings.Duration)
                };
                var content = new StringContent(JsonConvert.SerializeObject(state), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Put, requestUri);
                request.Headers.Add("Authorization", $"Bearer {settings.AuthToken}");
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

        public override void OnTick() { }

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            
            Tools.AutoPopulateSettings(settings, payload.Settings);
            
            SaveSettings();
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        #region Private Methods

        private Task SaveSettings()
        {
            return Connection.SetSettingsAsync(JObject.FromObject(settings));
        }

        #endregion

        #region "Private Classes"
        public class Payload
        {
            [JsonProperty(PropertyName = "power")]
            public string Power { get; set; } = "on";

            [JsonProperty(PropertyName = "color")]
            public string Color { get; set; } = "rgb:255,255,225";

            [JsonProperty(PropertyName = "brightness")]
            public double Brightness { get; set; } = 1.0;

            [JsonProperty(PropertyName = "duration")]
            public double Duration { get; set; } = 0.0;

        }
        #endregion
    }
    //public enum Color { Blue, Cyan, Green, Orange, Pink, Purple, Red, Yellow, White };



}