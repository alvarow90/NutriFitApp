using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace NutriFitApp.Mobile.ViewModels;

public partial class ChatViewModel : ObservableObject
{
    [ObservableProperty]
    private string newMessage;

    [ObservableProperty]
    private ObservableCollection<string> messages = new();

    [RelayCommand]
    private void SendMessage()
    {
        if (!string.IsNullOrWhiteSpace(NewMessage))
        {
            Messages.Add($"Tú: {NewMessage}");
            NewMessage = string.Empty;
        }
    }
}
