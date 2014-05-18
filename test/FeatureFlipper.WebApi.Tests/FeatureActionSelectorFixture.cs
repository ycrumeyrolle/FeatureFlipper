namespace FeatureFlipper.WebApi.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using Moq;
    using Xunit;

    public class FeatureActionSelectorFixture
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new FeatureActionSelector(null));
        }

        [Fact]
        public void GetActionMapping()
        {
            // Arrange
            var expectedMapping = new Mock<ILookup<string, HttpActionDescriptor>>().Object;
            Mock<IHttpActionSelector> inner = new Mock<IHttpActionSelector>(MockBehavior.Strict);
            var selector = new FeatureActionSelector(inner.Object);
            inner
                .Setup(s => s.GetActionMapping(It.IsAny<HttpControllerDescriptor>()))
                .Returns(expectedMapping);

            Mock<HttpControllerDescriptor> descriptor = new Mock<HttpControllerDescriptor>();

            // Act 
            var mapping = selector.GetActionMapping(descriptor.Object);

            // Assert
            Assert.NotNull(mapping);
            Assert.Same(expectedMapping, mapping);
            inner.Verify(s => s.GetActionMapping(It.IsAny<HttpControllerDescriptor>()), Times.Once());
        }

        [Fact]
        public void SelectAction_GuardClause()
        {
            // Arrange
            Mock<IHttpActionSelector> inner = new Mock<IHttpActionSelector>(MockBehavior.Strict);
            var selector = new FeatureActionSelector(inner.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => selector.SelectAction(null));
        }

        [Theory]
        [MemberData("GetSelectActionParameters")]
        public void SelectAction_ReturnsActionDescriptor(FeatureAttribute[] attributes)
        {
            // Arrange
            var expectedActionDescriptor = new Mock<HttpActionDescriptor>(MockBehavior.Strict);
            expectedActionDescriptor
                .Setup(d => d.GetCustomAttributes<FeatureAttribute>())
                .Returns(new Collection<FeatureAttribute>(new[] { new FeatureAttribute("X") }));
            Mock<IHttpActionSelector> inner = new Mock<IHttpActionSelector>(MockBehavior.Strict);
            inner
                .Setup(s => s.SelectAction(It.IsAny<HttpControllerContext>()))
                .Returns(expectedActionDescriptor.Object);
            var selector = new FeatureActionSelector(inner.Object);
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);
            bool isOn = true;
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), out isOn))
                .Returns(true);
            Features.Flipper = flipper.Object;

            Mock<HttpControllerContext> context = new Mock<HttpControllerContext>();

            // Act 
            var actionDescriptor = selector.SelectAction(context.Object);

            // Assert
            Assert.NotNull(actionDescriptor);
            Assert.Equal(expectedActionDescriptor.Object, actionDescriptor);
        }

        [Fact]
        public void SelectAction_ThrowsHttpResponseException_Http410()
        {
            // Arrange
            var expectedActionDescriptor = new Mock<HttpActionDescriptor>(MockBehavior.Strict);
            expectedActionDescriptor
                .Setup(d => d.GetCustomAttributes<FeatureAttribute>())
                .Returns(new Collection<FeatureAttribute>(new[] { new FeatureAttribute("X") }));
            Mock<IHttpActionSelector> inner = new Mock<IHttpActionSelector>(MockBehavior.Strict);
            inner
                .Setup(s => s.SelectAction(It.IsAny<HttpControllerContext>()))
                .Returns(expectedActionDescriptor.Object);
            var selector = new FeatureActionSelector(inner.Object);
            Mock<IFeatureFlipper> flipper = new Mock<IFeatureFlipper>(MockBehavior.Strict);
            bool isOn = false;
            flipper
                .Setup(f => f.TryIsOn(It.IsAny<string>(), out isOn))
                .Returns(true);
            Features.Flipper = flipper.Object;

            Mock<HttpControllerContext> context = new Mock<HttpControllerContext>();
            context.Object.Request = new HttpRequestMessage();

            // Act & Assert
            var exception = Assert.Throws<HttpResponseException>(() => selector.SelectAction(context.Object));

            Assert.NotNull(exception.Response);
            Assert.Equal(HttpStatusCode.Gone, exception.Response.StatusCode);
        }

        public static IEnumerable<object[]> GetSelectActionParameters()
        {
            yield return new object[] { new FeatureAttribute[0] };
            yield return new object[] { new[] { new FeatureAttribute("X") } };
        }
    }
}
