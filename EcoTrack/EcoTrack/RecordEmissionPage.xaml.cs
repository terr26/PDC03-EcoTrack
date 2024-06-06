using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcoTrack
{
    public partial class RecordEmissionPage : ContentPage
    {
        public RecordEmissionPage()
        {
            InitializeComponent();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var action = new SustainableAction
            {
                ActionCode = actionCodeEntry.Text,
                Description = descriptionEntry.Text,
                Category = categoryEntry.Text,
                ImpactLevel = impactLevelEntry.Text,
                Frequency = frequencyEntry.Text
            };

            await App.Database.SaveActionAsync(action);
            await Navigation.PopAsync();
        }
    }
}