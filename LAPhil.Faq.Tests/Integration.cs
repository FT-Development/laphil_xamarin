using System;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Xunit;
using LAPhil.Application;
using LAPhil.Faq;


namespace LAPhil.Faq.Tests
{
    public class Integration: IClassFixture<ConfigureServices>
    {
        [Fact]
        public async Task FetchFaqs()
        {
            var faqService = ServiceContainer.Resolve<FaqService>();
            var result = await faqService.GetFaqs();

            Assert.True(result.Length > 0);
        }

        //[Fact]
        public async Task SortByCategory()
        {
            var faqService = ServiceContainer.Resolve<FaqService>();
            var result = await faqService.GetFaqs();

            var groupedResult = result.GroupBy(
                keySelector: faq => faq.Headline.Category.Label, 
                elementSelector: faq => faq
            );
        }
    }
}
