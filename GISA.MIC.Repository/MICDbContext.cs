using System;
using System.Collections.Generic;
using GISA.Domain.Model;
using GISA.Domain.Model.MIC;
using GISA.MIC.Repository.Configuration;
using Microsoft.EntityFrameworkCore;

namespace GISA.MIC.Repository
{
    public class MICDbContext : DbContext
    {
        private static readonly Bogus.DataSets.Name nameGenerator = new Bogus.DataSets.Name("pt_BR");
        private static readonly Bogus.DataSets.Address addressGenerator = new Bogus.DataSets.Address("pt_BR");

        public MICDbContext(DbContextOptions<MICDbContext> options)
            : base(options) { }

        public DbSet<Associate> Associates { get; set; }

        public DbSet<Consult> Consults { get; set; }

        public DbSet<Covenant> Covenants { get; set; }

        public DbSet<Specialty> Specialties { get; set; }

        public DbSet<MonthlyPayment> MonthlyPayments { get; set; }

        public DbSet<Provider> Providers { get; set; }

        public DbSet<Procedure> Procedures { get; set; }
        
        public DbSet<HealthInsurance> HealthInsurances { get; set; }
        
        //public DbSet<Address> Addresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Associate>().ToTable("tb_associate");
            modelBuilder.Entity<Consult>().ToTable("tb_consult");
            modelBuilder.Entity<Covenant>().ToTable("tb_covenant");
            modelBuilder.Entity<Specialty>().ToTable("tb_specialty");
            modelBuilder.Entity<MonthlyPayment>().ToTable("tb_monthly_payment");
            modelBuilder.Entity<Provider>().ToTable("tb_provider");
            modelBuilder.Entity<Procedure>().ToTable("tb_procedure");
            modelBuilder.Entity<HealthInsurance>().ToTable("tb_health_insurance");
            //modelBuilder.Entity<Address>().ToTable("tb_address");

            // Garante a persistência dos enumeradores como String
            modelBuilder.Entity<Associate>()
                .Property(e => e.Genre)
                .HasColumnName("genre")
                .HasConversion<string>();

            modelBuilder.Entity<Provider>()
                .Property(e => e.Genre)
                .HasColumnName("genre")
                .HasConversion<string>();

            modelBuilder.Entity<Associate>()
                .Property(e => e.PlanStatus)
                .HasColumnName("plan_status")
                .HasConversion<string>();

            modelBuilder.Entity<Associate>()
                .Property(e => e.PersonType)
                .HasColumnName("person_type")
                .HasConversion<string>();

            modelBuilder.Entity<Provider>()
                .Property(e => e.PersonType)
                .HasColumnName("person_type")
                .HasConversion<string>();

            modelBuilder.Entity<Associate>()
                .Property(e => e.PlanType)
                .HasColumnName("plan_type")
                .HasConversion<string>();

            modelBuilder.Entity<Consult>()
                .Property(e => e.Status)
                .HasColumnName("status")
                .HasConversion<string>();

            modelBuilder.Entity<Covenant>()
                .Property(e => e.MemberType)
                .HasColumnName("member_type")
                .HasConversion<string>();

            modelBuilder.Entity<HealthInsurance>()
                .Property(e => e.CategoryPlan)
                .HasColumnName("category_plan")
                .HasConversion<string>();

            modelBuilder.Entity<Provider>()
                .Property(e => e.Qualification)
                .HasColumnName("qualification")
                .HasConversion<string>();

            modelBuilder.Entity<Procedure>()
                .Property(e => e.Sector)
                .HasColumnName("sector")
                .HasConversion<string>();

            // Configura os objetos endereço/email como propriedades
            modelBuilder.ApplyConfiguration(new AssociateConfiguration());
            modelBuilder.ApplyConfiguration(new CovenantConfiguration());
            modelBuilder.ApplyConfiguration(new ProviderConfiguration());

            #region Consult

            modelBuilder.Entity<Consult>(e =>
            {
                e.HasIndex("ProviderId", "ConsultDate").IsUnique();
            });

            #endregion

            #region Associate e HealthInsurance

            modelBuilder.Entity<Associate>(e =>
            {
                e.HasIndex(e => e.CPF).IsUnique();
                e.HasIndex(e => e.CardNumber).IsUnique();
            });

