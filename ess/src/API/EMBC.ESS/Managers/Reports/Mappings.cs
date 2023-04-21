using AutoMapper;
using EMBC.ESS.Resources.Reports;
using EMBC.ESS.Utilities.Dynamics.Microsoft.Dynamics.CRM;

namespace EMBC.ESS.Managers.Reports
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Evacuee, Evacuee>()
                .ForMember(d => d.LastName, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.LastName))
                .ForMember(d => d.FirstName, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.FirstName))
                .ForMember(d => d.DateOfBirth, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.DateOfBirth))
                .ForMember(d => d.Gender, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.Gender))
                .ForMember(d => d.PreferredName, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.PreferredName))
                .ForMember(d => d.Initials, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.Initials))
                .ForMember(d => d.AddressLine1, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.AddressLine1))
                .ForMember(d => d.AddressLine2, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.AddressLine2))
                .ForMember(d => d.Community, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.Community))
                .ForMember(d => d.Province, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.Province))
                .ForMember(d => d.PostalCode, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.PostalCode))
                .ForMember(d => d.Country, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.Country))
                .ForMember(d => d.Phone, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.Phone))
                .ForMember(d => d.Email, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.Email))
                .ForMember(d => d.MailingAddressLine1, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.MailingAddressLine1))
                .ForMember(d => d.MailingAddressLine2, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.MailingAddressLine2))
                .ForMember(d => d.MailingCommunity, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.MailingCommunity))
                .ForMember(d => d.MailingProvince, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.MailingProvince))
                .ForMember(d => d.MailingPostal, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.MailingPostal))
                .ForMember(d => d.MailingCountry, opts => opts.ConvertUsing<PersonalInfoConverter, string>(s => s.MailingCountry))
                ;

            CreateMap<era_etransfertransaction, EMBC.ESS.Resources.Reports.EpaymentReport>()
                .ForMember(d => d.TransactionId, opts => opts.MapFrom(s => s.era_name))
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.firstname : null))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.lastname : null))
                .ForMember(d => d.PreferredName, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.era_preferredname : null))
                .ForMember(d => d.Initials, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.era_initial : null))
                .ForMember(d => d.Gender, opts => opts.ConvertUsing<GenderConverter, int?>(s => s.era_Payee_contact != null ? s.era_Payee_contact.gendercode : null))
                .ForMember(d => d.DateOfBirth, opts => opts.MapFrom(s => s.era_Payee_contact != null && s.era_Payee_contact.birthdate.HasValue
                    ? $"{s.era_Payee_contact.birthdate.Value.Month:D2}/{s.era_Payee_contact.birthdate.Value.Day:D2}/{s.era_Payee_contact.birthdate.Value.Year:D2}"
                    : null))
                .ForMember(d => d.PhoneNumber, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.address1_telephone1 : null))
                .ForMember(d => d.RegistrationDate, opts => opts.MapFrom(s => s.era_Payee_contact != null && s.era_Payee_contact.era_registrationdate.HasValue
                    ? $"{s.era_Payee_contact.era_registrationdate.Value.Month:D2}/{s.era_Payee_contact.era_registrationdate.Value.Day:D2}/{s.era_Payee_contact.era_registrationdate.Value.Year:D2}"
                    : null))
                .ForMember(d => d.Email, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.emailaddress1 : null))
                .ForMember(d => d.SiteSupplierNumber, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.era_sitesuppliernumber : null))
                .ForMember(d => d.SupplierNumber, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.era_suppliernumber : null))
                .ForMember(d => d.AddressLine1, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.address1_line1 : null))
                .ForMember(d => d.AddressLine2, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.address1_line2 : null))
                .ForMember(d => d.CityOrJurisdiction, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.address1_city : null))
                .ForMember(d => d.Postalcode, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.address1_postalcode : null))
                .ForMember(d => d.Country, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.address1_country : null))
                .ForMember(d => d.MailingAddressLine1, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.address2_line1 : null))
                .ForMember(d => d.MailingAddressLine2, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.address2_line2 : null))
                .ForMember(d => d.MailingJurisdiction, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.era_MailingCity : null))
                .ForMember(d => d.MailingCityOrJurisdiction, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.address2_city : null))
                .ForMember(d => d.MailingPostalcode, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.address2_postalcode : null))
                .ForMember(d => d.MailingProvinceState, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.era_MailingProvinceState : null))
                .ForMember(d => d.MailingCountry, opts => opts.MapFrom(s => s.era_Payee_contact != null ? s.era_Payee_contact.era_MailingCountry : null))
                //.ForMember(d => d.era_Evacuationfiles, opts => opts.MapFrom(s => s.era_Payee_contact.era_evacuationfile_Registrant))
                //.ForMember(d => d.LastName, opts => opts.MapFrom(s => s.era_Payee_contact.era_contact_era_evacueesupport))
                ;
        }

#pragma warning disable CA1034 // Nested types should not be visible

        public class PersonalInfoConverter : IValueConverter<string, string>
#pragma warning restore CA1034 // Nested types should not be visible
        {
            public string Convert(string sourceMember, ResolutionContext context)
            {
                if (ShouldShowPersonalInfo(context)) return sourceMember;
                else return string.Empty;
            }

            public static bool ShouldShowPersonalInfo(ResolutionContext ctx) =>
                ctx.Items.ContainsKey("IncludePersonalInfo") && bool.Parse(ctx.Items["IncludePersonalInfo"].ToString());
        }

        public class GenderConverter : IValueConverter<string, int?>, IValueConverter<int?, string>
        {
            public int? Convert(string sourceMember, ResolutionContext? context) => sourceMember switch
            {
                "Male" => 1,
                "Female" => 2,
                "X" => 3,
                _ => null
            };

            public string Convert(int? sourceMember, ResolutionContext? context) => sourceMember switch
            {
                1 => "Male",
                2 => "Female",
                3 => "X",
                _ => null
            };
        }
    }
}
