using Breeze.Models.Constants;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Breeze.Models.Entities;

[Table(TableNames.USERS_TABLE)]
public class UserEntity : IdentityUser<int>
{
    [Column(DbColumnNames.FIRST_NAME)]
    public required string FirstName { get; set; }

    [Column(DbColumnNames.LAST_NAME)]
    public required string LastName { get; set; }

    [Column(DbColumnNames.GENDER)]
    public string? Gender { get; set; }

    [Column(DbColumnNames.USER_HANDLE)]
    public required string UserHandle { get; set; }

    [Column(DbColumnNames.TRUSTED_DEVICE_ID)]
    public required string TrustedDeviceId { get; set; }

    [Column(DbColumnNames.ACCEPTED_TERMS_AND_CONDITIONS)]
    public bool AcceptedTermsAndConditions { get; set; }

    [Column(DbColumnNames.CREATED_BY)]
    public required string CreatedBy { get; set; }

    [Column(DbColumnNames.CREATED_DATE)]
    public DateTime CreatedDate { get; set; }

    [Column(DbColumnNames.MODIFIED_BY)]
    public string? ModifiedBy { get; set; }

    [Column(DbColumnNames.MODIFIED_DATE)]
    public DateTime? ModifiedDate { get; set; }

    [Column(DbColumnNames.DELETED)]
    public bool Deleted { get; set; } = false;

    [Column(DbColumnNames.ROW_VERSION)]
    [Timestamp]
    public byte[] RowVersion { get; set; } = null!;
}