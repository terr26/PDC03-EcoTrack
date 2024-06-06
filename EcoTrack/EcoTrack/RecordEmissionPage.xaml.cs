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

        public RecordEmissionPage()
        {
            InitializeComponent();
            Actions = new ObservableCollection<SustainableAction>();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (editingAction != null)
            {
                editingAction.ActionCode = actionCodeEntry.Text;
                editingAction.Description = descriptionEntry.Text;
                editingAction.Category = categoryEntry.Text;
                editingAction.ImpactLevel = impactLevelEntry.Text;
                editingAction.Frequency = frequencyEntry.Text;

                await App.Database.SaveActionAsync(editingAction);
                DisplaySavedInputs(editingAction);
                editingAction = null;
            }
            else
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
                DisplaySavedInputs(action);
            }

            ClearForm();
        }

        void DisplaySavedInputs(SustainableAction action)
        {
            var stack = new StackLayout { Padding = new Thickness(0, 10) };
            stack.Children.Add(new Label { Text = $"Action Code: {action.ActionCode}" });
            stack.Children.Add(new Label { Text = $"Description: {action.Description}" });
            stack.Children.Add(new Label { Text = $"Category: {action.Category}" });
            stack.Children.Add(new Label { Text = $"Impact Level: {action.ImpactLevel}" });
            stack.Children.Add(new Label { Text = $"Frequency: {action.Frequency}" });

            var editButton = new Button { Text = "Edit" };
            editButton.Clicked += (s, e) => EditAction(action);
            stack.Children.Add(editButton);

            var deleteButton = new Button { Text = "Delete" };
            deleteButton.Clicked += async (s, e) =>
            {
                Actions.Remove(action);
                await App.Database.DeleteActionAsync(action);
                savedInputsLayout.Children.Remove(stack);
            };
            stack.Children.Add(deleteButton);

            savedInputsLayout.Children.Add(stack);
        }

        void EditAction(SustainableAction action)
        {
            actionCodeEntry.Text = action.ActionCode;
            descriptionEntry.Text = action.Description;
            categoryEntry.Text = action.Category;
            impactLevelEntry.Text = action.ImpactLevel;
            frequencyEntry.Text = action.Frequency;

            editingAction = action;
        }

        void ClearForm()
        {
            actionCodeEntry.Text = string.Empty;
            descriptionEntry.Text = string.Empty;
            categoryEntry.Text = string.Empty;
            impactLevelEntry.Text = string.Empty;
            frequencyEntry.Text = string.Empty;
        }
    }
}