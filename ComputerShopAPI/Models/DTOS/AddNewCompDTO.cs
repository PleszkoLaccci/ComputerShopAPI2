using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerShopAPI.Models.DTOS
{
    public class AddNewCompDTO
    {
        public string? Brand { get; set; }
        public string? Type { get; set; }
        public double Display { get; set; }
        public int Memory { get; set; }
        public Guid OsId { get; set; }
    }
}
