using DatabaseOperations.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseOperations.Services.GenerateDDL
{
    public class SystemObjectDto : IQueryResult
    {
        [Column("object_id")]
        public string Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("type_desc")]
        public string TypeDesc { get; set; }
    }
}
