
using JobInja.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobInja.Api.Data.EfConfigurations;

public class ProvinceEntityConfiguration : IEntityTypeConfiguration<Province>
{
    private static readonly DateTime SeedDate = new(2026, 1, 1);

    private static readonly string[] ProvinceNames =
    [
        "آذربایجان شرقی", "آذربایجان غربی", "اردبیل", "اصفهان", "البرز",
        "ایلام", "بوشهر", "تهران", "چهارمحال و بختیاری", "خراسان جنوبی",
        "خراسان رضوی", "خراسان شمالی", "خوزستان", "زنجان", "سمنان",
        "سیستان و بلوچستان", "فارس", "قزوین", "قم", "کردستان",
        "کرمان", "کرمانشاه", "کهگیلویه و بویراحمد", "گلستان", "گیلان",
        "لرستان", "مازندران", "مرکزی", "هرمزگان", "همدان", "یزد"
    ];

    public void Configure(EntityTypeBuilder<Province> builder)
    {
        builder.ToTable("Provinces");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.HasData(ProvinceNames
            .Select((name, index) => new Province
        {
            Id = index + 1,
            Name = name,
            DateCreated = SeedDate
        }).ToArray());
    }
}