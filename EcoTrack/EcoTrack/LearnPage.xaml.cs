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
        public ObservableCollection<ForumPost> ForumPosts { get; set; }
        ForumPost editingPost = null;

        public LearnPage()
        {
            InitializeComponent();
            ForumPosts = new ObservableCollection<ForumPost>();
            forumListView.ItemsSource = ForumPosts;
        }

        void OnSubmitClicked(object sender, EventArgs e)
        {
            if (editingPost != null)
            {
                editingPost.Content = forumEditor.Text;
                editingPost.Timestamp = DateTime.Now;
                forumListView.ItemsSource = null;
                forumListView.ItemsSource = ForumPosts;
                editingPost = null;
                forumEditor.Text = string.Empty;
                return;
            }

            if (!string.IsNullOrEmpty(forumEditor.Text))
            {
                ForumPosts.Add(new ForumPost
                {
                    User = "User",
                    Content = forumEditor.Text,
                    Timestamp = DateTime.Now
                });
                forumEditor.Text = string.Empty;
            }
        }

        void OnEditClicked(object sender, EventArgs e)
        {
            var post = (ForumPost)((Button)sender).CommandParameter;
            forumEditor.Text = post.Content;
            editingPost = post;
        }

        void OnDeleteClicked(object sender, EventArgs e)
        {
            var post = (ForumPost)((Button)sender).CommandParameter;
            ForumPosts.Remove(post);
        }
    }

    public class ForumPost
    {
        public string User { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}