using ClientCRUD.Models;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace ClientCRUD.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private IPageDialogService _dialogService;
        private INavigationService _navigationService;
        public LoginViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;

            SignInCommand = new DelegateCommand(SingIn);

        }


        public ICommand SignInCommand { get; private set; }
        private async void SingIn()
        {
            if (String.IsNullOrEmpty(TxtUser) || String.IsNullOrWhiteSpace(TxtUser))
            {
                await _dialogService.DisplayAlertAsync("Alerta", "Ingresa un email", "Ok");
                return;
            }

            if (String.IsNullOrEmpty(TxtPassword) || String.IsNullOrWhiteSpace(TxtPassword))
            {
                await _dialogService.DisplayAlertAsync("Alerta", "Ingresa la contraeña, porfavor", "Ok");
                return;
            }
            //TxtUser = "FORTEDEV";
            //TxtPassword = "Apply2019@pass";

            OverlayIndicator = true;
            var token = await Config.Config.PostWebService(new Usuario()
            {
                usuario = TxtUser,
                password = TxtPassword
            });

            if (token != null)
            {
                var response = JObject.Parse(token);
                var code = response.Value<int>("code");

                if (code == 200)
                {
                    var tokenstring = response.Value<JObject>("data");
                    var strToken = tokenstring.Value<string>("token");
                    if (App.Current.Properties.ContainsKey("token"))
                    {
                        App.Current.Properties["token"] = strToken;
                    }
                    else
                    {
                        App.Current.Properties.Add("token", strToken);
                    }
                    
                    await _navigationService.NavigateAsync("MainPage?token=" + strToken);
                }
                else
                {
                    var mensaje = response.Value<string>("message");
                    await _dialogService.DisplayAlertAsync("Alert", mensaje, "Ok");
                }
            }
            OverlayIndicator = false;
        }

        private string _TxtUser;

        public string TxtUser
        {
            get { return _TxtUser; }
            set { SetProperty(ref _TxtUser, value); }
        }

        private string _TxtPassword;

        public string TxtPassword
        {
            get { return _TxtPassword; }
            set { SetProperty(ref _TxtPassword, value); }
        }

        private bool _OverlayIndicator;
        public bool OverlayIndicator
        {
            get { return _OverlayIndicator; }
            set { SetProperty(ref _OverlayIndicator, value); }
        }

    }
}
