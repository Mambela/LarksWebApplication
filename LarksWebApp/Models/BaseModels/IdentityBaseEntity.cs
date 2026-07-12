using System.ComponentModel.DataAnnotations.Schema;

namespace LarksWebApp.Model.BaseModels
{
    public class IdentityBaseEntity :TimeStampsEntity
    {
        [Column(Order = 0)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column(Order = 1)]
        public string Designation { get; set; }
    }
}
