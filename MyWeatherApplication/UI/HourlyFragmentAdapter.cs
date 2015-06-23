using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MyWeatherApplication.VO;

namespace MyWeatherApplication.UI
{
    /// <summary>
    /// Adapter to display hourly weather
    /// </summary>
    class HourlyFragmentAdapter: BaseAdapter<WeatherResponse>
    {
        private int resourceID;
        private List<WeatherResponse> hourlyWeather;
        private Activity context = null;

        public HourlyFragmentAdapter(Activity activityContext, int resId, List<WeatherResponse> list)
        {
            context = activityContext;
            resourceID = resId;
            hourlyWeather = list;
        }

        /// <summary>
        /// This method gets call for each row of list view and creates rowMethod to intialize all the elements for view creation
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View lobjView = null;
            ViewHolder lobjViewHolder = null;
            WeatherResponse tempWeather = null;
            try
            {
                tempWeather = hourlyWeather[position];
                lobjView = convertView;
                if (lobjView != null)
                    lobjViewHolder = lobjView.Tag as ViewHolder;

                if (lobjViewHolder == null)
                {
                    lobjViewHolder = new ViewHolder();
                    lobjView = context.LayoutInflater.Inflate(resourceID, null, false);
                    lobjViewHolder.txtTime = lobjView.FindViewById<TextView>(Resource.Id.txt_Time);
                    lobjViewHolder.txtWeatherCondition = lobjView.FindViewById<TextView>(Resource.Id.txt_WeatherCondition);
                    lobjViewHolder.txtTemperature = lobjView.FindViewById<TextView>(Resource.Id.txt_Temperature);
                    lobjViewHolder.txtTemperatureLabel = lobjView.FindViewById<TextView>(Resource.Id.txt_TemperatureLabel);
                    lobjViewHolder.txtHumidity = lobjView.FindViewById<TextView>(Resource.Id.txt_Humidity);
                    lobjViewHolder.txtTemperatureLabel.Visibility = ViewStates.Visible;
                    lobjViewHolder.txtTemperature.Visibility = ViewStates.Visible;
                    lobjView.Tag = lobjViewHolder;
                }
                // Initialize text views with values
                lobjViewHolder.txtTime.Text = tempWeather.Time.ToString(GlobalConstants.DateTimeFormat);
                lobjViewHolder.txtWeatherCondition.Text = tempWeather.WeatherCondition;
                lobjViewHolder.txtTemperature.Text = tempWeather.Temperature.ToString()+GlobalConstants.FahrenheitKeyword;
                lobjViewHolder.txtHumidity.Text = tempWeather.Humidity.ToString()+GlobalConstants.PercentageSymbol;
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, GlobalConstants.ErrorMessage + ex.ToString(), ToastLength.Short);
            }
            return lobjView;
        }

        public override WeatherResponse this[int position]
        {
            get {
                return hourlyWeather[position];
            }
        }

        public override int Count
        {
            get {

                return hourlyWeather.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public TextView txtTime { get; set; }
            public TextView txtWeatherCondition { get; set; }
            public TextView txtTemperature { get; set; }
            public TextView txtTemperatureLabel { get; set; }
            public TextView txtHumidity { get; set; }
        }
    }
}