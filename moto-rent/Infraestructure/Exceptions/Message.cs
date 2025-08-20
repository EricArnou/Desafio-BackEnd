namespace moto_rent.Infraestructure.Exceptions;


public class Message
{
    public string Mensagem { get; set; }

    public Message(string mensagem)
    {
        Mensagem = mensagem;
    }
}