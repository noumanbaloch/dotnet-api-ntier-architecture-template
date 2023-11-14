using Breeze.Models.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Breeze.Models.Entities;

[Table(TableNames.OTP_CODES_TABLE)]
public class OTPCodeEntity : BaseEntity
{
    [Key]
    [Column(DbColumnNames.ID)]
    public int Id { get; set; }

    [Column(DbColumnNames.USER_NAME)]
    public required string UserName { get; set; }

    [Column(DbColumnNames.OTP_CODE)]
    public required string OTPCode { get; set; }

    [Column(DbColumnNames.EXPIRATION_TIME)]
    public DateTime ExpirationTime { get; set; }

    [Column(DbColumnNames.OTP_USE_CASE)]
    public required string OTPUseCase { get; set; }
}
