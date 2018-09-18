using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xunit;
using LAPhil.User.Tests.Models;

namespace LAPhil.User.Tests
{
    public class Deserialization
    {
        [Fact]
        public void BlocksOfficeCollectionSingleTest()
        {
            var value = @"{
                ""string"": {},
                ""stringlist"": {},
                ""datetime"": {},
                ""isboolean"": {},
                ""number"": {}
            }";

            var data = $"{{\"nullablearray\": {value}, \"nullablelist\": {value}}}";
            var obj = JsonConvert.DeserializeObject<ListModel>(data);

            Assert.True(obj.NullableArray != null);
            Assert.True(obj.NullableList != null);

            Assert.True(obj.NullableArray.Length == 1);
            Assert.True(obj.NullableList.Count == 1);
        }

        [Fact]
        public void BlocksOfficeCollectionMultiTest()
        {
            var value = @"{
                ""string"": {},
                ""stringlist"": {},
                ""datetime"": {},
                ""isboolean"": {},
                ""number"": {}
            }";

            var data = $"{{ \"nullablearray\": [{value}], \"nullablelist\": [{value}] }}";
            var obj = JsonConvert.DeserializeObject<ListModel>(data);

            Assert.True(obj.NullableArray != null);
            Assert.True(obj.NullableList != null);

