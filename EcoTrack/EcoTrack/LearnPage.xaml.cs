using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace EcoTrack
{
    public partial class LearnPage : ContentPage
    {
        public ObservableCollection<Article> Articles { get; set; }

        public LearnPage()
        {
            InitializeComponent();

            Articles = new ObservableCollection<Article>
            {
                new Article
                {
                    Title = "Engage Your Employees in Sustainability This Earth Month",
                    Description = "Earth Month occurs in April every year, marking the month when Earth Day falls. Earth Month is a time to raise awareness about the...",
                    TimeAgo = "about a year ago",
                    ReadTime = "83 read"
                },
                new Article
                {
                    Title = "How to Make Sustainability Part of Your Company Culture",
                    Description = "According to a recent survey of CEOs by the United Nations Global Compact and Accenture, 93% of CEOs see sustainability as important to...",
                    TimeAgo = "about a year ago",
                    ReadTime = "77 read"
                },
                new Article
                {
                    Title = "7 Sustainability Certifications to Consider for Your Business",
                    Description = "As climate consciousness among employees grows, so does the interest in certification programs that validate sustainable practices...",
                    TimeAgo = "about a year ago",
                    ReadTime = "77 read"
                }
            };

            articlesListView.ItemsSource = Articles;
        }
    }

    public class Article
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TimeAgo { get; set; }
        public string ReadTime { get; set; }
    }
}