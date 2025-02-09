using CustomerManagement.View.UserControls;

namespace CustomerManagement.Test
{
    [Apartment(ApartmentState.STA)]
    public class FooterBarTest
    {
        [Test]
        public void TestGetMachineName()
        {
            string machineName = Environment.MachineName;
            Assert.That(FooterBar.MachineName, Is.EqualTo(machineName));
        }


        [Test]
        public void TestGetVersionNumber_ShouldReturnVersionNumber()
        {
            FooterBar testFooterBar = new FooterBar();

            string? versionNumber = testFooterBar.VersionNumber;

            const string expectedVersionNumber = "1.0.0.0";

            Assert.That(versionNumber, Is.EqualTo(expectedVersionNumber));
        }
    }
}
