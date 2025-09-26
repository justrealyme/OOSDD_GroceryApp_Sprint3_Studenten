using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Text.RegularExpressions;

namespace Grocery.App.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        private readonly IClientService _clientService;
        private readonly GlobalViewModel _global;
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private string name = "";

        [ObservableProperty]
        private string email = "";

        [ObservableProperty]
        private string password = "";

        [ObservableProperty]
        private string confirmPassword = "";

        [ObservableProperty]
        private string registerMessage = "";

        [ObservableProperty]
        private Color messageColor = Colors.Red;

        public RegisterViewModel(IClientService clientService, GlobalViewModel global, IServiceProvider serviceProvider)
        {
            _clientService = clientService;
            _global = global;
            _serviceProvider = serviceProvider;
        }

        [RelayCommand]
        private async Task Register()
        {
            // Reset message
            RegisterMessage = "";

            // Validate input
            if (!ValidateInput())
                return;

            try
            {
                // Check if email already exists
                var existingClient = _clientService.Get(Email);
                if (existingClient != null)
                {
                    RegisterMessage = "Er bestaat al een account met dit e-mailadres.";
                    MessageColor = Colors.Red;
                    return;
                }

                // Create new client
                var newClient = _clientService.Register(Name, Email, Password);
                if (newClient != null)
                {
                    RegisterMessage = "Account succesvol aangemaakt! Je wordt doorgestuurd naar het inlogscherm.";
                    MessageColor = Colors.Green;

                    // Wait a moment to show success message
                    await Task.Delay(2000);

                    // Navigate back to login using MainPage
                    var loginView = _serviceProvider.GetService<LoginView>();
                    if (loginView != null)
                    {
                        Application.Current.MainPage = loginView;
                    }
                }
                else
                {
                    RegisterMessage = "Er ging iets mis bij het aanmaken van het account.";
                    MessageColor = Colors.Red;
                }
            }
            catch (Exception ex)
            {
                RegisterMessage = $"Fout: {ex.Message}";
                MessageColor = Colors.Red;
            }
        }

        [RelayCommand]
        private void GoToLogin()
        {
            // Navigate back to login using MainPage
            var loginView = _serviceProvider.GetService<LoginView>();
            if (loginView != null)
            {
                Application.Current.MainPage = loginView;
            }
        }

        private bool ValidateInput()
        {
            // Check if all fields are filled
            if (string.IsNullOrWhiteSpace(Name))
            {
                RegisterMessage = "Vul je naam in.";
                MessageColor = Colors.Red;
                return false;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                RegisterMessage = "Vul je e-mailadres in.";
                MessageColor = Colors.Red;
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                RegisterMessage = "Vul een wachtwoord in.";
                MessageColor = Colors.Red;
                return false;
            }

            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                RegisterMessage = "Bevestig je wachtwoord.";
                MessageColor = Colors.Red;
                return false;
            }

            // Validate email format
            if (!IsValidEmail(Email))
            {
                RegisterMessage = "Vul een geldig e-mailadres in.";
                MessageColor = Colors.Red;
                return false;
            }

            // Check password length
            if (Password.Length < 6)
            {
                RegisterMessage = "Het wachtwoord moet minimaal 6 karakters lang zijn.";
                MessageColor = Colors.Red;
                return false;
            }

            // Check if passwords match
            if (Password != ConfirmPassword)
            {
                RegisterMessage = "De wachtwoorden komen niet overeen.";
                MessageColor = Colors.Red;
                return false;
            }

            return true;
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Simple regex for email validation
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }
    }
}