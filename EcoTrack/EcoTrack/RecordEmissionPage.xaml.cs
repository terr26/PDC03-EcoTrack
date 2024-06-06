using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcoTrack
{
    public partial class RecordEmissionPage : ContentPage
    {
        public ObservableCollection<SustainableAction> Actions { get; set; }
        SustainableAction editingAction = null;
        StackLayout editingStack = null;

        public RecordEmissionPage()
        {
            InitializeComponent();
            Actions = new ObservableCollection<SustainableAction>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadSavedActions();
        }

        async Task LoadSavedActions()
        {
            var actions = await App.Database.GetActionsAsync();
            foreach (var action in actions)
            {
                DisplaySavedInputs(action);
                Actions.Add(action);
            }
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(descriptionEntry.Text) || categoryPicker.SelectedIndex == -1 || string.IsNullOrEmpty(impactLevelEntry.Text) || frequencyPicker.SelectedIndex == -1)
            {
                await DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }

            var category = (string)categoryPicker.SelectedItem;
            var frequency = (string)frequencyPicker.SelectedItem;

            if (editingAction != null)
            {
                editingAction.Description = descriptionEntry.Text;
                editingAction.Category = category;
                editingAction.ImpactLevel = impactLevelEntry.Text;
                editingAction.Frequency = frequency;

                await App.Database.SaveActionAsync(editingAction);
                DisplaySavedInputs(editingAction, editingStack);
                editingAction = null;
                editingStack = null;
            }
            else
            {
                var action = new SustainableAction
                {
                    ActionCode = $"T{Actions.Count + 1:D3}",
                    Description = descriptionEntry.Text,
                    Category = category,
                    ImpactLevel = impactLevelEntry.Text,
                    Frequency = frequency
                };

                await App.Database.SaveActionAsync(action);
                DisplaySavedInputs(action);
                Actions.Add(action);
            }

            ClearForm();
        }

        void DisplaySavedInputs(SustainableAction action, StackLayout stack = null)
        {
            if (stack == null)
            {
                stack = new StackLayout { Padding = new Thickness(0, 10) };
                savedInputsLayout.Children.Add(stack);
            }
            else
            {
                stack.Children.Clear();
            }

            stack.Children.Add(new Label { Text = $"Action Code: {action.ActionCode}", TextColor = Color.White });
            stack.Children.Add(new Label { Text = $"Description: {action.Description}", TextColor = Color.White });
            stack.Children.Add(new Label { Text = $"Category: {action.Category}", TextColor = Color.White });
            stack.Children.Add(new Label { Text = $"Impact Level: {action.ImpactLevel}", TextColor = Color.White });
            stack.Children.Add(new Label { Text = $"Frequency: {action.Frequency}", TextColor = Color.White });

            var editButton = new Button { Text = "Edit", BackgroundColor = Color.FromHex("#2AA198"), TextColor = Color.White, CornerRadius = 20, HorizontalOptions = LayoutOptions.FillAndExpand };
            editButton.Clicked += (s, e) => EditAction(action, stack);
            stack.Children.Add(editButton);

            var deleteButton = new Button { Text = "Delete", BackgroundColor = Color.FromHex("#2AA198"), TextColor = Color.White, CornerRadius = 20, HorizontalOptions = LayoutOptions.FillAndExpand };
            deleteButton.Clicked += async (s, e) =>
            {
                Actions.Remove(action);
                await App.Database.DeleteActionAsync(action);
                savedInputsLayout.Children.Remove(stack);
            };
            stack.Children.Add(deleteButton);
        }

        void EditAction(SustainableAction action, StackLayout stack)
        {
            descriptionEntry.Text = action.Description;
            categoryPicker.SelectedItem = action.Category;
            impactLevelEntry.Text = action.ImpactLevel;
            frequencyPicker.SelectedItem = action.Frequency;

            editingAction = action;
            editingStack = stack;
        }

        void ClearForm()
        {
            descriptionEntry.Text = string.Empty;
            categoryPicker.SelectedIndex = -1;
            impactLevelEntry.Text = string.Empty;
            frequencyPicker.SelectedIndex = -1;
        }
    }
}