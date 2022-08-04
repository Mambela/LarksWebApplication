using System.ComponentModel.DataAnnotations.Schema;

namespace LarksCoreApi.Model.BaseModels
{
    public class IdentityBaseEntity :TimeStampsEntity
    {
        [Column(Order = 0)]
        public int Id { get; set; }

        [Column(Order = 1)]
        public string Designation { get; set; }
    }
}
