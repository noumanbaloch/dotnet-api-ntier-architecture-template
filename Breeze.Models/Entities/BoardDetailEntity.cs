using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Breeze.Models.Entities;

public class BoardDetailEntity : BaseEntity
{
    public int Id { get; set; }

    [Column(DbColumnNames.BOARD_NAME)]
    public required string BoardName { get; set; }
}
