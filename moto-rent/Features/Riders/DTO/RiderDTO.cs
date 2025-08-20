namespace moto_rent.Features.Riders.DTOs
{
    public class RiderDto
    {

        public RiderDto(Rider rider)
        {
            identificador = rider.Id;
            nome = rider.Name;
            cnpj = rider.Cnpj;
            dataNascimento = rider.BirthDate;
            cnh = rider.Cnh;
            CnhCategory = rider.CnhCategory.ToString();
            imagemCnh = rider.ImageCnh;
        }

        public RiderDto() { }
        public string identificador { get; set; } = Guid.NewGuid().ToString("N");
        public string nome { get; set; } = string.Empty;
        public string cnpj { get; set; } = string.Empty;
        public DateTime dataNascimento { get; set; }
        public string cnh { get; set; } = string.Empty;
        public string CnhCategory { get; set; } = string.Empty;
        public string imagemCnh { get; set; } = string.Empty;
    }
}
