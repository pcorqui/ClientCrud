using ClientCRUD.Models;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientCRUD.ViewModels
{
    public class MainPageViewModel : ViewModelBase,INavigatedAware
    {
        INavigationService _navigationService;
        IPageDialogService _dialogService;
        string _token;
        public ObservableCollection<Client> ClientList { get; set; }
        public MainPageViewModel(INavigationService navigationService,IPageDialogService dialogService)
            : base(navigationService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;
            
            ClientList = new ObservableCollection<Client>();
            Title = "Clientes";

            DeleteCommand = new DelegateCommand<Client>(Delete);
            UpdateCommand = new DelegateCommand<Client>(Update);
            CreateCommand = new DelegateCommand(Create);

        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            _token = parameters.GetValue<string>("token");
            await ShowClient();
        }

        public ICommand CreateCommand { get; private set; }
        private async void Create()
        {
            await _navigationService.NavigateAsync("RegisterClientPage");
        }

        public ICommand DeleteCommand { get; private set; }
        private async void Delete(Client client)
        {
            var result = await _dialogService.DisplayAlertAsync("Alerta", "¿Deseas borrar este cliente?", "Si", "No");
            if (result)
            {
                //borrar con el el ws
                
               var status = await Config.Config.DeleteClient(client.ClientID);
                if(status == 200)
                {
                    await _dialogService.DisplayAlertAsync("Informacion", "Cliente eliminado", "Ok");
                    ClientList.Remove(client);
                }

            }
        }
        
        public ICommand UpdateCommand { get; private set; }
        private async void Update(Client client)
        {
            var navParams = new NavigationParameters()
            {
                {"cliente",client }
            };
            await _navigationService.NavigateAsync("RegisterClientPage",navParams);
        }

            
        public async Task ShowClient()
        {
            ClientList.Clear();
            var result = await Config.Config.GetWebServiceWithToken("/clientes");
            var json = JObject.Parse(result);
            var code = json.Value<int>("code");

            if(code == 200)
            {
                var data = json.Value<JArray>("data");

                foreach (var item in data)
                {
                    ClientList.Add(new Client()
                    {

                        ClientID = item.Value<int>("clienteId") + "",
                        FullName = item.Value<string>("nombreCompleto"),                                                
                        Email = item.Value<string>("correoElectronico"),
                        CreditLimite = item.Value<double>("limiteCredito"),
                        StatusClient = item.Value<int>("estatusClienteId"),
                        Edad = item.Value<int>("edad")

                    });
                    RaisePropertyChanged("ClientList");
                }                 
            }
            

            RaisePropertyChanged("ClientList");
        }
    }
}
