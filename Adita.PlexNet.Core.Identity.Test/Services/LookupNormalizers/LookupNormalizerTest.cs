namespace Adita.PlexNet.Core.Identity.Test.Services.LookupNormalizers
{
    [TestClass]
    public class LookupNormalizerTest
    {
        [TestMethod]
        public void CanNormalize()
        {
            UpperInvariantLookupNormalizer lookupNormalizer = new();
            string name = "Setya Adi Kurnia";
            string email = "setyaadikurnia@hotmail.com";

            Assert.IsFalse(lookupNormalizer.NormalizeName(name) == name);
            Assert.IsFalse(lookupNormalizer.NormalizeName(email) == email);
            Assert.AreEqual(lookupNormalizer.NormalizeName(name), "SETYA ADI KURNIA");
            Assert.AreEqual(lookupNormalizer.NormalizeName(email), "SETYAADIKURNIA@HOTMAIL.COM");
        }
    }
}
