using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcoTrack
{
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        async void OnLetsGoButtonClicked(object sender, EventArgs e)
        {
            // Navigate to MainPage
            await Navigation.PushAsync(new MainPage());
        }
    }
}