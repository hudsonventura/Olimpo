namespace Olimpo;

public class Ativo
{
    public string name { get; set; }
    public string host { get; set; }
    public int port { get; set; }

    public Type type {get; set; }


    public enum Type{
        Ping,
        TCP
    }
}
