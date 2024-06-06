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
            LoadActionsAsync();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(descriptionEntry.Text) ||
                string.IsNullOrWhiteSpace(categoryPicker.SelectedItem?.ToString()) ||
                string.IsNullOrWhiteSpace(impactLevelEntry.Text) ||
                string.IsNullOrWhiteSpace(frequencyPicker.SelectedItem?.ToString()))
            {
                await DisplayAlert("Validation Error", "All fields must be filled.", "OK");
                return;
            }

            if (editingAction != null)
            {
                editingAction.Description = descriptionEntry.Text;
                editingAction.Category = categoryPicker.SelectedItem.ToString();
                editingAction.ImpactLevel = impactLevelEntry.Text;
                editingAction.Frequency = frequencyPicker.SelectedItem.ToString();

                await App.Database.SaveActionAsync(editingAction);
                UpdateDisplayedInputs(editingStack, editingAction);
                editingAction = null;
                editingStack = null;
            }
            else
            {
                var actionCode = await GenerateNextActionCodeAsync();
                var action = new SustainableAction
                {
                    ActionCode = actionCode,
                    Description = descriptionEntry.Text,
                    Category = categoryPicker.SelectedItem.ToString(),
                    ImpactLevel = impactLevelEntry.Text,
                    Frequency = frequencyPicker.SelectedItem.ToString()
                };

                await App.Database.SaveActionAsync(action);
                DisplaySavedInputs(action);
            }

            ClearForm();
        }

        async Task<string> GenerateNextActionCodeAsync()
        {
            var actions = await App.Database.GetActionsAsync();
            if (actions.Count == 0) return "T001";

            var lastActionCode = actions.Max(a => a.ActionCode);
            int lastNumber = int.Parse(lastActionCode.Substring(1));
            return $"T{lastNumber + 1:D3}";
        }

        async void LoadActionsAsync()
        {
            var actions = await App.Database.GetActionsAsync();
            foreach (var action in actions)
            {
                DisplaySavedInputs(action);
            }
        }

        void DisplaySavedInputs(SustainableAction action)
        {
            var stack = new StackLayout { Padding = new Thickness(0, 10) };
            UpdateDisplayedInputs(stack, action);

            savedInputsLayout.Children.Add(stack);
        }

        void UpdateDisplayedInputs(StackLayout stack, SustainableAction action)
        {
            stack.Children.Clear();

            stack.Children.Add(new Label { Text = $"Action Code: {action.ActionCode}" });
            stack.Children.Add(new Label { Text = $"Description: {action.Description}" });
            stack.Children.Add(new Label { Text = $"Category: {action.Category}" });
            stack.Children.Add(new Label { Text = $"Impact Level: {action.ImpactLevel}" });
            stack.Children.Add(new Label { Text = $"Frequency: {action.Frequency}" });

            var editButton = new Button { Text = "Edit" };
            editButton.Clicked += (s, e) => EditAction(action, stack);
            stack.Children.Add(editButton);

            var deleteButton = new Button { Text = "Delete" };
            deleteButton.Clicked += async (s, e) =>
            {
                await App.Database.DeleteActionAsync(action);
                savedInputsLayout.Children.Remove(stack);
            };
            stack.Children.Add(deleteButton);
        }

        void EditAction(SustainableAction action, StackLayout stack)
        {
            actionCodeEntry.Text = action.ActionCode;
            descriptionEntry.Text = action.Description;
            categoryPicker.SelectedItem = action.Category;
            impactLevelEntry.Text = action.ImpactLevel;
            frequencyPicker.SelectedItem = action.Frequency;

            editingAction = action;
            editingStack = stack;
        }

        void ClearForm()
        {
            actionCodeEntry.Text = string.Empty;
            descriptionEntry.Text = string.Empty;
            categoryPicker.SelectedItem = null;
            impactLevelEntry.Text = string.Empty;
            frequencyPicker.SelectedItem = null;
        }
    }
}