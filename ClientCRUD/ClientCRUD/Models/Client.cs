using System;
using System.Collections.Generic;
using System.Text;

namespace ClientCRUD.Models
{
    public class Client
    {
        public string ClientID { get; set; }
        public int Edad { get; set; }
        public string FullName { get; set; }
        public string RFC { get; set; }
        public string BirdDate { get; set; }
        public string Email { get; set; }
        public string CellPhone { get; set; }
        public string Domicilio { get; set; }
        public double CreditLimite { get; set; }
        public int StatusClient { get; set; }
    }
}
