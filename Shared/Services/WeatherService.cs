﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Shared.Services
{
    public class WeatherService
    {
        private readonly string _outsideUrl = "https://api.openweathermap.org/data/2.5/weather?lat=59.325&lon=18.05&appid=139cfff42c01e754ed06e2fcf2b44031";
        private readonly string _insideUrl = "http://localhost:7093/api/GetLatestInsideTemperature";
        private readonly System.Timers.Timer _timer;
        private readonly HttpClient _http;

        public string? CurrentWeatherCondition { get; private set; }
        public string? CurrentOutsideTemperature { get; private set; }
        public string? CurrentInsideTemperature { get; private set; }
        public event Action? WeatherUpdated;


        public WeatherService(HttpClient http)
        {
            _http = http;
            Task.Run(SetCurrentWeatherAsync);

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += async (s, e) => await SetCurrentWeatherAsync();
            _timer.Start();
        }

        private async Task SetCurrentWeatherAsync()
        {
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(await _http.GetStringAsync(_outsideUrl));
                CurrentOutsideTemperature = (data!.main.temp - 273.15).ToString("#");
                CurrentWeatherCondition = GetWeatherConditionIcon(data!.weather[0].description.ToString());

            }
            catch
            {
                CurrentOutsideTemperature = "--";
                CurrentWeatherCondition = GetWeatherConditionIcon("--");
            }

            await GetInsideTemperature();

            WeatherUpdated?.Invoke();
        }

        private string GetWeatherConditionIcon(string value)
        {
            return value switch
            {
                "clear sky" => "\uf185",
                "few clouds" => "\uf6c4",
                "overcast clouds" => "\uf1be",
                "scattered clouds" => "\uf0c2",
                "broken clouds" => "\uf1be",
                "shower rain" => "\uf73d",
                "rain" => "\uf740",
                "thunderstorm" => "\uf76c",
                "snow" => "\uf2dc",
                "mist" => "\uf75f",
                _ => "\uf5c7",
            };
        }

        private async Task GetInsideTemperature()
        {
            try
            {
                await Task.Delay(2000);
                var data = JsonConvert.DeserializeObject<dynamic>(await _http.GetStringAsync(_insideUrl));
                CurrentInsideTemperature = data!.Temperature.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                CurrentInsideTemperature = "--";
            }
        }
    }
}