            Random r = new Random();

            modelBuilder.Entity<HealthInsurance>(p =>
            {
                // Seed Planos
                p.HasData(
                    new HealthInsurance
                    (
                        id:1,
                        categoryPlan: ECategoryPlan.VIP,
                        ansNumber: r.Next(999999).ToString().PadLeft(6, '0'),
                        issuedAt: DateTime.UtcNow.AddDays(r.Next(100, 300) * -1),
                        issuedBy: 1,
                        commercialName: "VIP BLACK - TOP INTERNATIONAL",
                        dentalPlan:true
                    ),
                    new HealthInsurance
                    (
                        id: 2,
                        categoryPlan: ECategoryPlan.Enfermaria,
                        ansNumber: r.Next(999999).ToString().PadLeft(6, '0'),
                        issuedAt: DateTime.UtcNow.AddDays(r.Next(100, 300) * -1),
                        issuedBy: 1,
                        commercialName: "ENFERMARIA PADRÃO COM ODONTO",
                        dentalPlan: true
                    ),
                    new HealthInsurance
                    (
                        id: 3,
                        categoryPlan: ECategoryPlan.Apartamento,
                        ansNumber: r.Next(999999).ToString().PadLeft(6, '0'),
                        issuedAt: DateTime.UtcNow.AddDays(r.Next(100, 300) * -1),
                        issuedBy: 1,
                        commercialName: "APARTAMENTO PADRÃO",
                        dentalPlan: true
                    ),
                    new HealthInsurance
                    (
                        id: 4,
                        categoryPlan: ECategoryPlan.VIP,
                        ansNumber: r.Next(999999).ToString().PadLeft(6, '0'),
                        issuedAt: DateTime.UtcNow.AddDays(r.Next(100, 300) * -1),
                        issuedBy: 1,
                        commercialName: "VIP BLACK - TOP INTERNATIONAL",
                        dentalPlan: true
                    ),
                    new HealthInsurance
                    (
                        id: 5,
                        categoryPlan: ECategoryPlan.Apartamento,
                        ansNumber: r.Next(999999).ToString().PadLeft(6, '0'),
                        issuedAt: DateTime.UtcNow.AddDays(r.Next(100, 300) * -1),
                        issuedBy: 1,
                        commercialName: "APARTAMENTO PADRÃO - COM ODONTO",
                        dentalPlan: true
                    ),
                    new HealthInsurance
                    (
                        id: 6,
                        categoryPlan: ECategoryPlan.Enfermaria,
                        ansNumber: r.Next(999999).ToString().PadLeft(6, '0'),
                        issuedAt: DateTime.UtcNow.AddDays(r.Next(100, 300) * -1),
                        issuedBy: 1,
                        commercialName: "ENFERMARIA PADRÃO - BASICO 10",
                        dentalPlan: false
                    )
                );
            });

            var associates = new List<object>();

            long planId = 0;

            for (long personId = 1; personId < 11; personId++)
            {
                var randomGender = r.Next(2);

                planId = ++planId > 5 ? planId - 5 : planId;

                associates.Add(new
                {
                    PlanId = planId,
                    Id = personId * 1000,
                    CPF = $"{r.Next(999999999).ToString().PadLeft(11, '0')}",
                    IssuedAt = DateTime.UtcNow,
                    IssuedBy = (long)1,
                    JoiningDate = DateTime.UtcNow.AddDays(-90),
                    BirthDate = DateTime.UtcNow.AddYears(-35),
                    Genre = randomGender == 0 ? EGenderPerson.Masculino : EGenderPerson.Feminino,
                    Name = $"{nameGenerator.FirstName(randomGender == 0 ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female)} {nameGenerator.LastName(randomGender == 0 ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female)}",
                    CardNumber = $"{r.Next(99999999).ToString().PadLeft(8, '0')}{r.Next(99999999).ToString().PadLeft(8, '0')}",
                    RG = $"{r.Next(999999999).ToString().PadLeft(9, '0')}",
                    PlanStatus = EPlanStatus.Ativo,
                    PersonType = EPersonType.Associado,
                    PlanType = randomGender == 0 ? EContractType.Individual : EContractType.Empresarial
                });
            }

            modelBuilder.Entity<Associate>(p =>
            {
                // Seed Associate
                p.HasData(associates.ToArray());
            });

