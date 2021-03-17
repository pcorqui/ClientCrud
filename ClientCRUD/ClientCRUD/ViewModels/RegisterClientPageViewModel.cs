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
        bool update = true;
        string mensaje = "Cliente agregado";
        public RegisterClientPageViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;

            LoadPicker();
            RegisterCommand = new DelegateCommand(Registrar);
            ReturnCommand = new DelegateCommand(Return); }
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

            if (String.IsNullOrEmpty(TxtCreditLimit) || String.IsNullOrWhiteSpace(TxtCreditLimit))
            {
                await _dialogService.DisplayAlertAsync("Alerta", "Favor de introducir un limite de credito", "ok");
                return;
            }

            var credito = Math.Round(Double.Parse(TxtCreditLimit), 2);
            var status = _selectedItem.StatusID;

            if (update)
            {
                mensaje = "Cliente actualizado";
                client.nombreCompleto = TxtFullName;
                client.rfc = TxtRFC;
                client.telefonoMovil = TxtCellPhone;
                client.correoElectronico = TxtEmail;
                client.domicilio = TxtDomicilio;
                client.fechaNacimiento = DtBirdDate.ToString("O");
                client.limiteCredito = credito;
                client.estatusClienteId = SelectedItem.StatusID;
            }
            else
            {
                client = new Client()
                {
                    nombreCompleto = TxtFullName,
                    rfc = TxtRFC,
                    telefonoMovil = TxtCellPhone,
                    correoElectronico = TxtEmail,
                    domicilio = TxtDomicilio,
                    fechaNacimiento = DtBirdDate.ToString("O"),
                    limiteCredito = credito,
                    estatusClienteId = SelectedItem.StatusID,

                };
            }
            //client = new Client()
            //{
            //    nombreCompleto = "Paul Cortes",
            //    rfc = "hdhdyf",
            //    telefonoMovil = "2711522808",
            //    correoElectronico = "yenia94@hotmail.com",
            //    domicilio = "Mexico magico",
            //    fechaNacimiento = DtBirdDate.ToString("O"),
            //    limiteCredito = 200,
            //    estatusClienteId = 1,
            //};

            var result = await Config.Config.SaveOrUpdateClient(client, update);
            switch (result)
            {
                case 200:
                    await _dialogService.DisplayAlertAsync("Informacion", "Cliente actualizado", "Ok");
                    Return();
                    break;
                case 400:
                    await _dialogService.DisplayAlertAsync("Informacion", "El(la) cliente que intentas agregar o actualizar ya existe en la base de datos", "Ok");
                    break;
            }
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

            if (parameters.ContainsKey("update"))
            {
                update = false;
            }

            if (parameters.ContainsKey("cliente"))
            {
                client = parameters.GetValue<Client>("cliente");
                TxtFullName = client.nombreCompleto;
                TxtRFC = client.rfc;
                TxtEmail = client.correoElectronico;
                TxtCellPhone = client.telefonoMovil;
                TxtDomicilio = client.domicilio;
                TxtCreditLimit = client.limiteCredito + "";
                SelectedItem = Status[client.estatusClienteId];
            }
        }

        //cargar Picker
        public void LoadPicker()
        {
            Status = new List<PickerObject>();
            Status.Add(new PickerObject { StatusID = 0, NameStatus="Desactivado"});
            Status.Add(new PickerObject { StatusID = 1, NameStatus="Activado"});

        }

        private PickerObject _selectedItem;
        public PickerObject SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        //nombre
        private string _TxtFullName;

        public string TxtFullName
        {
            get { return _TxtFullName; }
            set { SetProperty(ref _TxtFullName, value); }
        }

        //email
        private string _TxtEmail;
        public string TxtEmail
        {
            get { return _TxtEmail; }
            set { SetProperty(ref _TxtEmail, value); }
        }

        private string _TxtPassword;
        //password
        public string TxtPassword
        {
            get { return _TxtPassword; }
            set { SetProperty(ref _TxtPassword, value); }
        }


        //Fecha de Nacimiento
        private string _TxtFechaDeNacimiento;
        public string TxtFechaDeNacimiento
        {
            get { return _TxtFechaDeNacimiento; }
            set { SetProperty(ref _TxtFechaDeNacimiento, value); }
        }

        //Status Persona
        private int _TxtStatus;
        public int TxtStatus
        {
            get { return _TxtStatus; }
            set { SetProperty(ref _TxtStatus, value); }
        }

        //Limite de credito
        private string _TxtCreditLimit;
        public string TxtCreditLimit
        {
            get { return _TxtCreditLimit; }
            set { SetProperty(ref _TxtCreditLimit, value); }
        }

        //RFC
        private string _TxtRFC;
        public string TxtRFC
        {
            get { return _TxtRFC; }
            set { SetProperty(ref _TxtRFC, value); }
        }

        //CellPhone
        private string _TxtCellPhone;
        public string TxtCellPhone
        {
            get { return _TxtCellPhone; }
            set { SetProperty(ref _TxtCellPhone, value); }
        }

        //Domicilio
        private string _TxtDomicilio;
        public string TxtDomicilio
        {
            get { return _TxtDomicilio; }
            set { SetProperty(ref _TxtDomicilio, value); }
        }

        private DateTime _MaximumDate = DateTime.Now;
        public DateTime MaximumDate
        {
            get { return _MaximumDate; }
            set { SetProperty(ref _MaximumDate, value); }
        }

        private DateTimeOffset _DtBirdDate = DateTime.Now;
        public DateTimeOffset DtBirdDate
        {
            get { return _DtBirdDate; }
            set { SetProperty(ref _DtBirdDate, value); }
        }

        private List<PickerObject> _Picker;
        public List<PickerObject> Status
        {
            get { return _Picker; }
            set { SetProperty(ref _Picker, value); }
        }
    }
}
