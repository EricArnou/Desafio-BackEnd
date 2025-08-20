namespace moto_rent.Features.Riders.DTOs
{
    public class RiderDto
    {

        public RiderDto(Rider rider)
        {
            identificador = rider.Id;
            nome = rider.Name;
            cnpj = rider.Cnpj;
            data_nascimento = rider.BirthDate;
            numero_cnh = rider.Cnh;
            tipo_cnh = rider.CnhCategory.ToString();
            imagem_cnh = rider.ImageCnh;
        }

        public RiderDto() { }
        public string identificador { get; set; } = Guid.NewGuid().ToString("N");
        public string nome { get; set; } = string.Empty;
        public string cnpj { get; set; } = string.Empty;
        public DateTime data_nascimento { get; set; }
        public string numero_cnh { get; set; } = string.Empty;
        public string tipo_cnh { get; set; } = string.Empty;
        public string imagem_cnh { get; set; } = string.Empty;
    }
}
