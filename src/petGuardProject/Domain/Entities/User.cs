using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class User: User<Guid>
{
    public bool LockoutEnabled { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public int AccessFailedCount { get; set; }

    public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; } = default!;
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = default!;
    public virtual ICollection<OtpAuthenticator> OtpAuthenticators { get; set; } = default!;
    public virtual ICollection<EmailAuthenticator> EmailAuthenticators { get; set; } = default!;
}
