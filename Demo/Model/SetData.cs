using Demo.Model.Entitys;

namespace Demo.Model
{
    public class SetData
    {
        private ApplicationDBContext _applicationDBContext;
        public SetData(IWebHostEnvironment env, ApplicationDBContext applicationDBContext)
        {
           
            _applicationDBContext = applicationDBContext;
            if (env.IsEnvironment("test")) { innitMock(); }
            if (applicationDBContext == null)
            {
                throw new System.ArgumentNullException(nameof(applicationDBContext));
            }

            innit();
        }

        private void innitMock()
        {
            UserEntity userEntity = new();
            userEntity.UserCode = "001";
            userEntity.FullName = "xx xx";
            userEntity.LineId = "x";
            _applicationDBContext.UserEntitys.Add(userEntity);
            _applicationDBContext.SaveChanges();
        }

        private void innit()
        {
           
        }
    }
}
