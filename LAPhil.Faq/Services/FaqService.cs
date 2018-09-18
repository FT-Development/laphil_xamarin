using System;


namespace LAPhil.Faq
{
    public class FaqService
    {
        public readonly IFaqDriver Driver;

        public FaqService(IFaqDriver driver)
        {
            Driver = driver;
        }

        public IObservable<Faq[]> GetFaqs()
        {
            return Driver.GetFaqs();
        }
    }
}
