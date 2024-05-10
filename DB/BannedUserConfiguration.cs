using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ns_DB
{
    public class BannedUserConfiguration : IEntityTypeConfiguration<BannedUser>
    {
        public void Configure(EntityTypeBuilder<BannedUser> builder)
        {
            builder.HasKey(e => e.IP);
        }
    }
}