            Assert.True(obj.NullableArray.Length == 1);
            Assert.True(obj.NullableList.Count == 1);
        }

        [Fact]
        public void BlocksOfficeNullableNullTest()
        {
            var data = @"{
                ""string"": {},
                ""stringlist"": {},
                ""datetime"": {},
                ""isboolean"": {},
                ""number"": {}
            }";
            
            var obj = JsonConvert.DeserializeObject<NullableModel>(data);

            Assert.True(obj.String == default(string));
            Assert.True(obj.StringList == default(List<string>));
            Assert.True(obj.DateTime == default(DateTimeOffset));
            Assert.True(obj.IsBoolean == default(bool));
            Assert.True(obj.Number == default(int));
        }

        [Fact]
        public void BlocksOfficeNullableValueTest()
        {
            var data = @"{
                ""string"": ""lorem"",
                ""stringlist"": [""ipsum"", ""dolor""],
                ""datetime"": ""2018-01-03T06:10:00-0800"",
                ""isboolean"": true,
                ""number"": 15099
            }";

            var obj = JsonConvert.DeserializeObject<NullableModel>(data);

            Assert.True(obj.String == "lorem");
            Assert.True(obj.StringList.Count == 2);
            Assert.True(obj.StringList[0] == "ipsum");
            Assert.True(obj.StringList[1] == "dolor");
            Assert.True(obj.DateTime == new DateTimeOffset(
                year: 2018, month: 1, day: 3,
                hour: 6, minute: 10, second: 0,
                offset: new TimeSpan(hours: -8, minutes: 0, seconds: 0)
            ));
            Assert.True(obj.IsBoolean == true);
            Assert.True(obj.Number == 15099);
        }

        [Fact]
        public void DeserializeAddress()
        {
            var data = ResourceManager.FileString("address.json");
            var obj = JsonConvert.DeserializeObject<Address>(data);

            Assert.True(obj.AddressId == "3996655");
            Assert.True(obj.CustomerId == "1459479");
            Assert.True(obj.AddressType == "3");
            Assert.True(obj.AddressTypeDescription == "Home Address");
            Assert.True(obj.Street1 == "97 Icknield Street");
            Assert.True(obj.Street2 == null);
            Assert.True(obj.City == "Birmingham");
            Assert.True(obj.State == "West Midlands");
            Assert.True(obj.PostalCode == "B18 6RU");
            Assert.True(obj.PostalCodeFormat == "B18 6RU");
            Assert.True(obj.CountryId == "209");
            Assert.True(obj.CountryLong == "United Kingdom");
            Assert.True(obj.CountryShort == "UK ");
            Assert.True(obj.Start == new DateTimeOffset(
                year: 2017, month: 9, day: 19, 
                hour: 3, minute: 5, second: 5, 
                offset: new TimeSpan(hours: -7, minutes: 0, seconds: 0)
            ));
            Assert.True(obj.End == new DateTimeOffset(
                year: 2017, month: 9, day: 19,
                hour: 3, minute: 5, second: 5,
                offset: new TimeSpan(hours: -7, minutes: 0, seconds: 0)
            ));
            Assert.True(obj.Months == "YYYYYYYYYYYY");
            Assert.True(obj.PrimaryInd == "N");
            Assert.True(obj.Inactive == "Y");
            Assert.True(obj.GeoArea == "4");
        }

        [Fact]
        public void DeserializeConstituency()
        {
            var data = ResourceManager.FileString("constituency.json");
            var obj = JsonConvert.DeserializeObject<Constituency>(data);

            Assert.True(obj.Value == "WD Current CYO Buyer");
            Assert.True(obj.CreatedBy == "webapi  ");

            Assert.True(obj.CreatedAt == new DateTimeOffset(
                year: 2017, month: 12, day: 3,
                hour: 11, minute: 22, second: 27, millisecond: 250,
                offset: new TimeSpan(hours: -8, minutes: 0, seconds: 0)
            ));

            Assert.True(obj.N1N2Ind == "1");
            Assert.True(obj.Id == "41");
            Assert.True(obj.ShortDescription == "WCC");
        }

        [Fact]
        public void DeserializeConstituentHeader()
        {
            var data = ResourceManager.FileString("constituent_header.json");
            var obj = JsonConvert.DeserializeObject<ConstituentHeader>(data);

            Assert.True(obj.CustomerId == "1459479");
            Assert.True(obj.FullName1 == "Krzysztof Kurasz                                       ");
            Assert.True(obj.AllConst == "WCC");
            Assert.True(obj.MemberLevel == "A16");
            Assert.True(obj.CurrentStatus == "Active");

            Assert.True(obj.MemberExpiration == new DateTimeOffset(
                year: 2018, month: 11, day: 29,
                hour: 23, minute: 59, second: 59, millisecond: 997,
                offset: new TimeSpan(hours: -8, minutes: 0, seconds: 0)
            ));

            Assert.True(obj.CreatedAt == new DateTimeOffset(
                year: 2016, month: 10, day: 17,
                hour: 6, minute: 7, second: 9, millisecond: 63,
                offset: new TimeSpan(hours: -7, minutes: 0, seconds: 0)
            ));
            Assert.True(obj.Donation == "2283.0000");
            Assert.True(obj.UserId == "webapi  ");
            Assert.True(obj.OnAccount == "30.0000");
            Assert.True(obj.CustomerType == "1");
            Assert.True(obj.CustomerGroup == "1");
            Assert.True(obj.WorkerInd == "N");
        }

        [Fact]
        public void DeserializeContribution()
        {
            var data = ResourceManager.FileString("contribution.json");
            var obj = JsonConvert.DeserializeObject<Contribution>(data);

            Assert.True(obj.CustomerId == "1459479");
            Assert.True(obj.ContributionDate == new DateTimeOffset(
                year: 2017, month: 11, day: 30,
                hour: 4, minute: 44, second: 40, millisecond: 267,
                offset: new TimeSpan(hours: -8, minutes: 0, seconds: 0)
            ));

            Assert.True(obj.RecordAmount == 8.0);
            Assert.True(obj.ContributionAmount == 8.0);
            Assert.True(obj.Type == "Gift");
            Assert.True(obj.Campaign == "2018 Individual");
            Assert.True(obj.Appeal == "LAPA Website");
            Assert.True(obj.MediaType == "Website");
            Assert.True(obj.SourceName == "LAPA Website");
            Assert.True(obj.FundId == "Add-On Donation-4351");
            Assert.True(obj.ReferenceId == "1646223");
            Assert.True(obj.CreditInd == "N");
            Assert.True(obj.N1N2Ind == "1");
            Assert.True(obj.Solicitor == "LWebsite");
            Assert.True(obj.ContributionDesignation == "Annual Fund Support");
        }

        [Fact]
        public void DeserializeEmailAddress()
        {
            var data = ResourceManager.FileString("email_address.json");
            var obj = JsonConvert.DeserializeObject<EmailAddress>(data);

            Assert.True(obj.EmailAddressId == "3996656");
            Assert.True(obj.Address == "krzysztof.kurasz@mademedia.co.uk");
            Assert.True(obj.AddressType == "1");
            Assert.True(obj.AddressTypeDescription == "Primary Email Address");
            Assert.True(obj.EmailInd == "Y");
            Assert.True(obj.N1N2Ind == "3");
            Assert.True(obj.Months == "YYYYYYYYYYYY");
            Assert.True(obj.PrimaryInd == "Y");
            Assert.True(obj.Inactive == "N");
            Assert.True(obj.HtmlInd == "Y");
            Assert.True(obj.MarketInd == "Y");
            Assert.True(obj.Inactive1 == "N");
            Assert.True(obj.CustomerId == "1459479");
        }

        [Fact]
        public void DeserializeMembership()
        {
            var data = ResourceManager.FileString("membership.json");
            var obj = JsonConvert.DeserializeObject<Membership>(data);

            Assert.True(obj.MemberOrgId == "2");
            Assert.True(obj.CurrentStatus == "2");
            Assert.True(obj.MembershipLevel == "A16");
            Assert.True(obj.InitAt == new DateTimeOffset(
                year: 2017, month: 11, day: 30,
                hour: 0, minute: 0, second: 0,
                offset: new TimeSpan(hours: -8, minutes: 0, seconds: 0)
            ));

            Assert.True(obj.CurrentStatusDescription == "Active");
            Assert.True(obj.MembershipLevelDescription == "Prospective Friends");
            Assert.True(obj.MembershipLevelCategory == "4");
            Assert.True(obj.CategoryDescription == "Friends");
            Assert.True(obj.MembershipOrgDescription == "Annual");

            Assert.True(obj.InceptionAt == new DateTimeOffset(
                year: 2017, month: 11, day: 30,
                hour: 0, minute: 0, second: 0,
                offset: new TimeSpan(hours: -8, minutes: 0, seconds: 0)
            ));

            Assert.True(obj.BenProvider == "0");

            Assert.True(obj.LapseAt == new DateTimeOffset(
                year: 2019, month: 2, day: 28,
                hour: 23, minute: 59, second: 59, millisecond: 997,
                offset: new TimeSpan(hours: -8, minutes: 0, seconds: 0)
            ));

            Assert.True(obj.RenewalAt == new DateTimeOffset(
                year: 2018, month: 8, day: 29,
                hour: 23, minute: 59, second: 59, millisecond: 997,
                offset: new TimeSpan(hours: -7, minutes: 0, seconds: 0)
            ));


            Assert.True(obj.DeclinedInd == "Y");
            Assert.True(obj.CustomerMemberId == "365113");
        }

        [Fact]
        public void DeserializePhone()
        {
            var data = ResourceManager.FileString("phone.json");
            var obj = JsonConvert.DeserializeObject<Phone>(data);

            Assert.True(obj.PhoneId == "4813241");
            Assert.True(obj.PhoneNumber == "(555) 666-777");
            Assert.True(obj.Type == "56");
            Assert.True(obj.TypeDescription == "Primary");
            Assert.True(obj.DayInd == "E");
            Assert.True(obj.TeleInd == "Y");
            Assert.True(obj.PrimaryAddress == "N");
            Assert.True(obj.CustomerId == "1459479");
        }

        [Fact]
        public void DeserializeRanking()
        {
            var data = ResourceManager.FileString("ranking.json");
            var obj = JsonConvert.DeserializeObject<Ranking>(data);

            Assert.True(obj.RankType == "3");
            Assert.True(obj.RankTypeDescription == "CYO Subscriber");
            Assert.True(obj.Rank == "1");
        }

        [Fact]
        public void DeserializeUserDigest()
        {
            var data = ResourceManager.FileString("user_digest.json");
            var obj = JsonConvert.DeserializeObject<UserDigest>(data);

            Assert.True(obj.CustomerId == "1459479");
            Assert.True(obj.MOS == "29");
            Assert.True(obj.PromotionCode == "6259");
            Assert.True(obj.BU == "1");
            Assert.True(obj.Status == "P");
            Assert.True(obj.OriginalMOS == "29");

            Assert.True(obj.ConstituentHeader != null);
            Assert.True(obj.Constituency != null);
            Assert.True(obj.Contributions.Length == 1);
            Assert.True(obj.EmailAddresses != null);
            Assert.True(obj.Rankings != null);
            Assert.True(obj.Memberships != null);
            Assert.True(obj.Phones != null);
            Assert.True(obj.Addresses.Length == 1);



            Assert.True(obj.ConstituentHeader[0].CustomerId == "1459479");
            Assert.True(obj.ConstituentHeader[0].FullName1 == "Krzysztof Kurasz                                       ");
            Assert.True(obj.ConstituentHeader[0].AllConst == "WCC");
            Assert.True(obj.ConstituentHeader[0].MemberLevel == "A16");
            Assert.True(obj.ConstituentHeader[0].CurrentStatus == "Active");

            Assert.True(obj.ConstituentHeader[0].MemberExpiration == new DateTimeOffset(
                year: 2018, month: 11, day: 29,
                hour: 23, minute: 59, second: 59, millisecond: 997,
                offset: new TimeSpan(hours: -8, minutes: 0, seconds: 0)
            ));

            Assert.True(obj.ConstituentHeader[0].CreatedAt == new DateTimeOffset(
                year: 2016, month: 10, day: 17,
                hour: 6, minute: 7, second: 9, millisecond: 63,
                offset: new TimeSpan(hours: -7, minutes: 0, seconds: 0)
            ));
            Assert.True(obj.ConstituentHeader[0].Donation == "2283.0000");
            Assert.True(obj.ConstituentHeader[0].UserId == "webapi  ");
            Assert.True(obj.ConstituentHeader[0].OnAccount == "30.0000");
            Assert.True(obj.ConstituentHeader[0].CustomerType == "1");
            Assert.True(obj.ConstituentHeader[0].CustomerGroup == "1");
            Assert.True(obj.ConstituentHeader[0].WorkerInd == "N");



            Assert.True(obj.Constituency[0].Value == "WD Current CYO Buyer");
            Assert.True(obj.Constituency[0].CreatedBy == "webapi  ");

            Assert.True(obj.Constituency[0].CreatedAt == new DateTimeOffset(
                year: 2017, month: 12, day: 3,
                hour: 11, minute: 22, second: 27, millisecond: 250,
                offset: new TimeSpan(hours: -8, minutes: 0, seconds: 0)
            ));

            Assert.True(obj.Constituency[0].N1N2Ind == "1");
            Assert.True(obj.Constituency[0].Id == "41");
            Assert.True(obj.Constituency[0].ShortDescription == "WCC");



            Assert.True(obj.Contributions[0].CustomerId == "1459479");
            Assert.True(obj.Contributions[0].ContributionDate == new DateTimeOffset(
                year: 2017, month: 11, day: 30,
                hour: 4, minute: 44, second: 40, millisecond: 267,
                offset: new TimeSpan(hours: -8, minutes: 0, seconds: 0)
            ));

            Assert.True(obj.Contributions[0].RecordAmount == 8.0);
            Assert.True(obj.Contributions[0].ContributionAmount == 8.0);
            Assert.True(obj.Contributions[0].Type == "Gift");
            Assert.True(obj.Contributions[0].Campaign == "2018 Individual");
            Assert.True(obj.Contributions[0].Appeal == "LAPA Website");
            Assert.True(obj.Contributions[0].MediaType == "Website");
            Assert.True(obj.Contributions[0].SourceName == "LAPA Website");
            Assert.True(obj.Contributions[0].FundId == "Add-On Donation-4351");
            Assert.True(obj.Contributions[0].ReferenceId == "1646223");
            Assert.True(obj.Contributions[0].CreditInd == "N");
            Assert.True(obj.Contributions[0].N1N2Ind == "1");
            Assert.True(obj.Contributions[0].Solicitor == "LWebsite");
            Assert.True(obj.Contributions[0].ContributionDesignation == "Annual Fund Support");



            Assert.True(obj.EmailAddresses[0].EmailAddressId == "3996656");
            Assert.True(obj.EmailAddresses[0].Address == "krzysztof.kurasz@mademedia.co.uk");
            Assert.True(obj.EmailAddresses[0].AddressType == "1");
            Assert.True(obj.EmailAddresses[0].AddressTypeDescription == "Primary Email Address");
            Assert.True(obj.EmailAddresses[0].EmailInd == "Y");
            Assert.True(obj.EmailAddresses[0].N1N2Ind == "3");
            Assert.True(obj.EmailAddresses[0].Months == "YYYYYYYYYYYY");
            Assert.True(obj.EmailAddresses[0].PrimaryInd == "Y");
            Assert.True(obj.EmailAddresses[0].Inactive == "N");
            Assert.True(obj.EmailAddresses[0].HtmlInd == "Y");
            Assert.True(obj.EmailAddresses[0].MarketInd == "Y");
            Assert.True(obj.EmailAddresses[0].Inactive1 == "N");
            Assert.True(obj.EmailAddresses[0].CustomerId == "1459479");



            Assert.True(obj.Rankings[0].RankType == "3");
            Assert.True(obj.Rankings[0].RankTypeDescription == "CYO Subscriber");
            Assert.True(obj.Rankings[0].Rank == "1");



            Assert.True(obj.Memberships[0].MemberOrgId == "2");
            Assert.True(obj.Memberships[0].CurrentStatus == "2");
            Assert.True(obj.Memberships[0].MembershipLevel == "A16");
            Assert.True(obj.Memberships[0].InitAt == new DateTimeOffset(
                year: 2017, month: 11, day: 30,
                hour: 0, minute: 0, second: 0,
                offset: new TimeSpan(hours: -8, minutes: 0, seconds: 0)
            ));

            Assert.True(obj.Memberships[0].CurrentStatusDescription == "Active");
            Assert.True(obj.Memberships[0].MembershipLevelDescription == "Prospective Friends");
            Assert.True(obj.Memberships[0].MembershipLevelCategory == "4");
            Assert.True(obj.Memberships[0].CategoryDescription == "Friends");
            Assert.True(obj.Memberships[0].MembershipOrgDescription == "Annual");

            Assert.True(obj.Memberships[0].InceptionAt == new DateTimeOffset(
                year: 2017, month: 11, day: 30,
                hour: 0, minute: 0, second: 0,
                offset: new TimeSpan(hours: -8, minutes: 0, seconds: 0)
            ));

            Assert.True(obj.Memberships[0].BenProvider == "0");

            Assert.True(obj.Memberships[0].LapseAt == new DateTimeOffset(
                year: 2019, month: 2, day: 28,
                hour: 23, minute: 59, second: 59, millisecond: 997,
                offset: new TimeSpan(hours: -8, minutes: 0, seconds: 0)
            ));

            Assert.True(obj.Memberships[0].RenewalAt == new DateTimeOffset(
                year: 2018, month: 8, day: 29,
                hour: 23, minute: 59, second: 59, millisecond: 997,
                offset: new TimeSpan(hours: -7, minutes: 0, seconds: 0)
            ));


            Assert.True(obj.Memberships[0].DeclinedInd == "Y");
            Assert.True(obj.Memberships[0].CustomerMemberId == "365113");



            Assert.True(obj.Phones[0].PhoneId == "4813241");
            Assert.True(obj.Phones[0].PhoneNumber == "(555) 666-777");
            Assert.True(obj.Phones[0].Type == "56");
            Assert.True(obj.Phones[0].TypeDescription == "Primary");
            Assert.True(obj.Phones[0].DayInd == "E");
            Assert.True(obj.Phones[0].TeleInd == "Y");
            Assert.True(obj.Phones[0].PrimaryAddress == "N");
            Assert.True(obj.Phones[0].CustomerId == "1459479");



            Assert.True(obj.Addresses[0].AddressId == "3996655");
            Assert.True(obj.Addresses[0].CustomerId == "1459479");
            Assert.True(obj.Addresses[0].AddressType == "3");
            Assert.True(obj.Addresses[0].AddressTypeDescription == "Home Address");
            Assert.True(obj.Addresses[0].Street1 == "97 Icknield Street");
            Assert.True(obj.Addresses[0].Street2 == null);
            Assert.True(obj.Addresses[0].City == "Birmingham");
            Assert.True(obj.Addresses[0].State == "West Midlands");
            Assert.True(obj.Addresses[0].PostalCode == "B18 6RU");
            Assert.True(obj.Addresses[0].PostalCodeFormat == "B18 6RU");
            Assert.True(obj.Addresses[0].CountryId == "209");
            Assert.True(obj.Addresses[0].CountryLong == "United Kingdom");
            Assert.True(obj.Addresses[0].CountryShort == "UK ");
            Assert.True(obj.Addresses[0].Start == new DateTimeOffset(
                year: 2017, month: 9, day: 19,
                hour: 3, minute: 5, second: 5,
                offset: new TimeSpan(hours: -7, minutes: 0, seconds: 0)
            ));
            Assert.True(obj.Addresses[0].End == new DateTimeOffset(
                year: 2017, month: 9, day: 19,
                hour: 3, minute: 5, second: 5,
                offset: new TimeSpan(hours: -7, minutes: 0, seconds: 0)
            ));
            Assert.True(obj.Addresses[0].Months == "YYYYYYYYYYYY");
            Assert.True(obj.Addresses[0].PrimaryInd == "N");
            Assert.True(obj.Addresses[0].Inactive == "Y");
            Assert.True(obj.Addresses[0].GeoArea == "4");

        }
    }
}
