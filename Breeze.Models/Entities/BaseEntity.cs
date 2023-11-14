using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Breeze.Models.Entities;

public class BaseEntity
{
    [Column(DbColumnNames.CREATED_BY)]
    public required string CreatedBy { get; set; }

    [Column(DbColumnNames.CREATED_DATE)]
    public DateTime CreatedDate { get; set; }

    [Column(DbColumnNames.MODIFIED_BY)]
    public string? ModifiedBy { get; set; }

    [Column(DbColumnNames.MODIFIED_DATE)]
    public DateTime? ModifiedDate { get; set; }

    [Column(DbColumnNames.DELETED)]
    public bool Deleted { get; set; }

    [Column(DbColumnNames.ROW_VERSION)]
    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;
}