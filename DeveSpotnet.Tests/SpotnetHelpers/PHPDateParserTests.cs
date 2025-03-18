using DeveSpotnet.SpotnetHelpers;

namespace DeveSpotnet.Tests.SpotnetHelpers
{
    [TestClass]
    public class PHPDateParserTests
    {
        [DataTestMethod]
        [DataRow("Sun, 10 Mar 19 20:11:41 UTC", 0, 2019, 3, 10, 20, 11, 41)]
        [DataRow("Tue, 12 Mar 2019 11:03:24 -0700 (PDT)", -7, 2019, 3, 12, 11, 03, 24)]
        [DataRow("20 Jul 16 10:47 CEST", 2, 2016, 7, 20, 10, 47, 0)]
        [DataRow("20 Jul 16 10:56 CEST", 2, 2016, 7, 20, 10, 56, 0)]
        [DataRow("18 Sep 16 22:44 CEST", 2, 2016, 9, 18, 22, 44, 0)]
        [DataRow("16 Oct 16 17:52 UTC", 0, 2016, 10, 16, 17, 52, 0)]
        [DataRow("16 Oct 16 18:15 UTC", 0, 2016, 10, 16, 18, 15, 0)]
        [DataRow("03 Dec 16 16:32 UTC", 0, 2016, 12, 3, 16, 32, 0)]
        [DataRow("11 Jan 17 14:43 UTC", 0, 2017, 1, 11, 14, 43, 0)]
        [DataRow("23 Apr 17 20:07:12 UTC", 0, 2017, 4, 23, 20, 07, 12)]
        [DataRow("24 Apr 17 21:32:26 CEST", 2, 2017, 4, 24, 21, 32, 26)]
        [DataRow("24 Apr 17 21:34:48 CEST", 2, 2017, 4, 24, 21, 34, 48)]
        [DataRow("28 Apr 17 23:39:10 CEST", 2, 2017, 4, 28, 23, 39, 10)]
        [DataRow("10 May 17 21:07:13 UTC", 0, 2017, 5, 10, 21, 07, 13)]
        [DataRow("25 Jun 17 22:48 CEST", 2, 2017, 6, 25, 22, 48, 0)]
        [DataRow("25 Jun 17 22:50 CEST", 2, 2017, 6, 25, 22, 50, 0)]
        [DataRow("25 Jun 17 20:52 UTC", 0, 2017, 6, 25, 20, 52, 0)]
        [DataRow("Mon, 26 Feb 18 12:45:57 CET", 1, 2018, 2, 26, 12, 45, 57)]
        [DataRow("Mon, 26 Feb 18 12:51:26 CET", 1, 2018, 2, 26, 12, 51, 26)]
        [DataRow("Mon, 26 Feb 18 13:15:18 CET", 1, 2018, 2, 26, 13, 15, 18)]
        [DataRow("Thu, 01 Mar 18 19:25:21 CET", 1, 2018, 3, 1, 19, 25, 21)]
        [DataRow("Sat, 03 Mar 18 10:31:17 CET", 1, 2018, 3, 3, 10, 31, 17)]
        [DataRow("Sat, 03 Mar 18 10:52:47 CET", 1, 2018, 3, 3, 10, 52, 47)]
        [DataRow("Mon, 05 Mar 18 11:49:03 CET", 1, 2018, 3, 5, 11, 49, 03)]
        [DataRow("Mon, 12 Mar 18 09:29:39 CET", 1, 2018, 3, 12, 09, 29, 39)]
        [DataRow("Mon, 12 Mar 18 17:24:58 CET", 1, 2018, 3, 12, 17, 24, 58)]
        [DataRow("Fri, 16 Mar 18 23:35:32 CET", 1, 2018, 3, 16, 23, 35, 32)]
        [DataRow("Sun, 18 Mar 18 13:32:57 CET", 1, 2018, 3, 18, 13, 32, 57)]
        [DataRow("Sun, 18 Mar 18 16:45:49 CET", 1, 2018, 3, 18, 16, 45, 49)]
        [DataRow("Wed, 28 Mar 18 12:03:28 CEST", 2, 2018, 3, 28, 12, 03, 28)]
        [DataRow("Thu, 29 Mar 18 10:02:24 CEST", 2, 2018, 3, 29, 10, 02, 24)]
        [DataRow("Thu, 29 Mar 18 10:13:28 CEST", 2, 2018, 3, 29, 10, 13, 28)]
        public void TryParseNntpDate_ValidDates_ReturnsExpectedResult(
            string input, int expectedOffsetHours,
            int expectedYear, int expectedMonth, int expectedDay,
            int expectedHour, int expectedMinute, int expectedSecond)
        {
            bool success = PHPDateParser.TryParseNntpDate(input, out DateTimeOffset result);
            Assert.IsTrue(success, $"Failed to parse: {input}");

            // Verify the result has the expected time zone offset.
            Assert.AreEqual(TimeSpan.FromHours(expectedOffsetHours), result.Offset, $"Offset mismatch for {input}");

            // Convert the result to the expected offset and check individual date components.
            DateTimeOffset converted = result.ToOffset(TimeSpan.FromHours(expectedOffsetHours));
            Assert.AreEqual(expectedYear, converted.Year, $"Year mismatch for {input}");
            Assert.AreEqual(expectedMonth, converted.Month, $"Month mismatch for {input}");
            Assert.AreEqual(expectedDay, converted.Day, $"Day mismatch for {input}");
            Assert.AreEqual(expectedHour, converted.Hour, $"Hour mismatch for {input}");
            Assert.AreEqual(expectedMinute, converted.Minute, $"Minute mismatch for {input}");
            Assert.AreEqual(expectedSecond, converted.Second, $"Second mismatch for {input}");
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(null)]
        [DataRow("Not a date")]
        public void TryParseNntpDate_InvalidDates_ReturnsFalse(string input)
        {
            bool success = PHPDateParser.TryParseNntpDate(input, out DateTimeOffset result);
            Assert.IsFalse(success, $"Unexpectedly parsed invalid date: {input}");
        }
    }
}