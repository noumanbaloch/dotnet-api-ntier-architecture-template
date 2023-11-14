using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Breeze.Models.Entities;

[Table(TableNames.COLLEGES_TABLE)]
public class CollegeEntity : BaseEntity
{
    [Key]
    [Column(DbColumnNames.ID)]
    public int Id { get; set; }

    [Column(DbColumnNames.COLLEGE_NAME)]
    public required string CollegeName { get; set; }
}