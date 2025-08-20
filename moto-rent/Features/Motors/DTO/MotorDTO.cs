namespace moto_rent.Features.Motors.DTOs
{
    public class MotorDto
    {

        public MotorDto(Motor motor)
        {
            identificador = motor.Id;
            ano = motor.Year;
            modelo = motor.Model;
            placa = motor.LicensePlate;
        }

        public MotorDto() {}
        public string identificador { get; set; } = Guid.NewGuid().ToString("N");
        public int ano { get; set; }
        public string modelo { get; set; }
        public string placa { get; set; }
    }

    public class UpdateMotorDto
    {
        public string placa { get; set; }
    }
}
