using ClientCRUD.Models;
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
    public class RegisterClientPageViewModel : BindableBase, INavigationAware
    {
        IPageDialogService _dialogService;
        INavigationService _navigationService;
        Client client;
        public RegisterClientPageViewModel(INavigationService navigationService,IPageDialogService dialogService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;

            RegisterCommand = new DelegateCommand(Registrar);
            ReturnCommand = new DelegateCommand(Return);       }
        public ICommand RegisterCommand { get; set; }
        public async void Registrar()
        {
            //validaciones
            if (String.IsNullOrEmpty(TxtFullName) || String.IsNullOrWhiteSpace(TxtFullName))
            {
                await _dialogService.DisplayAlertAsync("Alerta", "Favor de introducir su nombre", "ok");
                return;
            }

            if (String.IsNullOrEmpty(TxtRFC) || String.IsNullOrWhiteSpace(TxtRFC))
            {
                await _dialogService.DisplayAlertAsync("Alerta", "Favor de introducir su RFC", "ok");
                return;
            }

            if (String.IsNullOrEmpty(TxtCellPhone) || String.IsNullOrWhiteSpace(TxtCellPhone))
            {
                await _dialogService.DisplayAlertAsync("Alerta", "Favor de introducir su Numero de celular", "ok");
                return;
            }

            if (String.IsNullOrEmpty(TxtEmail) || String.IsNullOrWhiteSpace(TxtEmail))
            {
                await _dialogService.DisplayAlertAsync("Alerta", "Favor de introducir su email", "ok");
                return;
            }

            if (String.IsNullOrEmpty(TxtPassword) || String.IsNullOrWhiteSpace(TxtPassword))
            {
                await _dialogService.DisplayAlertAsync("Alerta", "Favor de introducir su password", "ok");
                return;
            }
            
            if (String.IsNullOrEmpty(TxtDomicilio) || String.IsNullOrWhiteSpace(TxtDomicilio))
            {
                await _dialogService.DisplayAlertAsync("Alerta", "Favor de introducir su domicilio", "ok");
                return;
            }

            //if (String.IsNullOrEmpty(TxtCreditLimit) || String.IsNullOrWhiteSpace(TxtCreditLimit))
            //{
            //    await _dialogService.DisplayAlertAsync("Alerta", "Favor de introducir un limite de credito", "ok");
            //    return;
            //}
            client = new Client()
            {
                FullName = TxtFullName,
                RFC = TxtRFC,
                CellPhone = TxtCellPhone,
                Email = TxtEmail,
                Domicilio = TxtDomicilio,
                CreditLimite = TxtCreditLimit,
                StatusClient = 1,
                    
            };
            var token = App.Current.Properties["token"] as string;
            await Config.Config.SaveOrUpdateClient(client, true);
        }

        public ICommand ReturnCommand { get; set; }
        public void Return()
        {
            //here is the WS

            _navigationService.NavigateAsync("../");
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("cliente"))
            {
                client = parameters.GetValue<Client>("cliente");
                TxtFullName = client.FullName;
                TxtRFC = client.RFC;
                TxtEmail = client.Email;
                TxtCellPhone = client.CellPhone;
                TxtDomicilio = client.Domicilio;
                TxtCreditLimit = client.CreditLimite;
                TxtStatus = client.StatusClient;
            }           
        }

        //nombre
        private string _TxtFullName;

        public string TxtFullName
        {
            get { return _TxtFullName; }
            private set { SetProperty(ref _TxtFullName, value); }
        }

        //email
        private string _TxtEmail;
        public string TxtEmail
        {
            get { return _TxtEmail; }
            private set { SetProperty(ref _TxtEmail, value); }
        }

        private string _TxtPassword;
        //password
        public string TxtPassword
        {
            get { return _TxtPassword; }
            private set { SetProperty(ref _TxtPassword, value); }
        }


        //Fecha de Nacimiento
        private string _TxtFechaDeNacimiento;
        public string TxtFechaDeNacimiento
        {
            get { return _TxtFechaDeNacimiento; }
            private set { SetProperty(ref _TxtFechaDeNacimiento, value); }
        }

        //Status Persona
        private int _TxtStatus;
        public int TxtStatus
        {
            get { return _TxtStatus; }
            private set { SetProperty(ref _TxtStatus, value); }
        }

        //Limite de credito
        private double _TxtCreditLimit;
        public double TxtCreditLimit
        {
            get { return _TxtCreditLimit; }
            private set { SetProperty(ref _TxtCreditLimit, value); }
        } 
        
        //RFC
        private string _TxtRFC;
        public string TxtRFC
        {
            get { return _TxtRFC; }
            private set { SetProperty(ref _TxtRFC, value); }
        } 
        
        //CellPhone
        private string _TxtCellPhone;
        public string TxtCellPhone
        {
            get { return _TxtCellPhone; }
            private set { SetProperty(ref _TxtCellPhone, value); }
        }
        
        //Domicilio
        private string _TxtDomicilio;
        public string TxtDomicilio
        {
            get { return _TxtDomicilio; }
            private set { SetProperty(ref _TxtDomicilio, value); }
        }

    }
}
