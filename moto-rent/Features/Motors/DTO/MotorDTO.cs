namespace moto_rent.Features.Motors.DTOs
{
    public class MotorDto
    {
        public string Id { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
    }

    public class CreateMotorDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public int Year { get; set; }
        public string Model { get; set; }
        public string LicensePlate { get; set; }
    }

    public class UpdateMotorDto
    {
        public string LicensePlate { get; set; }
    }
}
