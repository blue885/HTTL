using MVCModel;
using MVCDTO;


namespace MVCCore.Services
{
    public interface IGenericService<TEntity, TDto, TPrimitiveDto>

        where TEntity : class, IPrimitiveEntity, IBaseEntity, new()
        where TDto : class, TPrimitiveDto
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO

    {
        TEntity GetByID(int id);
        
        bool Editable(TDto dto);
        bool Save(TDto dto);
        bool Delete(int id);
    }
}