            #endregion

            #region Procedure

            modelBuilder.Entity<Procedure>(p =>
            {
                // Seed Procedures
                p.HasData(
                        new Procedure(id: 1, tussCode: "00000039", name: "ANATOMO PATOLOGICO (BIOPSIA DE ORGAOS - PATHOS)", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 2, tussCode: "00000065", name: "USG ABDOME TOTAL E PROSTATA", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 3, tussCode: "00000065", name: "USG ABDOME TOTAL E PELVE", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 4, tussCode: "40808033", name: "MAMOGRAFIA BILATERAL", sector: EProcedureSector.Mamografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 5, tussCode: "40808041", name: "MAMOGRAFIA DIGITALIZADA", sector: EProcedureSector.Mamografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 6, tussCode: "40808092", name: "CORE BIOPSIA GUIADA POR UltraSom", sector: EProcedureSector.Punção, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 7, tussCode: "40808092", name: "BIOPSIA PERCUTANEA DE FRAGMENTO MAMARIO", sector: EProcedureSector.Punção, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 8, tussCode: "40808130", name: "DENSITOMETRIA OSSEA COLUNA LOMBAR E FEMUR", sector: EProcedureSector.Densitometria, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 9, tussCode: "40808149", name: "DENSITOMETRIA OSSEA CORPO INTEIRO", sector: EProcedureSector.Densitometria, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 10, tussCode: "40809099", name: "PUNCAO ASPIRATIVA ORIENTADA POR USG ( PAAF )", sector: EProcedureSector.Punção, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 11, tussCode: "40809099", name: "PUNCAO OU BIOPSIA MAMARIA PERCUTANEA POR AGULHA FINA", sector: EProcedureSector.Punção, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 12, tussCode: "40901114", name: "USG MAMAS", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 13, tussCode: "40901122", name: "USG ABDOME TOTAL", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 14, tussCode: "40901122", name: "USG ABDOME TOTAL COM PROVA DE BOYDEN", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 15, tussCode: "40901130", name: "USG ABDOME SUPERIOR", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 16, tussCode: "40901130", name: "USG HIPOCONDRIO DIREITO", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 17, tussCode: "40901181", name: "USG PELVICA", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 18, tussCode: "40901203", name: "USG BOLSA ESCROTAL", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 19, tussCode: "40901203", name: "USG ORBITA", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 20, tussCode: "40901203", name: "USG ORGAOS SUPERFICIAIS", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 21, tussCode: "40901203", name: "USG TIREOIDE", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 22, tussCode: "40901211", name: "USG ANTEBRACO", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 23, tussCode: "40901211", name: "USG AXILAS", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 24, tussCode: "40901211", name: "USG BRACO", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 25, tussCode: "40901211", name: "USG CERVICAL", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 26, tussCode: "40901211", name: "USG COXA", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 27, tussCode: "40901211", name: "USG ORGAOS E ESTRUTURAS SUPERFICIAIS", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 28, tussCode: "40901211", name: "USG MUSCULO / TENDAO", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 29, tussCode: "40901211", name: "USG PAREDE ABDOMINAL", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 30, tussCode: "40901211", name: "USG PERNA", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 31, tussCode: "40901211", name: "USG REGIAO INGUINAL UNILATERAL", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 32, tussCode: "40901211", name: "USG TENDÃO", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 33, tussCode: "40901220", name: "USG ARTICULACOES", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 34, tussCode: "40901220", name: "USG COTOVELO", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 35, tussCode: "40901220", name: "USG JOELHO", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 36, tussCode: "40901220", name: "USG MAO", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 37, tussCode: "40901220", name: "USG OMBRO", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 38, tussCode: "40901220", name: "USG PE", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 39, tussCode: "40901220", name: "USG PUNHO", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 40, tussCode: "40901220", name: "USG QUADRIL INFANTIL UNILATERAL", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 41, tussCode: "40901220", name: "USG QUADRIL UNILATERAL", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 42, tussCode: "40901220", name: "USG TORNOZELO", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 43, tussCode: "40901300", name: "USG TRANSVAGINAL", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 44, tussCode: "40901319", name: "USG TRANSVAGINAL PARA CONTROLE DE OVULACAO", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 45, tussCode: "40901360", name: "DOPPLER COLOR DE CAROTIDAS OU VERTEBRAIS", sector: EProcedureSector.Doppler, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 46, tussCode: "40901378", name: "DOPPLER COLOR DE SUBCLAVIAS E JUGULARES", sector: EProcedureSector.Doppler, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 47, tussCode: "40901386", name: "DOPPLER COLOR DE ORGAOS OU ESTRUTURAS", sector: EProcedureSector.Doppler, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 48, tussCode: "40901386", name: "DOPPLER COLOR TIREOIDE", sector: EProcedureSector.Doppler, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 49, tussCode: "40901408", name: "DOPPLER COLOR DE AORTA", sector: EProcedureSector.Doppler, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 50, tussCode: "40901408", name: "DOPPLER COLOR DE ILIACAS", sector: EProcedureSector.Doppler, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 51, tussCode: "40901432", name: "DOPPLER COLOR DE VEIA CAVA", sector: EProcedureSector.Doppler, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 52, tussCode: "40901459", name: "DOPPLER COLOR ARTERIAL DE MEMBRO SUPERIOR", sector: EProcedureSector.Doppler, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 53, tussCode: "40901467", name: "DOPPLER COLOR VENOSO DE MEMBRO SUPERIOR", sector: EProcedureSector.Doppler, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 54, tussCode: "40901475", name: "DOPPLER COLOR ARTERIAL DE MEMBRO INFERIOR", sector: EProcedureSector.Doppler, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 55, tussCode: "40901483", name: "DOPPLER COLOR VENOSO DE MEMBRO INFERIOR", sector: EProcedureSector.Doppler, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 56, tussCode: "40901750", name: "USG PROSTATA VIA ABDOMINAL", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 57, tussCode: "40901769", name: "USG APARELHO URINARIO", sector: EProcedureSector.UltraSom, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 58, tussCode: "41001010", name: "TC CRANIO S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 59, tussCode: "41001010", name: "TC ORBITAS S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 60, tussCode: "41001010", name: "TC SELA TURCICA S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 61, tussCode: "41001028", name: "TC MASTOIDES S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 62, tussCode: "41001028", name: "TC OSSOS TEMPORAIS S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 63, tussCode: "41001028", name: "TC OUVIDO S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 64, tussCode: "41001036", name: "TC FACE S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 65, tussCode: "41001036", name: "TC SEIOS DA FACE S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 66, tussCode: "41001044", name: "TC MANDIBULA OU ATM S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 67, tussCode: "41001060", name: "TC FARINGE S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 68, tussCode: "41001060", name: "TC LARINGE S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 69, tussCode: "41001060", name: "TC PESCOCO S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 70, tussCode: "41001060", name: "TC TIREOIDE S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 71, tussCode: "41001079", name: "TC TORAX S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 72, tussCode: "41001095", name: "TC ABDOME TOTAL (SUPERIOR + PELVE + RETROP) S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 73, tussCode: "41001109", name: "TC ABDOME SUPERIOR S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 74, tussCode: "41001117", name: "TC BACIA S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 75, tussCode: "41001117", name: "TC PELVE S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 76, tussCode: "41001125", name: "TC COCCIX S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 77, tussCode: "41001125", name: "TC COLUNA CERVICAL S/CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 78, tussCode: "41001125", name: "TC COLUNA DORSAL S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 79, tussCode: "41001125", name: "TC COLUNA LOMBAR S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 80, tussCode: "41001125", name: "TC COLUNA SACRAL S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 81, tussCode: "41001133", name: "TC COLUNA - SEGMENTO ADICIONAL", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 82, tussCode: "41001141", name: "TC ARTICULACAO S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 83, tussCode: "41001141", name: "TC COTOVELO S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 84, tussCode: "41001141", name: "TC COXO FEMURAL S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 85, tussCode: "41001141", name: "TC ESTERNO CLAVICULAR S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 86, tussCode: "41001141", name: "TC JOELHO S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 87, tussCode: "41001141", name: "TC OMBRO S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 88, tussCode: "41001141", name: "TC TORNOZELO S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 89, tussCode: "41001141", name: "TC PUNHO S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 90, tussCode: "41001141", name: "TC TAGT (TC QUADRIL + TC JOELHO)", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 91, tussCode: "41001141", name: "TC QUADRIL UNILATERAL S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 92, tussCode: "41001141", name: "TC SACRO ILIACA S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 93, tussCode: "41001150", name: "TC ANTEBRACO S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 94, tussCode: "41001150", name: "TC BRACO S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 95, tussCode: "41001150", name: "TC COXA S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 96, tussCode: "41001150", name: "TC MAO S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 97, tussCode: "41001150", name: "TC PE S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 98, tussCode: "41001150", name: "TC PERNA S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 99, tussCode: "41001150", name: "TC SEGMENTO APENDICULAR S/ CONTRASTE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 100, tussCode: "41001192", name: "ESCANOMETRIA DE MMIIS BILATERAL POR TOMOGRAFIA", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 101, tussCode: "41001206", name: "RECONSTRUCAO TRIDIMENCIONAL - TC - ACRESCENTAR O EXAME BASE", sector: EProcedureSector.Tomografia, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 102, tussCode: "41101014", name: "RM CRANIO S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 103, tussCode: "41101022", name: "RM HIPOFISE S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 104, tussCode: "41101022", name: "RM SELA HIPOFISE S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 105, tussCode: "41101022", name: "RM SELA TURCICA S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 106, tussCode: "41101030", name: "RM BASE DE CRANIO S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 107, tussCode: "41101073", name: "RM ORBITA S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 108, tussCode: "41101081", name: "RM MASTOIDES S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 109, tussCode: "41101090", name: "RM FACE S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 110, tussCode: "41101103", name: "RM ARTICULACAO TEMPORO MANDIBULAR (BILATERAL) S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 111, tussCode: "41101111", name: "RM FARINGE, LARINGE, TRAQUEIA S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 112, tussCode: "41101111", name: "RM PESCOCO (CAROTIDAS) S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 113, tussCode: "41101120", name: "RM ARCOS COSTAIS S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 114, tussCode: "41101120", name: "RM PAREDE TORACICA S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 115, tussCode: "41101120", name: "RM TORAX S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 116, tussCode: "41101170", name: "RM ABDOMEN SUPERIOR S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 117, tussCode: "41101170", name: "RM PAREDE ABDOMINAL S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 118, tussCode: "41101189", name: "RM PELVE S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 119, tussCode: "41101189", name: "RM REGIAO GLUTEA S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 120, tussCode: "41101189", name: "RM SINFISE PUBICA S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 121, tussCode: "41101200", name: "RM PENIS s/CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 122, tussCode: "41101219", name: "RM BOLSA ESCROTAL", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 123, tussCode: "41101227", name: "RM COLUNA CERVICAL S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 124, tussCode: "41101227", name: "RM COLUNA LOMBO-SACRA S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 125, tussCode: "41101227", name: "RM COLUNA TORACICA S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 126, tussCode: "41101227", name: "RM REGIAO CERVICAL S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 127, tussCode: "41101243", name: "RM PLEXO BRAQUIAL (UNILATERAL) S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 128, tussCode: "41101251", name: "RM ANTEBRACO S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 129, tussCode: "41101251", name: "RM BRACO S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 130, tussCode: "41101251", name: "RM SEGMENTO APENDICULAR (UNILATERAl) S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 131, tussCode: "41101260", name: "RM MAO S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 132, tussCode: "41101278", name: "RM BACIA S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 133, tussCode: "41101278", name: "RM SACRO COCCIX S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 134, tussCode: "41101278", name: "RM SACRO ILIACAS BILATERAL S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 135, tussCode: "41101286", name: "RM COXA S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 136, tussCode: "41101294", name: "RM PERNA S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 137, tussCode: "41101308", name: "RM PE S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 138, tussCode: "41101316", name: "RM ARTICULACAO", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 139, tussCode: "41101316", name: "RM CLAVICULA S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 140, tussCode: "41101316", name: "RM COTOVELO (UNILATERAL) S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 141, tussCode: "41101316", name: "RM COXO-FEMURAL UNILATERAL S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 142, tussCode: "41101316", name: "RM JOELHO (UNILATERAL) S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 143, tussCode: "41101316", name: "RM OMBRO (UNILATERAL) S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 144, tussCode: "41101316", name: "RM PUNHO S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 145, tussCode: "41101316", name: "RM QUADRIL UNILATERAL S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 146, tussCode: "41101316", name: "RM TORNOZELO (CALCANEO) (UNILATERAL) S/ CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 147, tussCode: "41101324", name: "ANGIO-RESSONANCIA ENCEFALO S/C", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 148, tussCode: "41101324", name: "ANGIO-RESSONANCIA CRANIO S/C", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 149, tussCode: "41101359", name: "HIDRO-RM (COLANGIO-RM OU URO-RM, OU MIELO-RM) C/CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 150, tussCode: "41101359", name: "HIDRO-RM (COLANGIO-RM OU URO-RM, OU MIELO-RM) S/CONTRASTE", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 151, tussCode: "41101537", name: "ANGIO-RESSONANCIA ARTERIAL CRANIO S/C", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 152, tussCode: "41101545", name: "ANGIO-RESSONANCIA VENOSA CRANIO S/C", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 153, tussCode: "41101618", name: "ANGIO-RESSONANCIA PESCOÇO ARTERIAL S/C", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow),
                        new Procedure(id: 154, tussCode: "41101626", name: "ANGIO-RESSONANCIA PESCOÇO VENOSA S/C", sector: EProcedureSector.Ressonância, issuedBy: 1, issuedAt: DateTime.UtcNow)
                    );
            });

