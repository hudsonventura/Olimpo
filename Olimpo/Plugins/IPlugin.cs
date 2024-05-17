using Olimpo;

namespace Olimpo.Plugins;

public interface IPlugin
{
    public Task<Result> Test(Ativo ativo);
}
