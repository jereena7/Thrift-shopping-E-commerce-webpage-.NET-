namespace ThriftShop.DataAccessLayer
{
    public abstract class BaseDAL
    {       
            protected readonly string _connectionString;

            public BaseDAL(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }
        
    }
}
