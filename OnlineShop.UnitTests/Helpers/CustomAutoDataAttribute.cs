using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoFixture;

namespace OnlineShop.UnitTests.Helpers
{
    public class CustomAutoDataAttribute : AutoDataAttribute
    {
        public CustomAutoDataAttribute() : base(CreateFixture) { }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture();

            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true, GenerateDelegates = true });

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        }
    }
}
