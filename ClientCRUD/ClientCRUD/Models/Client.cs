namespace ClientCRUD.Models
{
    public class Client
    {
        public int clienteId { get; set; } = 0;
        public int Edad { get; set; }
        public string nombreCompleto { get; set; }
        public string rfc { get; set; }
        public string fechaNacimiento { get; set; }
        public string correoElectronico { get; set; }
        public string telefonoMovil { get; set; }
        public string domicilio { get; set; }
        public double limiteCredito { get; set; }
        public int estatusClienteId { get; set; }
    }
}
