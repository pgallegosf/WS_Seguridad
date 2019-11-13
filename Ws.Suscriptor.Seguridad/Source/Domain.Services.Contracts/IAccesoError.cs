using Domain.Entities;

namespace Domain.Services.Contracts
{
    public interface IAccesoError
    {
        bool Add(EError error);
    }
}
