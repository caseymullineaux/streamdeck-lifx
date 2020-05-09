using BarRaider.SdTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace au.com.mullineaux.lifx
{
    [PluginActionId("au.com.mullineaux.lifx.effectmove")]
    public class EffectMove : PluginBase
    {
        private class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                PluginSettings instance = new PluginSettings();
                instance.AuthToken = String.Empty;
                instance.Selector = String.Empty;
                instance.Direction = String.Empty;
                instance.Period = 1.0;
                instance.Cycles = 1.0;
   
                return instance;
            }

            [JsonProperty(PropertyName = "authToken")]
            public string AuthToken { get; set; }

            [JsonProperty(PropertyName = "selector")]
            public string Selector { get; set; }

            [JsonProperty(PropertyName = "direction")]
            public string Direction { get; set; }



            [JsonProperty(PropertyName = "period")]
            public double Period { get; set; }

            [JsonProperty(PropertyName = "cycles")]
            public double? Cycles { get; set; }





        }

        #region Private Members

        private PluginSettings settings;


        #endregion
        public EffectMove(SDConnection connection, InitialPayload payload) : base(connection, payload)
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

        public override async void KeyPressed(KeyPayload payload)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                string requestUri = $"https://api.lifx.com/v1/lights/{settings.Selector}/effects/move";
                string authToken = settings.AuthToken;

                var state = new Payload()
                {
                    Direction = settings.Direction, 
                    Period = Convert.ToDouble(settings.Period), 
                    Cycles = Convert.ToDouble(settings.Cycles),
            };
                var content = new StringContent(JsonConvert.SerializeObject(state), Encoding.UTF8, "application/json");
                Logger.Instance.LogMessage(TracingLevel.DEBUG, $"{await content.ReadAsStringAsync()}");

                var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
                request.Headers.Add("Authorization", $"Bearer {settings.AuthToken}");
                request.Content = content;

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Logger.Instance.LogMessage(TracingLevel.ERROR, $"Call to LIFX API failed");
                    Logger.Instance.LogMessage(TracingLevel.ERROR, $"Status Code: {response.StatusCode}");
                    Logger.Instance.LogMessage(TracingLevel.ERROR, $"{await response.Content.ReadAsStringAsync()}");

                } else
                {
                    Logger.Instance.LogMessage(TracingLevel.DEBUG, $"{await response.Content.ReadAsStringAsync()}");
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

            [JsonProperty(PropertyName = "direction")]
            public string Direction { get; set; } = "forward";

            [JsonProperty(PropertyName = "period")]
            public double Period { get; set; } = 1.0;


            [JsonProperty(PropertyName = "cycles")]
            public double Cycles { get; set; } = 1.0;

            [JsonProperty(PropertyName = "power_on")]
            public bool PowerOn { get; set; } = true;

        }
        #endregion

    }



}