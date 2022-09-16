using Demo.Model.Entitys;
using Demo.Model.Interface;

namespace Demo.Model.Repository
{
    public class UserRepository : IUserRepository
    {
       
        private readonly IHttpClientFactory _clientFactory;
        private ApplicationDBContext _applicationDBContext;
        public UserRepository(ApplicationDBContext applicationDBContext, IHttpClientFactory clientFactory)
        {
            if (applicationDBContext == null)
            {
                throw new System.ArgumentNullException(nameof(applicationDBContext));
            }
            _applicationDBContext = applicationDBContext;
            _clientFactory = clientFactory;
         
        }
        public async Task<UserEntity> getUserEntity(string lineId)
        {

            UserEntity userEntity = _applicationDBContext.UserEntitys.Where(w => w.LineId == lineId && w.IsDeactivate == false).FirstOrDefault();

            return userEntity;
        }
    }
}
