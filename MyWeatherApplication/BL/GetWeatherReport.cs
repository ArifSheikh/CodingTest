using Android.Media;
using System;
using System.Collections.Generic;
using System.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyWeatherApplication.BL
{
    class GetWeatherReport
    {
        private static GetWeatherReport getWeatherReport = null;

        /// <summary>
        /// This method returns single instance of the class
        /// </summary>
        /// <returns></returns>
        public static GetWeatherReport GetInstance()
        {
            if(getWeatherReport==null)
            {
                getWeatherReport = new GetWeatherReport();
            }
            return getWeatherReport;
        }

        /// <summary>
        /// This methods gets the response from web service
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> FetchWeatherAsync(string url)
        {

            string jsonString = string.Empty;
            try
            {
                // Create an HTTP web request using the URL:
                HttpWebRequest lobjHttpRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                lobjHttpRequest.ContentType = "application/json";
                lobjHttpRequest.Proxy = WebRequest.DefaultWebProxy;
                lobjHttpRequest.Method = "GET";

                // Send the request to the server and wait for the response:
                using (WebResponse webResponse = await lobjHttpRequest.GetResponseAsync())
                {
                    // Get a stream representation of the HTTP web response:
                    using (System.IO.Stream stream = webResponse.GetResponseStream())
                    {
                        StreamReader streamReader = new StreamReader(stream);

                        jsonString = streamReader.ReadToEnd();
                    }
                }

            }
            
            catch (Exception ex)
            {
                throw;
            }
            return jsonString;
        }

    }
}
