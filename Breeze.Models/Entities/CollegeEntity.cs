using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Breeze.Models.Entities;

public class CollegeEntity : BaseEntity
{
    public int Id { get; set; }

    [Column(DbColumnNames.COLLEGE_NAME)]
    public required string CollegeName { get; set; }
}