using BarRaider.SdTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace au.com.mullineaux.lifx
{
    [PluginActionId("au.com.mullineaux.lifx.decreasebrightness")]
    public class DecreaseBrightness : PluginBase
    {
        public bool ButtonPressed = false;
        private class PluginSettings
        {
            public bool ButtonPressed = false;

            public static PluginSettings CreateDefaultSettings()
            {
                PluginSettings instance = new PluginSettings();
                
                instance.AuthToken = String.Empty;
                instance.Selector = String.Empty;
                return instance;
            }

            [JsonProperty(PropertyName = "authToken")]
            public string AuthToken { get; set; }

            [JsonProperty(PropertyName = "selector")]
            public string Selector { get; set; }




        }

        #region Private Members

        private PluginSettings settings;


        #endregion
        public DecreaseBrightness(SDConnection connection, InitialPayload payload) : base(connection, payload)
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
            Logger.Instance.LogMessage(TracingLevel.INFO, $"OnApplicationDidLaunch called");
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

        public override void KeyPressed(KeyPayload payload)
        {
            ButtonPressed = true;
            Logger.Instance.LogMessage(TracingLevel.INFO, "Key Pressed");

    
        }

        public override void KeyReleased(KeyPayload payload) {
            ButtonPressed = false;
            Logger.Instance.LogMessage(TracingLevel.INFO, "OnRelased called");
        }

        public override void OnTick() {
            // TODO:
            // increase brightness by 5
            if (ButtonPressed)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    string requestUri = $"https://api.lifx.com/v1/lights/{settings.Selector}/state/delta";
                    string authToken = settings.AuthToken;

                    var state = new Payload()
                    {
                        Brightness = -0.15,
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(state), Encoding.UTF8, "application/json");

                    var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
                    request.Headers.Add("Authorization", $"Bearer {settings.AuthToken}");
                    request.Content = content;

                    // making this a blocking call as not to spam the API with requests
                    client.SendAsync(request).Wait();

    
                }
            }

        }

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

        #region "Private Methods"
        public class Payload
        {
            [JsonProperty(PropertyName = "power")]
            public string Power { get; set; } = "on";


            [JsonProperty(PropertyName = "brightness")]
            public double Brightness { get; set; }

        }
        #endregion

    }



}