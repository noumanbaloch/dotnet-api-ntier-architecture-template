using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Breeze.Models.Entities;

[Table(TableNames.BOARD_DETAILS_TABLE)]
public class BoardDetailEntity : BaseEntity
{
    [Key]
    [Column(DbColumnNames.ID)]
    public int Id { get; set; }

    [Column(DbColumnNames.BOARD_NAME)]
    public required string BoardName { get; set; }
}
