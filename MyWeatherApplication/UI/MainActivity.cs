using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;
using Android.Locations;
using System.Collections.Generic;
using MyWeatherApplication.VO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MyWeatherApplication.BL;
using System.Threading;
using Android.Util;

namespace MyWeatherApplication.UI
{
    /// <summary>
    /// Main activity that starts the application
    /// </summary>
    [Activity(Label = "MyWeatherApplication", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, ILocationListener
    {
        LocationManager locationManager;
        Location location;
        DateTime unixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        WeatherResponse currentWeather = null;
        List<WeatherResponse> hourlyWeather = null;
        List<DayWeatherReport> dailyWeather = null;
        GetWeatherReport getWeatherReport = null;
        ProgressDialog progress = null;
        bool IsResponseRecieved = false;

        /// <summary>
        /// Method to initialize all the resources
        /// </summary>
        /// <param name="bundle"></param>
        protected override async void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);

                // Set our view from the "main" layout resource
                SetContentView(Resource.Layout.Main);

                // create tabs for current hourly and daily weather
                setTabs();

                InitializeLocationManager();

                //create web service url
                string url =string.Concat(GlobalConstants.WeatherForecastAPIUrl, GlobalConstants.Separator , GlobalConstants.APIKey , GlobalConstants.Separator, GeoLocations.Latitude ,"," , GeoLocations.Longitude);

                // Fetch the weather information asynchronously, parse the results,
                // then update the screen:
                getWeatherReport = GetWeatherReport.GetInstance();
                
                //Show Spinner for getting the response from the Web Service
                ShowProgressBar();

                string strJsonString = await getWeatherReport.FetchWeatherAsync(url);
                if(!string.IsNullOrEmpty(strJsonString) && !string.IsNullOrWhiteSpace(strJsonString))
                {
                IsResponseRecieved = true;
                JSONParser(strJsonString);
                }

                //Dismiss Progress Bar
                DismissProgressBar();
                
                //Refresh the fragment
                RefreshFragment();
            }
            catch ( InvalidOperationException ex)
            {
                Toast.MakeText(this, GlobalConstants.ErrorMessage + ex.ToString(), ToastLength.Short);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GlobalConstants.ErrorMessage + ex.ToString(), ToastLength.Short);
            }
        }

        // Gets weather data from the passed URL.  
        private async Task<string> FetchWeatherAsync(string url)
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
                    using (Stream stream =  webResponse.GetResponseStream())
                    {
                        StreamReader streamReader = new StreamReader(stream);

                        jsonString = streamReader.ReadToEnd();
                    }
                }
                
            }
            catch (System.Net.WebException ex)
            {
                Toast.MakeText(this, GlobalConstants.ErrorMessage + ex.ToString(), ToastLength.Short);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GlobalConstants.ErrorMessage + ex.ToString(), ToastLength.Short);
            }
            return jsonString;
        }

        /// <summary>
        /// Method to create tabs using action bar
        /// </summary>
        private void setTabs()
        {
            try
            {
                ActionBar actionBar = this.ActionBar;
                actionBar.NavigationMode = ActionBarNavigationMode.Tabs;
                ActionBar.Tab tabCurrentWeather = ActionBar.NewTab();
                tabCurrentWeather.SetText("Current Weather");
                tabCurrentWeather.TabSelected += (sender, args) =>
                {
                    Fragment currentFragment = null;
                    if (IsResponseRecieved)
                    {
                        currentFragment = new CurrentFragment(true);
                    }
                    else
                    { 
                     currentFragment = new CurrentFragment(false);
                    }
                    if (currentFragment != null)
                    {
                        FragmentTransaction transaction = FragmentManager.BeginTransaction();
                        transaction.Replace(Resource.Id.fragment_container, currentFragment);
                        transaction.Commit();

                    }
                };
                ActionBar.AddTab(tabCurrentWeather);

                ActionBar.Tab tabHourlyWeather = ActionBar.NewTab();
                tabHourlyWeather.SetText("Hourly Weather");
                tabHourlyWeather.TabSelected += (sender, args) =>
                {
                    Fragment hourlyFragment = new HourlyFragment();

                    if (hourlyFragment != null)
                    {
                        FragmentTransaction transaction = FragmentManager.BeginTransaction();
                        transaction.Replace(Resource.Id.fragment_container, hourlyFragment);
                        transaction.Commit();

                    }
                };
                ActionBar.AddTab(tabHourlyWeather);

                ActionBar.Tab tabDailyWeather = ActionBar.NewTab();
                tabDailyWeather.SetText("Daily Weather");
                tabDailyWeather.TabSelected += (sender, args) =>
                {
                    Fragment dailyFragment = new DailyFragment();

                    FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    transaction.Replace(Resource.Id.fragment_container, dailyFragment);
                    transaction.Commit();
                };
                ActionBar.AddTab(tabDailyWeather);
            }
            catch( Exception ex)
            {
                Toast.MakeText(this, GlobalConstants.ErrorMessage + ex.ToString(), ToastLength.Short);
            }
        }

        /// <summary>
        /// Parses the weather data, then writes temperature, humidity, conditions, and 
        // location to the screen.
        /// </summary>
        /// <param name="json"></param>
        private void JSONParser(string json)
        {
            try
            {
                currentWeather = WeatherReport.GetCurrentInstance();
                JObject jsonResponseObject = JObject.Parse(json);
                JObject jsonCurrentObject = (JObject)jsonResponseObject["currently"];

                currentWeather.Time = unixTime.AddSeconds(Convert.ToDouble(jsonCurrentObject["time"].ToString())).ToLocalTime();
                currentWeather.WeatherCondition = Convert.ToString(jsonCurrentObject["summary"]);
                currentWeather.Temperature = Convert.ToDouble(jsonCurrentObject["temperature"].ToString());
                currentWeather.Humidity = Convert.ToDouble(jsonCurrentObject["humidity"].ToString());

                JObject jsonHourlyObject = (JObject)jsonResponseObject["hourly"];
                JArray jsonHourlyArray = (JArray)jsonHourlyObject["data"];

                hourlyWeather = WeatherReport.GetHourlyInstance();

                foreach (JObject jsonTempObj in jsonHourlyArray)
                {
                    WeatherResponse tempWeather = new WeatherResponse();
                    tempWeather.WeatherCondition = Convert.ToString(jsonTempObj["summary"]);
                    tempWeather.Temperature = Convert.ToDouble(jsonTempObj["temperature"].ToString());
                    tempWeather.Humidity = Convert.ToDouble(jsonTempObj["humidity"].ToString());
                    tempWeather.Time = unixTime.AddSeconds(Convert.ToDouble(jsonTempObj["time"].ToString())).ToLocalTime();
                    hourlyWeather.Add(tempWeather);
                }

                JObject jsonDailyObject = (JObject)jsonResponseObject["daily"];
                JArray jsonDailyArray = (JArray)jsonDailyObject["data"];

                dailyWeather = WeatherReport.GetDailynstance(); ;
                foreach (JObject jsonTempObj in jsonDailyArray)
                {
                    DayWeatherReport tempWeather = new DayWeatherReport();
                    tempWeather.Time = unixTime.AddSeconds(Convert.ToDouble(jsonTempObj["time"].ToString())).ToLocalTime();
                    tempWeather.WeatherCondition = Convert.ToString(jsonTempObj["summary"]);
                    tempWeather.MinTemperature = Convert.ToDouble(jsonTempObj["temperatureMin"].ToString());
                    tempWeather.MaxTemperature = Convert.ToDouble(jsonTempObj["temperatureMax"].ToString());
                    tempWeather.Humidity = Convert.ToDouble(jsonTempObj["humidity"].ToString());
                    dailyWeather.Add(tempWeather);
                }
            }

            catch (JsonException ex)
            {
                Toast.MakeText(this, GlobalConstants.ErrorMessage + ex.ToString(), ToastLength.Short);
            }
            catch( Exception ex)
            {
                Toast.MakeText(this, GlobalConstants.ErrorMessage + ex.ToString(), ToastLength.Short);
            }
        }

        /// <summary>
        /// Method to get latitude and longitude of device
        /// </summary>
        public void InitializeLocationManager()
        {
            try
            {
                bool isNetworkEnabled = false;

                bool isGPSEnabled = false;

                locationManager = (LocationManager)GetSystemService(Context.LocationService);

                // getting network status
                isNetworkEnabled = locationManager.IsProviderEnabled(LocationManager.NetworkProvider);

                isGPSEnabled = locationManager.IsProviderEnabled(LocationManager.GpsProvider);

                if (!isNetworkEnabled && !isGPSEnabled)
                {
                    // no network provider is enabled
                    Toast.MakeText(this, "No location providers available !", ToastLength.Short).Show();
                }
                else
                {
                    // First get location from Network Provider
                    if (isNetworkEnabled)
                    {
                        locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, 0, 0, this);

                        if (locationManager != null)
                        {
                            location = locationManager.GetLastKnownLocation(LocationManager.NetworkProvider);
                            if (location != null)
                            {
                                if (location.Latitude != 0.0 && location.Longitude != 0.0)
                                {
                                    GeoLocations.Latitude = location.Latitude;
                                    GeoLocations.Longitude = location.Longitude;
                                }
                            }
                        }
                    }
                    // if GPS Enabled get lat/long using GPS Services
                    if (isGPSEnabled)
                    {
                        locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 0, 0, this);
                        if (locationManager != null)
                        {
                            location = locationManager.GetLastKnownLocation(LocationManager.GpsProvider);
                            if (location != null)
                            {
                                if (location.Latitude != 0.0 && location.Longitude != 0.0)
                                {
                                    GeoLocations.Latitude = location.Latitude;
                                    GeoLocations.Longitude = location.Longitude;
                                }
                            }
                        }
                    }
                }
            }
            catch ( InvalidOperationException ex)
            {
                Toast.MakeText(this, GlobalConstants.ErrorMessage + ex.ToString(), ToastLength.Short);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GlobalConstants.ErrorMessage + ex.ToString(), ToastLength.Short);
            }
        }

        /// <summary>
        /// This method is called whenever location of device gets changed
        /// </summary>
        /// <param name="loc"></param>
        public void OnLocationChanged(Location loc)
        {
            location = loc;
            if (loc == null)
            {
                GeoLocations.Latitude = location.Latitude;
                GeoLocations.Longitude = location.Longitude;
            }
        }

        /// <summary>
        /// This methods gets call when application resumes
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
        }

        /// <summary>
        /// This methods gets call when application pauses
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
        }

        /// <summary>
        /// This methods gets call when network provider or GPR is disabled
        /// </summary>
        /// <param name="provider"></param>
        public void OnProviderDisabled(string provider)
        {
            Toast.MakeText(this, "Provider Disabled", ToastLength.Short);
        }

        /// <summary>
        /// This methods gets call when network provider or GPR is enabled
        /// </summary>
        /// <param name="provider"></param>
        public void OnProviderEnabled(string provider)
        {
            Toast.MakeText(this, "Provider Enabled", ToastLength.Short);
        }

        /// <summary>
        /// This methods gets call when status of location changes
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="status"></param>
        /// <param name="extras"></param>
        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            Toast.MakeText(this,status.ToString(), ToastLength.Short);
        }

        
        /// <summary>
        /// This method refreshes the fragment once the data is retrieved from web service
        /// </summary>
        public void RefreshFragment()
        { 
            Fragment currentFragment =null;
            try
            {
                currentFragment = new CurrentFragment(true);
                if (currentFragment != null)
                {
                    FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    transaction.Replace(Resource.Id.fragment_container, currentFragment);
                    transaction.Commit();

                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GlobalConstants.ErrorMessage + ex.ToString(), ToastLength.Short);
            }
        }

        public void ShowProgressBar()
        {
            try
            {
                progress = new ProgressDialog(this);
                progress.Indeterminate = true;
                progress.SetProgressStyle(ProgressDialogStyle.Spinner);
                progress.SetMessage("Contacting server. Please wait...");
                progress.SetCancelable(false);
                progress.Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GlobalConstants.ErrorMessage + ex.ToString(), ToastLength.Short);
            }
        }

        public void DismissProgressBar()
        {

            try
            {
                if (progress != null)
                {
                    progress.Dismiss();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, GlobalConstants.ErrorMessage + ex.ToString(), ToastLength.Short);
            }
        }
    }

   
}

