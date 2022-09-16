using Demo.Model.Entitys;

namespace Demo.Model.Interface
{
    public interface IUserRepository
    {
        Task<UserEntity> getUserEntity(String lineId);
    }
}
