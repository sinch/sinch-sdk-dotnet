using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Sinch.Conversation.Messages.Message
{
    /// <summary>
    ///     Message containing contact information.
    /// </summary>
    public sealed class ContactInfoMessage : IOmniMessageOverride
    {
        /// <summary>
        ///     Gets or Sets Name
        /// </summary>
        [JsonPropertyName("name")]

        public required NameInfo Name { get; set; }



        /// <summary>
        ///     Phone numbers of the contact
        /// </summary>
        [JsonPropertyName("phone_numbers")]

        public required List<PhoneNumberInfo> PhoneNumbers { get; set; }



        /// <summary>
        ///     Physical addresses of the contact
        /// </summary>
        [JsonPropertyName("addresses")]
        public List<AddressInfo>? Addresses { get; set; }


        /// <summary>
        ///     Email addresses of the contact
        /// </summary>
        [JsonPropertyName("email_addresses")]
        public List<EmailInfo>? EmailAddresses { get; set; }


        /// <summary>
        ///     Gets or Sets Organization
        /// </summary>
        [JsonPropertyName("organization")]
        public OrganizationInfo? Organization { get; set; }


        /// <summary>
        ///     URLs/websites associated with the contact
        /// </summary>
        [JsonPropertyName("urls")]
        public List<UrlInfo>? Urls { get; set; }


        /// <summary>
        ///     Date of birth in YYYY-MM-DD format.
        /// </summary>
        [JsonPropertyName("birthday")]
        public DateTime? Birthday { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(ContactInfoMessage)} {{\n");
            sb.Append($"  {nameof(Name)}: ").Append(Name).Append('\n');
            sb.Append($"  {nameof(PhoneNumbers)}: ").Append(PhoneNumbers).Append('\n');
            sb.Append($"  {nameof(Addresses)}: ").Append(Addresses).Append('\n');
            sb.Append($"  {nameof(EmailAddresses)}: ").Append(EmailAddresses).Append('\n');
            sb.Append($"  {nameof(Organization)}: ").Append(Organization).Append('\n');
            sb.Append($"  {nameof(Urls)}: ").Append(Urls).Append('\n');
            sb.Append($"  {nameof(Birthday)}: ").Append(Birthday).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Name information of the contact.
    /// </summary>
    public sealed class NameInfo
    {
        /// <summary>
        ///     Full name of the contact
        /// </summary>
        [JsonPropertyName("full_name")]

        public required string FullName { get; set; }



        /// <summary>
        ///     First name.
        /// </summary>
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }


        /// <summary>
        ///     Last name.
        /// </summary>
        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }


        /// <summary>
        ///     Middle name.
        /// </summary>
        [JsonPropertyName("middle_name")]
        public string? MiddleName { get; set; }


        /// <summary>
        ///     Prefix before the name. e.g. Mr, Mrs, Dr etc.
        /// </summary>
        [JsonPropertyName("prefix")]
        public string? Prefix { get; set; }


        /// <summary>
        ///     Suffix after the name.
        /// </summary>
        [JsonPropertyName("suffix")]
        public string? Suffix { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(NameInfo)} {{\n");
            sb.Append($"  {nameof(FullName)}: ").Append(FullName).Append('\n');
            sb.Append($"  {nameof(FirstName)}: ").Append(FirstName).Append('\n');
            sb.Append($"  {nameof(LastName)}: ").Append(LastName).Append('\n');
            sb.Append($"  {nameof(MiddleName)}: ").Append(MiddleName).Append('\n');
            sb.Append($"  {nameof(Prefix)}: ").Append(Prefix).Append('\n');
            sb.Append($"  {nameof(Suffix)}: ").Append(Suffix).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Phone numbers of the contact.
    /// </summary>
    public sealed class PhoneNumberInfo
    {
        /// <summary>
        ///     Phone number with country code included.
        /// </summary>
        [JsonPropertyName("phone_number")]

        public required string PhoneNumber { get; set; }



        /// <summary>
        ///     Phone number type, e.g. WORK or HOME.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(PhoneNumberInfo)} {{\n");
            sb.Append($"  {nameof(PhoneNumber)}: ").Append(PhoneNumber).Append('\n');
            sb.Append($"  {nameof(Type)}: ").Append(Type).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Physical addresses of the contact.
    /// </summary>
    public sealed class AddressInfo
    {
        /// <summary>
        ///     City Name
        /// </summary>
        [JsonPropertyName("city")]
        public string? City { get; set; }


        /// <summary>
        ///     Country Name
        /// </summary>
        [JsonPropertyName("country")]
        public string? Country { get; set; }


        /// <summary>
        ///     Name of a state or region of a country.
        /// </summary>
        [JsonPropertyName("state")]
        public string? State { get; set; }


        /// <summary>
        ///     Zip/postal code
        /// </summary>
        [JsonPropertyName("zip")]
        public string? Zip { get; set; }


        /// <summary>
        ///     Address type, e.g. WORK or HOME
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }


        /// <summary>
        ///     Two letter country code.
        /// </summary>
        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(AddressInfo)} {{\n");
            sb.Append($"  {nameof(City)}: ").Append(City).Append('\n');
            sb.Append($"  {nameof(Country)}: ").Append(Country).Append('\n');
            sb.Append($"  {nameof(State)}: ").Append(State).Append('\n');
            sb.Append($"  {nameof(Zip)}: ").Append(Zip).Append('\n');
            sb.Append($"  {nameof(Type)}: ").Append(Type).Append('\n');
            sb.Append($"  {nameof(CountryCode)}: ").Append(CountryCode).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Email addresses of the contact.
    /// </summary>
    public sealed class EmailInfo
    {
        /// <summary>
        ///     Email address.
        /// </summary>
        [JsonPropertyName("email_address")]

        public required string EmailAddress { get; set; }



        /// <summary>
        ///     Email address type. e.g. WORK or HOME.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(EmailInfo)} {{\n");
            sb.Append($"  {nameof(EmailAddress)}: ").Append(EmailAddress).Append('\n');
            sb.Append($"  {nameof(Type)}: ").Append(Type).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     Organization information of the contact.
    /// </summary>
    public sealed class OrganizationInfo
    {
        /// <summary>
        ///     Company name
        /// </summary>
        [JsonPropertyName("company")]
        public string? Company { get; set; }


        /// <summary>
        ///     Department at the company
        /// </summary>
        [JsonPropertyName("department")]
        public string? Department { get; set; }


        /// <summary>
        ///     Corporate title, e.g. Software engineer
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(OrganizationInfo)} {{\n");
            sb.Append($"  {nameof(Company)}: ").Append(Company).Append('\n');
            sb.Append($"  {nameof(Department)}: ").Append(Department).Append('\n');
            sb.Append($"  {nameof(Title)}: ").Append(Title).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }

    /// <summary>
    ///     A URL/website
    /// </summary>
    public sealed class UrlInfo
    {
        /// <summary>
        ///     The URL to be referenced
        /// </summary>
        [JsonPropertyName("url")]

        public required string Url { get; set; }



        /// <summary>
        ///     Optional. URL type, e.g. Org or Social
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }


        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"class {nameof(UrlInfo)} {{\n");
            sb.Append($"  {nameof(Url)}: ").Append(Url).Append('\n');
            sb.Append($"  {nameof(Type)}: ").Append(Type).Append('\n');
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
