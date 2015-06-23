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
    /// Adapter to display daily weather
    /// </summary>
    class DailyFragmentAdapter: BaseAdapter<DayWeatherReport>
    {
        private Activity context = null;
        private int resourceID=0;
        private List<DayWeatherReport> dailyWeatherList=null;

        public DailyFragmentAdapter(Activity activityContext, int resID, List<DayWeatherReport> list)
        {
            context = activityContext;
            resourceID = resID;
            dailyWeatherList = list;
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
            DayWeatherReport tempDailyWeather = null;

            try
            {
                tempDailyWeather = dailyWeatherList[position];
                lobjView = convertView;
                if (lobjView != null)
                    lobjViewHolder = lobjView.Tag as ViewHolder;

                //save view in view holder
                if (lobjViewHolder == null)
                {
                    lobjViewHolder = new ViewHolder();
                    lobjView = context.LayoutInflater.Inflate(resourceID, null, false);
                    lobjViewHolder.txtTime = lobjView.FindViewById<TextView>(Resource.Id.txt_Time);
                    lobjViewHolder.txtWeatherCondition = lobjView.FindViewById<TextView>(Resource.Id.txt_WeatherCondition);
                    lobjViewHolder.txtWeatherConditionLabel = lobjView.FindViewById<TextView>(Resource.Id.txt_WeatherConditionLabel);
                    lobjViewHolder.txtMinTemperature = lobjView.FindViewById<TextView>(Resource.Id.txt_MinTemperature);
                    lobjViewHolder.txtMaxTemperature = lobjView.FindViewById<TextView>(Resource.Id.txt_MaxTemperature);
                    lobjViewHolder.txtHumidity = lobjView.FindViewById<TextView>(Resource.Id.txt_Humidity);
                    lobjViewHolder.txtMinTemperatureLabel = lobjView.FindViewById<TextView>(Resource.Id.txt_MinTemperatureLabel);
                    lobjViewHolder.txtMaxTemperatureLabel = lobjView.FindViewById<TextView>(Resource.Id.txt_MaxTemperatureLabel);
                    lobjViewHolder.txtMinTemperatureLabel.Visibility=ViewStates.Visible;
                    lobjViewHolder.txtMaxTemperatureLabel.Visibility = ViewStates.Visible;
                    lobjViewHolder.txtMinTemperature.Visibility = ViewStates.Visible;
                    lobjViewHolder.txtMaxTemperature.Visibility = ViewStates.Visible;
                    lobjViewHolder.txtWeatherConditionLabel.Visibility = ViewStates.Gone;
                    
                    lobjView.Tag = lobjViewHolder;
                }

                lobjViewHolder.txtTime.Text = tempDailyWeather.Time.ToString(GlobalConstants.DateTimeFormat);
                lobjViewHolder.txtWeatherCondition.Text = tempDailyWeather.WeatherCondition;
                lobjViewHolder.txtMinTemperature.Text = tempDailyWeather.MinTemperature.ToString()+GlobalConstants.FahrenheitKeyword;
                lobjViewHolder.txtMaxTemperature.Text = tempDailyWeather.MaxTemperature.ToString()+GlobalConstants.FahrenheitKeyword;
                lobjViewHolder.txtHumidity.Text = tempDailyWeather.Humidity.ToString()+ GlobalConstants.PercentageSymbol;
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, GlobalConstants.ErrorMessage + ex.ToString(), ToastLength.Short);
            }

            return lobjView;
        }

        public override DayWeatherReport this[int position]
        {
            get {
                return dailyWeatherList[position];
            }
        }

       
        public override int Count
        {
            get {

                return dailyWeatherList.Count;
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
            public TextView txtWeatherConditionLabel { get; set; }
            public TextView txtMinTemperature { get; set; }
            public TextView txtMaxTemperature { get; set; }
            public TextView txtMinTemperatureLabel { get; set; }
            public TextView txtMaxTemperatureLabel { get; set; }
            public TextView txtHumidity { get; set; }
        }

        //public int GetCount()
        //{

        //    if (weatherResponse.Count() <= 0)
        //        return 1;
        //    return weatherResponse.Count();
        //}
    }
}