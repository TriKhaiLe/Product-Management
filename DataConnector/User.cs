using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DataConnector
{
    public class User
    {
        [Key]
        public Guid id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }

}
