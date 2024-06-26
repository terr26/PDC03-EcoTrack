﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EcoTrack
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            InitializeAsync();
        }

        async void InitializeAsync()
        {
            await UpdateEmissionsAsync();
        }

        async void OnRecordEmissionClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecordEmissionPage());
        }

        void OnLearnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LearnPage());
        }

        void OnImpactClicked(object sender, EventArgs e)
        {
            DisplayAlert("Impact", "Highlighting the measurable impact of individual actions on the environment", "OK");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await UpdateEmissionsAsync();
        }

        async Task UpdateEmissionsAsync()
        {
            var actions = await App.Database.GetActionsAsync();
            double totalEmissions = actions.Sum(a => double.TryParse(a.ImpactLevel, out double impact) ? impact : 0.0);
            totalEmissionsLabel.Text = $"{totalEmissions:0.0}%";
            progressNumber.Text = $"{totalEmissions:0} kg";
            progressBar.Progress = totalEmissions / 180; // Assuming 100 kg CO2 is the goal for the month
        }
    }
}
