using System;
using System.IO;
using System.Net;
using System.Text;
using MeeyPage.Data.AppSetting;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace MeeyPage.Core
{
    public static class AppSettingsHelper
    {
        private static AppSettingsConfig _appSettings;

        public static AppSettingsConfig AppSettings
        {
            get { return _appSettings; }
        }

        public static IConfiguration GetConfiguration(string url = "https://localhost:44306/api/configuration/get-configuration?env=Development")
        {
            IConfigurationRoot configuration = null;
            string res = "";
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                var client = new RestClient();
                var request = new RestRequest(url, Method.Get) { RequestFormat = DataFormat.Json };
                var response = client.ExecuteAsync(request);
                res = response.Result.Content;
                byte[] byteArray = Encoding.ASCII.GetBytes(response.Result.Content);
                MemoryStream stream = new MemoryStream(byteArray);
                configuration = new ConfigurationBuilder().AddJsonStream(stream)
                   .Build();
            }
            catch (Exception ex)
            {
                CommonFunction.LogWrite(ex.Message + " " + url + " " + res);
            }

            return configuration;
        }
    }
}