            #endregion

            #region Specialty

            List<Specialty> specialties = new List<Specialty>
            {
                new Specialty(1, "Acupuntura", 1, DateTime.UtcNow),
                new Specialty(2, "Alergia e Imunologia", 1, DateTime.UtcNow),
                new Specialty(3, "Anestesiologia", 1, DateTime.UtcNow),
                new Specialty(4, "Angiologia", 1, DateTime.UtcNow),
                new Specialty(5, "Cancerologia", 1, DateTime.UtcNow),
                new Specialty(6, "Cardiologia", 1, DateTime.UtcNow),
                new Specialty(7, "Cirurgia Cardiovascular", 1, DateTime.UtcNow),
                new Specialty(8, "Cirurgia da Mão", 1, DateTime.UtcNow),
                new Specialty(9, "Cirurgia de Cabeça e Pescoço", 1, DateTime.UtcNow),
                new Specialty(10, "Cirurgia do Aparelho Digestivo", 1, DateTime.UtcNow),
                new Specialty(11, "Cirurgia Geral", 1, DateTime.UtcNow),
                new Specialty(12, "Cirurgia Pediátrica", 1, DateTime.UtcNow),
                new Specialty(13, "Cirurgia Plástica", 1, DateTime.UtcNow),
                new Specialty(14, "Cirurgia Torácica", 1, DateTime.UtcNow),
                new Specialty(15, "Cirurgia Vascular", 1, DateTime.UtcNow),
                new Specialty(16, "Clínica Médica", 1, DateTime.UtcNow),
                new Specialty(17, "Coloproctologia", 1, DateTime.UtcNow),
                new Specialty(18, "Dermatologia", 1, DateTime.UtcNow),
                new Specialty(19, "Endocrinologia e Metabologia", 1, DateTime.UtcNow),
                new Specialty(20, "Endoscopia", 1, DateTime.UtcNow),
                new Specialty(21, "Gastroenterologia", 1, DateTime.UtcNow),
                new Specialty(22, "Genética Médica", 1, DateTime.UtcNow),
                new Specialty(23, "Geriatria", 1, DateTime.UtcNow),
                new Specialty(24, "Ginecologia e Obstetrícia", 1, DateTime.UtcNow),
                new Specialty(25, "Hematologia e Hemoterapia", 1, DateTime.UtcNow),
                new Specialty(26, "Homeopatia", 1, DateTime.UtcNow),
                new Specialty(27, "Infectologia", 1, DateTime.UtcNow),
                new Specialty(28, "Mastologia", 1, DateTime.UtcNow),
                new Specialty(29, "Medicina de Família e Comunidade", 1, DateTime.UtcNow),
                new Specialty(30, "Medicina do Trabalho", 1, DateTime.UtcNow),
                new Specialty(31, "Medicina de Tráfego", 1, DateTime.UtcNow),
                new Specialty(32, "Medicina Esportiva", 1, DateTime.UtcNow),
                new Specialty(33, "Medicina Física e Reabilitação", 1, DateTime.UtcNow),
                new Specialty(34, "Medicina Intensiva", 1, DateTime.UtcNow),
                new Specialty(35, "Medicina Legal e Perícia Médica", 1, DateTime.UtcNow),
                new Specialty(36, "Medicina Nuclear", 1, DateTime.UtcNow),
                new Specialty(37, "Medicina Preventiva e Social", 1, DateTime.UtcNow),
                new Specialty(38, "Nefrologia", 1, DateTime.UtcNow),
                new Specialty(39, "Neurocirurgia", 1, DateTime.UtcNow),
                new Specialty(40, "Neurologia", 1, DateTime.UtcNow),
                new Specialty(41, "Nutrologia", 1, DateTime.UtcNow),
                new Specialty(42, "Oftalmologia", 1, DateTime.UtcNow),
                new Specialty(43, "Ortopedia e Traumatologia", 1, DateTime.UtcNow),
                new Specialty(44, "Otorrinolaringologia", 1, DateTime.UtcNow),
                new Specialty(45, "Patologia", 1, DateTime.UtcNow),
                new Specialty(46, "Patologia Clínica/Medicina Laboratorial", 1, DateTime.UtcNow),
                new Specialty(47, "Pediatria", 1, DateTime.UtcNow),
                new Specialty(48, "Pneumologia", 1, DateTime.UtcNow),
                new Specialty(49, "Psiquiatria", 1, DateTime.UtcNow),
                new Specialty(50, "Radiologia e Diagnóstico por Imagem", 1, DateTime.UtcNow),
                new Specialty(51, "Radioterapia", 1, DateTime.UtcNow),
                new Specialty(52, "Reumatologia", 1, DateTime.UtcNow),
                new Specialty(53, "Urologia", 1, DateTime.UtcNow)
            };

            modelBuilder.Entity<Specialty>(e =>
            {
                // Seed SpecialtyCollection
                e.HasData(specialties.ToArray());
            });

            #endregion

            #region Provider

            List<object> providers = new List<object>();

            for (long personId = 1; personId < 11; personId++)
            {
                var randomGender = r.Next(2);

                Enum.TryParse(r.Next(6).ToString(), out EProviderQualification providerQualification);

                providers.Add(new
                {
                    Qualification = providerQualification,
                    Id = personId * 10000,
                    CPF = $"{r.Next(999999999).ToString().PadLeft(11, '0')}",
                    IssuedAt = DateTime.UtcNow,
                    IssuedBy = (long)1,
                    JoiningDate = DateTime.UtcNow.AddDays(-90),
                    BirthDate = DateTime.UtcNow.AddYears(-35),
                    Genre = randomGender == 0 ? EGenderPerson.Masculino : EGenderPerson.Feminino,
                    Name = $"{nameGenerator.FirstName(randomGender == 0 ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female)} {nameGenerator.LastName(randomGender == 0 ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female)}",
                    CardNumber = $"{r.Next(99999999).ToString().PadLeft(8, '0')}{r.Next(99999999).ToString().PadLeft(8, '0')}",
                    RG = $"{r.Next(999999999).ToString().PadLeft(9, '0')}",
                    PersonType = EPersonType.Colaborador
                });
            }

            modelBuilder.Entity<Provider>(p =>
            {
                // Seed Providers
                p.HasData(providers.ToArray());
            });

            #endregion

            #region Specialty x Provider

            List<object> specialtyProvider = new List<object>();

            foreach (var specialty in specialties)
            {
                specialtyProvider.Add(new
                {
                    specialty_id = specialty.Id,
                    provider_id = (long)r.Next(1, 11) * 10000
                });
            }

            // Entidade de relacionamento Specialty x Provider
            modelBuilder.Entity<Provider>()
                        .HasMany(p => p.SpecialtyCollection)
                        .WithMany(p => p.Providers)
                        .UsingEntity<Dictionary<string, object>>(
                            "tb_specialty_provider",
                            j => j
                                .HasOne<Specialty>()
                                .WithMany()
                                .HasForeignKey("specialty_id"),
                            j => j
                                .HasOne<Provider>()
                                .WithMany()
                                .HasForeignKey("provider_id")).HasData(specialtyProvider.ToArray());

            #endregion
        }
    }
}